// MessageType -- Int16 enum used to represent game message type. 
// Copyright 2015 Austin Chambers 
using System;

namespace Game.Messaging
{
    /// <summary>
    /// Type of message, underlying type is 16 bits. 
    /// </summary>
    [Serializable]
    public enum MessageType : short
    {
        // Just to demonstrate that server is actually deserializing non-default value. 
        OtherMessageType = 0,
        
        ClientStateUpdate = 1,
    }
}
