using System;
using System.Linq;


using Org.Apache.Qpid.Messaging;

namespace QpidNetDemo.Connection.ApacheQpid
{
    /// <summary>
    /// Connects to an AMQP broker by using Apache Qpid.
    /// </summary>
    public class ApacheQpidConnectionProvider : IAmqpConnectionProvider, ISessionReceiver
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ApacheQpidConnectionProvider));
        private readonly Org.Apache.Qpid.Messaging.Connection connection;
        private readonly Session session;
        private readonly CallbackServer callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApacheQpidConnectionProvider"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        public ApacheQpidConnectionProvider(string hostName, string port)
        {
            string broker = string.Format("{0}:{1}", hostName, port);

            connection = null;
            try
            {
                connection = new Org.Apache.Qpid.Messaging.Connection(broker, "{protocol: amqp1.0}");
                connection.SetOption("sasl_mechanisms", "PLAIN");
                connection.SetOption("username", "guest");
                connection.SetOption("password", "guest");
                connection.Open();
                session = connection.CreateSession();
            }
            catch (Exception e)
            {
                Logger.Error("Could not connect to " + hostName + ":" + port);
                Console.WriteLine("Exception {0}.", e);
                if (null != connection)
                {
                    connection.Close();
                }

                return;
            }

            callback = new CallbackServer(session, this);
        }

        /// <summary>Gets or sets the message received callback.</summary>
        public Action<Message> MessageReceivedCallback { get; set; }

        /// <summary>Sends a message.</summary>
        /// <param name="messageContent">Content of the message.</param>
        /// <param name="queueName">Name of the queue.</param>
        public void SendMessage(string messageContent, string queueName)
        {
            if (session != null)
            {
                var address = string.Format("{0}; {{create: always, node:{{ type: queue, durable: true }}}}", queueName);
                Sender sender = session.CreateSender(address);
                sender.Send(new Org.Apache.Qpid.Messaging.Message(messageContent));
            }
            else
            {
                Logger.Error("Could not send on queue '" + queueName + "' : Invalid Session");
            }
        }

        /// <summary>Sends a message to the AMQP broker.</summary>
        /// <param name="message">The message.</param>
        /// <param name="queueName">Name of the target queue.</param>
        public void SendMessage(Message message, string queueName)
        {
            if (session != null)
            {
                var address = string.Format("{0}; {{create: always, node:{{ type: queue, durable: true }}}}", queueName);
                Sender sender = session.CreateSender(address);

                var qpidMessage = new Org.Apache.Qpid.Messaging.Message();
                qpidMessage.SetContent(message.ContentBytes);
                foreach (var property in message.Properties)
                {
                    qpidMessage.SetProperty(property.Key, property.Value);
                }

                sender.Send(qpidMessage);
            }
            else
            {
                Logger.Error("Could not send on queue '" + queueName + "' : Invalid Session");
            }
        }

        /// <summary>Closes a connection.</summary>
        public void Close()
        {
            if (callback != null)
            {
                callback.Close();
            }

            connection.Close();
        }

        /// <summary>Subscribes to a queue.</summary>
        /// <param name="queueName">Name of the queue.</param>
        public void SubscribeQueue(string queueName)
        {
            var address = string.Format("{0}; {{create: always, node:{{ type: queue, durable: true }}}}", queueName);
            if (session != null)
            {
                var receiver = session.CreateReceiver(address);
                receiver.Capacity = 100;
            }
            else
            {
                Logger.Error("Could not subscribe: Invalid Session");
            }
        }

        /// <summary>Called when a message has been received.</summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        public void MessageReceived(Receiver receiver, Org.Apache.Qpid.Messaging.Message message)
        {
            var contentBytes = new byte[message.ContentSize];
            message.GetContent(contentBytes);

            var amqpMessage = new Message
            {
                ContentBytes = contentBytes,
                ContentType = message.ContentType,
                Properties = message.Properties.ToDictionary(p => p.Key, p => p.Value)
            };

            MessageReceivedCallback(amqpMessage);
            receiver.Session.Acknowledge(message);
        }

        /// <summary>Called when an exception occurred in the session.</summary>
        /// <param name="exception">The exception.</param>
        public void SessionException(Exception exception)
        {
            Logger.Error("Error in callback server", exception);
        }
    }
}