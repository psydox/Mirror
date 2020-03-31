using NUnit.Framework;
using UnityEngine;

namespace Mirror.Tests
{
    internal class MyScriptableObject : ScriptableObject
    {
        public int someData;
    }

    [TestFixture]
    public class ScriptableObjectWriterTest
    {

        // ArraySegment<byte> is a special case,  optimized for no copy and no allocation
        // other types are generated by the weaver


        class ScriptableObjectMessage : MessageBase
        {
            public MyScriptableObject scriptableObject;
        }

        [Test]
        public void TestWriteScriptableObject()
        {
            ScriptableObjectMessage message = new ScriptableObjectMessage
            {
                scriptableObject = ScriptableObject.CreateInstance<MyScriptableObject>()
            };

            message.scriptableObject.someData = 10;

            byte[] data = MessagePacker.Pack(message);

            ScriptableObjectMessage unpacked = MessagePacker.Unpack<ScriptableObjectMessage>(data);

            Assert.That(unpacked.scriptableObject, Is.Not.Null);
            Assert.That(unpacked.scriptableObject.someData, Is.EqualTo(10));
        }

    }
}
