using System.Collections.Generic;

namespace QpidNetDemo.Connection
{
    /// <summary>
    /// Message that can be sent to or received from an AMQP broker.
    /// </summary>
    public class Message
    {
        /// <summary>Gets or sets the content type.</summary>
        public string ContentType { get; set; }

        /// <summary>Gets or sets the correlation ID.</summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Message"/> is durable.
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>Gets or sets the message ID.</summary>
        public string MessageId { get; set; }

        /// <summary>Gets or sets the priority.</summary>
        public byte Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Message"/> is redelivered.
        /// </summary>
        public bool Redelivered { get; set; }

        /// <summary>Gets or sets the subject.</summary>
        public string Subject { get; set; }

        /// <summary>Gets or sets the user identifier.</summary>
        public string UserId { get; set; }

        /// <summary>Gets or sets the content bytes.</summary>
        public byte[] ContentBytes { get; set; }

        /// <summary>Gets or sets the properties.</summary>
        public Dictionary<string, object> Properties { get; set; }
    }
}
