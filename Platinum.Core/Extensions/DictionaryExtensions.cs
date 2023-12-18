using Anhny010920.Core.Extensions;
using Newtonsoft.Json.Linq;
using Platinum.Core.Attributes;
using Platinum.Core.Extensions;
using Platinum.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Utilities.Web;

namespace Platinum.Core.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary instance to a NameValueCollection.
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string> input)
        {
            var collection = new NameValueCollection();

            foreach (var item in input)
            {
                collection.Add(item.Key, item.Value);
            }

            return collection;
        }

        /// <summary>
        /// Adds items from a <see cref="IEnumerable{T}"/> to the <see cref="Dictionary{TKey,TValue}"/>.
        /// </summary>
        public static void AddMany<TKey, TValue>(this Dictionary<TKey, TValue> input, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (KeyValuePair<TKey, TValue> item in items)
            {
                input.Add(item.Key, item.Value);
            }
        }

        public static Dictionary<string, string> ToEnumContentDictionary(this Enum source)
        {
            var enumMember = source.GetType().GetMember(source.ToString()).FirstOrDefault();

            var descriptionAttributes =
            enumMember == null
            ? new List<EnumContentAttribute>()
            : enumMember.GetCustomAttributes(typeof(EnumContentAttribute), true).Select(a => (EnumContentAttribute)a);
            return
            descriptionAttributes == null
            ? null
            : descriptionAttributes.ToDictionary(a => a.Key, a => a.Value);
        }

        /// <summary>
        /// Converts to keyvalue.
        /// </summary>
        /// <typeparam name="T">Type of content</typeparam>
        /// <param name="metaToken">The meta token.</param>
        /// <returns>IDictionary.</returns>
        public static IDictionary<string, string> ToDictionary<T>(this T metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            if (!(metaToken is JToken token))
            {
                return ToDictionary(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToDictionary();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                jValue?.ToString("o", CultureInfo.InvariantCulture) :
                jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }


        /// <summary>
        /// Serializes the dictionary to an XML string
        /// </summary>
        /// <returns></returns>
        public static string ToXml(this IDictionary items, string root = "root")
        {
            var rootNode = new XElement(root);

            foreach (DictionaryEntry item in items)
            {
                string xmlType = XmlUtils.MapTypeToXmlType(item.Value.GetType());

                // if it's a simple type use it
                if (!string.IsNullOrEmpty(xmlType))
                {
                    XAttribute typeAttr = new XAttribute("type", xmlType);
                    rootNode.Add(
                        new XElement(item.Key as string,
                                     typeAttr,
                                     item.Value)
                        );
                }
                else
                {
                    // complex type use serialization
                    if (SerializationUtils.SerializeObject(item.Value, out string xmlString))
                    {
                        XElement el = XElement.Parse(xmlString);

                        rootNode.Add(
                            new XElement(item.Key as string,
                            new XAttribute("type", "___" + item.Value.GetType().FullName),
                            el));
                    }
                }
            }

            return rootNode.ToString();
        }

        /// <summary>
        /// Loads the dictionary from an Xml string
        /// </summary>
        /// <param name="xml"></param>
        public static void FromXml(this IDictionary items, string xml)
        {
            items.Clear();

            var root = XElement.Parse(xml);

            foreach (XElement el in root.Elements())
            {
                string typeString = null;

                var typeAttr = el.Attribute("type");
                if (typeAttr != null)
                    typeString = typeAttr.Value;

                string val = el.Value;


                if (!string.IsNullOrEmpty(typeString) && typeString != "string" && !typeString.StartsWith("__"))
                {
                    // Simple type we know how to convert
                    Type type = XmlUtils.MapXmlTypeToType(typeString);
                    if (type != null)
                        items.Add(el.Name.LocalName, ReflectionUtils.StringToTypedValue(val, type));
                    else
                        items.Add(el.Name.LocalName, val);
                }
                else if (typeString.StartsWith("___"))
                {
                    Type type = ReflectionUtils.GetTypeFromName(typeString.Substring(3));
                    object serializationUtilsDeSerializeObject = SerializationUtils.DeSerializeObject(el.Elements().First().CreateReader(), type);
                    items.Add(el.Name.LocalName, serializationUtilsDeSerializeObject);
                }
                else
                    // it's a string or unknown type
                    items.Add(el.Name.LocalName, val);
            }
        }
    }
}
