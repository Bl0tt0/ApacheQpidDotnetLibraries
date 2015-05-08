using System.Threading.Tasks;

using Org.Apache.Qpid.Messaging;

namespace QpidNetDemo.Connection.ApacheQpid
{
    /// <summary>Server that users instantiate to connect a SessionReceiver
    /// callback to the stream of received messages received on a Session.
    /// </summary>
    public class CallbackServer
    {
        private readonly EventEngine ee;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackServer"/> class.
        /// </summary>
        /// <param name="session">The Session whose messages are collected.</param>
        /// <param name="callback">The user function call with each message.</param>
        public CallbackServer(Session session, ISessionReceiver callback)
        {
            ee = new EventEngine(session, callback);

            Task.Run(() => ee.Open());
        }

        /// <summary>
        /// Function to stop the server.
        /// </summary>
        public void Close()
        {
            ee.Close();
        }
    }
}