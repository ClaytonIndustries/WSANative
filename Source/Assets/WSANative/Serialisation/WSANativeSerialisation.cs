////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;
using System.Xml.Serialization;

namespace CI.WSANative.Serialisers
{
    public static class WSANativeSerialisation
    {
        /// <summary>
        /// Serialises the specified object to xml
        /// </summary>
        /// <typeparam name="T">The type of the object specified</typeparam>
        /// <param name="item">The object to serialise</param>
        /// <returns>The object expressed as xml</returns>
        public static string SerialiseToXML<T>(T item)
        {
            XmlSerializer serialiser = new XmlSerializer(item.GetType());

            using (StringWriter writer = new StringWriter())
            {
                serialiser.Serialize(writer, item);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserialises the specified xml into an object of the specified type
        /// </summary>
        /// <typeparam name="T">The type of the object contained in the xml</typeparam>
        /// <param name="xml">The xml</param>
        /// <returns>The deserialised object</returns>
        public static T DeserialiseXML<T>(string xml)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                return (T)serialiser.Deserialize(reader);
            }
        }
    }
}