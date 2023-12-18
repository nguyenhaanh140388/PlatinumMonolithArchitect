using Anhny010920.Core.Abstractions.Services;
using Anhny010920.Core.Attributes;
using AutoMapper;
using System.Reflection;
using Anhny010920.Core.Extensions;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Anhny010920.Core.Extensions
{
    public static class ObjectExtensions
    {
#if !NET5_0_OR_GREATER
        /// <summary>
        /// Performs a deep clone of a instance of an object.
        /// </summary>
        public static T DeepClone<T>(this T input)
            where T : ISerializable
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
#endif

        /// <summary>
        /// Casts an object to <typeparamref name="T"/>. This method will throw an exception if the object cannot be cast to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type which the <paramref name="input"/> should be cast to.</typeparam>
        /// <returns></returns>
        public static T As<T>(this object input)
        {
            return (T)input;
        }

        /// <summary>
        /// Tries to cast an object to <typeparamref name="T"/>. This method will return null if it cannot be cast.
        /// </summary>
        /// <typeparam name="T">Type which the <paramref name="input"/> should be cast to.</typeparam>
        /// <returns></returns>
        public static T AsOrDefault<T>(this object input)
            where T : class
        {
            return input as T;
        }

        /// <summary>
        /// Tries to cast an object to <typeparamref name="T"/>. This method will return a value from the <paramref name="defaultValueFactory"/> if the object cannot be cast.
        /// </summary>
        /// <param name="input">The object to try cast.</param>
        /// <param name="defaultValueFactory">A factory method which will return a default value.</param>
        /// <typeparam name="T">Type which the <paramref name="input"/> should be cast to.</typeparam>
        /// <returns>The original <paramref name="input"/> cast to <typeparamref name="T"/>, or a value returned by the <paramref name="defaultValueFactory"/></returns>
        public static T AsOrDefault<T>(this object input, Func<T> defaultValueFactory)
            where T : class
        {
            if (!(input is T result))
            {
                return defaultValueFactory();
            }

            return result;
        }

        /// <summary>
        /// Tries to parse this object instance as a <see cref="bool" />.
        /// </summary>
        public static bool? TryParseBoolean(this object input)
        {
            if (input is bool b)
            {
                return b;
            }

            var test = input as string;

            return test.TryParseBoolean();
        }

        /// <summary>
        /// Tries to parse this object instance as a <see cref="Guid" />.
        /// </summary>
        public static Guid? TryParseGuid(this object input)
        {
            if (input is Guid guid)
            {
                return guid;
            }

            var test = input as string;

            return test.TryParseGuid();
        }

        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static void CopyPropertiesFrom<T>(this T self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }

        }
        public static void MatchPropertiesFrom(this object self, object parent)
        {
            var childProperties = self.GetType().GetProperties();

            foreach (var childProperty in childProperties)
            {
                var currentAttribute = (MatchParentAttribute)childProperty.GetCustomAttribute(typeof(MatchParentAttribute), true);

                if (currentAttribute != null)
                {
                    var parentProperties = parent.GetType().GetProperties();
                    object parentPropertyValue = null;

                    foreach (var parentProperty in parentProperties)
                    {
                        if (parentProperty.Name == currentAttribute.ParentPropertyName)
                        {
                            if (parentProperty.PropertyType == childProperty.PropertyType)
                            {
                                parentPropertyValue = parentProperty.GetValue(parent);
                            }
                        }
                    }

                    childProperty.SetValue(self, parentPropertyValue);
                }
            }
        }
    }
}
