using System;

using Org.Apache.Qpid.Messaging;

namespace QpidNetDemo.Connection.ApacheQpid
{
    /// <summary> ISessionReceiver interface defines the callback for users to supply.
    /// Once established this callback will receive all messages for all 
    /// receivers defined by the current session.
    /// Users are expected not to 'fetch' or 'get' messages by any other means.
    /// Users must acknowledge() the Session's messages either in the callback
    /// function or by some other scheme. 
    /// </summary>
    public interface ISessionReceiver
    {
        /// <summary>Called when a message has been received.</summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        void MessageReceived(Receiver receiver, Org.Apache.Qpid.Messaging.Message message);

        /// <summary>Called when an exception occurred in the session.</summary>
        /// <param name="exception">The exception.</param>
        void SessionException(Exception exception);
    }
}