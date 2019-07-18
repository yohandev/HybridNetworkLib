using System;
using System.Collections.Generic;
using HybridNetworkLib.Generic;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    public class CobblestoneSerializer : IObjectSerializer
    {
        private readonly List<RegisteredPacket> _registeredTypes;

        public CobblestoneSerializer()
        {
            _registeredTypes = new List<RegisteredPacket>();
        }

        public void RegisterObjectType(Type type)
        {
            _registeredTypes.Add(new RegisteredPacket(type));
        }
        
        public byte[] SerializeObject(object obj)
        {
            var type = obj.GetType();
            int index;
            
            if ((index = _registeredTypes.IndexOf(type)) < 0)
            {
                Logger.Error("Attempting to serialize a non-registered object!");
                return null;
            }

            return _registeredTypes[index].Serialize(obj, (ushort)index);
        }

        public object DeserializeObject(byte[] arr)
        {
            var reader = new ByteReader(arr);
            int index = reader.ReadUShort();

            return _registeredTypes[index].Deserialize(reader);
        }
    }
}