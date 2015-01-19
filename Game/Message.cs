// Message -- Abstract class for game messages. 
// Copyright 2015 Austin Chambers 
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game.Messaging
{
    [Serializable]
    public abstract class Message
    {
        /// The message type
        private MessageType _messageType;

        /// Gets the message type
        public MessageType MessageType
        {
            get;
            private set;
        }

        // Constructor
        protected Message(MessageType messageType)
        {
            _messageType = messageType;
        }

        // Deserialize message object from memory stream
        public static T DeserializeFromStream<T>(MemoryStream zipStream)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(zipStream.GetBuffer()))
            {
                using (DeflateStream rawStream = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    return (T)formatter.Deserialize(rawStream);
                }
            }
        }

        // Serialize message object to memory stream
        public static byte[] SerializeToBytes(Message message)
        {
            using (MemoryStream rawStream = new MemoryStream())
            {
                using (DeflateStream comStream = new DeflateStream(rawStream, CompressionMode.Compress, true))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(comStream, message);
                }
                rawStream.Position = 0;
                return rawStream.GetBuffer();
            }
        }
    }
}
