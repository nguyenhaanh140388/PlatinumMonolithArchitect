using System.Runtime.Serialization;
using System.Xml;
// ReSharper disable UnusedMember.Global

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides a serializer which can write an instance of an object to binary.
    /// </summary>
    public static class BinarySerializationExtensions
    {
        /// <summary>
        /// Serializes an instance of an object to a <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// Uses DataContractSerializer for serialization.
        /// </remarks>
        /// <returns>Byte array containing serialized <typeparamref name="T"/></returns>
        public static MemoryStream SerializeToStream<T>(this T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var stream = new MemoryStream();

            using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
            {
                serializer.WriteObject(writer, obj);
            }

            return stream;
        }

        /// <summary>
        /// Serializes an instance of an object to a byte array.
        /// </summary>
        /// <remarks>
        /// Uses DataContractSerializer for serialization.
        /// </remarks>
        /// <returns>Byte array containing serialized <typeparamref name="T"/></returns>
        public static byte[] SerializeToByteArray<T>(this T obj)
        {
            return obj.SerializeToStream().ToArray();
        }

        /// <summary>
        /// Deserializes an object instance from a byte array.
        /// </summary>
        /// <returns>Deserialized instance of <typeparamref name="T"/></returns>
        public static T DeserializeFromBinary<T>(this byte[] data)
        {
            using (var stream = new MemoryStream(data))
                return stream.DeserializeFromBinary<T>();
        }

        /// <summary>
        /// Deserializes an object instances from a byte array.
        /// </summary>
        /// <returns>Deserialized instance of <typeparamref name="T"/></returns>
        public static T DeserializeFromBinary<T>(this Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
            {
                return (T)serializer.ReadObject(reader);
            }
        }
    }
}