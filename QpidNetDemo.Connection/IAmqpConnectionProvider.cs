using System;

namespace QpidNetDemo.Connection
{
    /// <summary>
    /// Provides connectivity via AMQP.
    /// </summary>
    public interface IAmqpConnectionProvider
    {
        /// <summary>Gets or sets the message received callback.</summary>
        Action<Message> MessageReceivedCallback { get; set; }

        /// <summary>Sends a string message.</summary>
        /// <param name="messageContent">Content of the message.</param>
        /// <param name="queueName">Name of the queue.</param>
        void SendMessage(string messageContent, string queueName);

        /// <summary>Sends a message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="queueName">Name of the queue.</param>
        void SendMessage(Message message, string queueName);

        /// <summary>Closes a connection.</summary>
        void Close();

        /// <summary>Subscribes to a queue.</summary>
        /// <param name="queueName">Name of the queue.</param>
        void SubscribeQueue(string queueName);
    }
}
