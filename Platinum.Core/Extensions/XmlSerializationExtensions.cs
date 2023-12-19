using System.Runtime.Serialization;
using System.Text;
using System.Xml;
// ReSharper disable UnusedMember.Global

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides the ability to serialise an object instance to and from XML.
    /// </summary>
    public static class XmlSerializationExtensions
    {
        /// <summary>
        /// Serialise an object instance to an XML string, using the default DataContractSerializer for the type. Uses UTF8 to convert to string.
        /// </summary>
        /// <returns>String containing the serialized data.</returns>
        public static string SerializeToXmlString<T>(this T obj) => Encoding.UTF8.GetString(obj.SerializeToXml());

        /// <summary>
        /// Serialise an object instance to an XML string, allowing for a DataContractSerializer to be specified. Uses UTF8 to convert to string.
        /// </summary>
        /// <returns>String containing the serialized data.</returns>
        public static string SerializeToXmlString<T>(this T obj, DataContractSerializer serializer) => Encoding.UTF8.GetString(obj.SerializeToXml(serializer));

        /// <summary>
        /// Serialise an object instance to XML, using the default DataContractSerializer for the type.
        /// </summary>
        /// <returns>Byte array containing the serialized data.</returns>
        public static byte[] SerializeToXml<T>(this T obj) => obj.SerializeToXml(new DataContractSerializer(typeof(T)));

        /// <summary>
        /// Serialise an object instance to XML, allowing for a DataContractSerializer to be specified.
        /// </summary>
        /// <returns>Byte array containing the serialized data.</returns>
        public static byte[] SerializeToXml<T>(this T obj, DataContractSerializer serializer)
        {
            var stream = new MemoryStream();

            using (var writer = XmlDictionaryWriter.CreateTextWriter(stream))
            {
                serializer.WriteObject(writer, obj);
            }

            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes an object from a byte array containing XML data.
        /// </summary>
        /// <returns>Instance of the deserialized object.</returns>
        public static T DeserializeFromXml<T>(this byte[] data) => data.DeserializeFromXml<T>(new DataContractSerializer(typeof(T)));

        /// <summary>
        /// Deserializes an object from a byte array containing XML data, using an existing DataContractSerializer.
        /// </summary>
        /// <returns>Instance of the deserialized object.</returns>
        public static T DeserializeFromXml<T>(this byte[] data, DataContractSerializer serializer)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max))
            {
                return (T)serializer.ReadObject(reader);
            }
        }
    }
}