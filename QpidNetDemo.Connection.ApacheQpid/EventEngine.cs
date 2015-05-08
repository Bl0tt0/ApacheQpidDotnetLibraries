using System;

using Org.Apache.Qpid.Messaging;

namespace QpidNetDemo.Connection.ApacheQpid
{
    /// <summary> EventEngine - wait for messages from the underlying C++ code.
    /// When available get them and deliver them via callback to our 
    /// client through the ISessionReceiver interface.
    /// This class consumes the thread that calls the Run() function.
    /// </summary>
    internal class EventEngine
    {
        private readonly Session session;
        private readonly ISessionReceiver callback;
        private bool keepRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventEngine"/> class.
        /// </summary>
        /// <param name="theSession">The session.</param>
        /// <param name="thecallback">The thecallback.</param>
        public EventEngine(Session theSession, ISessionReceiver thecallback)
        {
            session = theSession;
            callback = thecallback;
        }

        /// <summary>
        /// Function to call Session's nextReceiver, discover messages,
        /// and to deliver messages through the callback.
        /// </summary>
        public void Open()
        {
            try
            {
                keepRunning = true;
                while (keepRunning)
                {
                    Receiver receiver = session.NextReceiver(DurationConstants.SECOND);

                    if (null != receiver)
                    {
                        if (keepRunning)
                        {
                            Org.Apache.Qpid.Messaging.Message message = receiver.Fetch(DurationConstants.SECOND);
                            callback.MessageReceived(receiver, message);
                            receiver.Session.Acknowledge(message);
                        }
                    }
                    // else
                    //    receive timed out
                    //    EventEngine exits the nextReceiver() function periodically
                    //    in order to test the keepRunning flag
                }
            }
            catch (Exception e)
            {
                callback.SessionException(e);
            }

            // Private thread is now exiting.
        }

        /// <summary>
        /// Function to stop the EventEngine. Private thread will exit within
        /// one second.
        /// </summary>
        public void Close()
        {
            keepRunning = false;
        }
    }
}