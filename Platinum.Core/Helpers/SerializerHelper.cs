// <copyright file="SerializerHelper.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// SerializeFormat.
    /// </summary>
    public enum SerializeFormat
    {
        /// <summary>
        /// The XML
        /// </summary>
        XML = 1,

        /// <summary>
        /// The json
        /// </summary>
        JSON = 2,
    }

    /// <summary>
    /// SerializerHelper.
    /// </summary>
    public static class SerializerHelper
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <returns>Result.</returns>
        public static string Serialize<TEntity>(TEntity data, SerializeFormat format)
        {
            switch (format)
            {
                case SerializeFormat.XML:
                    return SerializeToXml(data);
                case SerializeFormat.JSON:
                    return SerializeToJson(data);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <returns>Result.</returns>
        public static TEntity Deserialize<TEntity>(string data, SerializeFormat format)
        {
            switch (format)
            {
                case SerializeFormat.XML:
                    return DeserializeToXml<TEntity>(data);
                case SerializeFormat.JSON:
                    return DeserializeFromJson<TEntity>(data);
                default:
                    return default;
            }
        }

        /*public static string SerializeToXml<TEntity>(T data)
        {
            var serializer = new DataContractSerializer(data.GetType());
            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder);
            serializer.WriteObject(writer, data);
            writer.Flush();
            return builder.ToString();
        }*/

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Result.</returns>
        private static string SerializeToXml<TEntity>(TEntity data)
        {
            var xmlSerializer = new XmlSerializer(typeof(TEntity));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        /*public static T DeserializeToXml<TEntity>(string data)
        {
            var serializer = new DataContractSerializer(data.GetType());
            var writer = XmlReader.Create(GenerateStreamFromString(data));
            var result = serializer.ReadObject(writer);
            return (T)result;
        }*/

        /// <summary>
        /// Deserializes to XML.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Result.</returns>
        private static TEntity DeserializeToXml<TEntity>(string data)
        {
            var xmlSerializer = new XmlSerializer(data.GetType());
            var stream = GenerateStreamFromString(data);
            var result = xmlSerializer.Deserialize(stream);
            return (TEntity)result;
        }

        /// <summary>
        /// Generates the stream from string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>Result.</returns>
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Result.</returns>
        private static string SerializeToJson<TEntity>(TEntity data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        /// <summary>
        /// Deserializes from json.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Result.</returns>
        private static TEntity DeserializeFromJson<TEntity>(string data)
        {
            return JsonConvert.DeserializeObject<TEntity>(data);
        }
    }
}
