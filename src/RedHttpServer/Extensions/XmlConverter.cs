using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Red.Interfaces;

namespace Red.Extensions
{
    /// <summary>
    ///     Very simple XmlConverter plugin using System.Xml.Serialization
    /// </summary>
    internal sealed class XmlConverter : IXmlConverter, IRedExtension
    {
        /// <inheritdoc />
        public string Serialize<T>(T obj)
        {
            try
            {
                using (var stream = new MemoryStream())
                using (var xml = new XmlTextWriter(stream, new UTF8Encoding(false)))
                {
                    var xs = new XmlSerializer(typeof(T));
                    xs.Serialize(xml, obj);
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            { 
                return string.Empty; 
            }
        }
        
        /// <inheritdoc />
        public T Deserialize<T>(string xmlData)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var stringReader = new StringReader(xmlData))
                {
                    return (T)xmlSerializer.Deserialize(stringReader);
                }
            }
            catch (Exception)
            { 
                return default; 
            }
        }
        
        /// <inheritdoc />
        public T Deserialize<T>(Stream xmlStream)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var stringReader = new StreamReader(xmlStream))
                {
                    return (T)xmlSerializer.Deserialize(stringReader);
                }
            }
            catch (Exception)
            { 
                return default; 
            }
        }

        public void Initialize(RedHttpServer server)
        {
            server.Plugins.Register<IXmlConverter, XmlConverter>(this);
        }
    }
}