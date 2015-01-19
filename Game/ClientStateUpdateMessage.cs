// ClientStateUpdateMessage -- Message to inform game server of new client state. 
// Copyright 2015 Austin Chambers 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Game.Messaging
{
    [Serializable]
    public class ClientStateUpdateMessage : Message
    {
        public byte _version;
        public Int16 _userID;
        public string _payload;

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public byte Version
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user ID
        /// </summary>
        public Int16 UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets message payload
        /// </summary>
        public string Payload
        {
            get;
            set;
        }

        // Public default constructor, needed for serialization
        public ClientStateUpdateMessage()
            : base(MessageType.ClientStateUpdate)
        {
        }

        // Public default constructor, needed for serialization
        public ClientStateUpdateMessage(byte version, Int16 userID, string payload)
            : base(MessageType.ClientStateUpdate)
        {
            this.Version = version;
            this.UserID = userID;
            this.Payload = payload;
        }

        // Convenient string representation of message. 
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.AppendLine(string.Format("MessageType: {0}({1})", this.MessageType, (Int16)this.MessageType));
            buf.AppendLine(string.Format("Version: {0}", this.Version));
            buf.AppendLine(string.Format("UserID: {0}", this.UserID));
            buf.AppendLine(string.Format("Payload:\n{0}", this.Payload));
            return buf.ToString();
        }
    }
}
