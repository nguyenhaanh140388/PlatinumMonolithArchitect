using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Anhny010920.Core.Utilities
{
    public static class SerializeUtils
    {        /// <summary>
             /// Objects to byte array.
             /// </summary>
             /// <param name="obj">The object.</param>
             /// <returns>Result.</returns>
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {


#pragma warning disable SYSLIB0011
                bf.Serialize(ms, obj);
#pragma warning restore SYSLIB0011
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the specified parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Result.</returns>
        public static T Deserialize<T>(byte[] param)
        {
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();


#pragma warning disable SYSLIB0011
                return (T)br.Deserialize(ms);
#pragma warning restore SYSLIB0011
            }
        }
    }
}
