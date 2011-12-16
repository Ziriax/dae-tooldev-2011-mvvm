using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DogFight
{
    [DataContract]
    public class World
    {
        [DataMember]
        public readonly List<Fighter> Fighters = new List<Fighter>();

        #region Serialization
        private static DataContractSerializer CreateSerializer()
        {
            var serializer = new DataContractSerializer(
                typeof(World),
                new[] { typeof(Fighter) },
                int.MaxValue,
                true,
                true,
                null);

            return serializer;
        }

        public void SaveTo(Stream stream)
        {
            var serializer = CreateSerializer();

            var xmlSettings = new XmlWriterSettings
                {
                    CloseOutput = false,
                    Indent = true,
                    NewLineChars = Environment.NewLine,
                };

            using( var xmlWriter = XmlWriter.Create(stream, xmlSettings) )
            {
                serializer.WriteObject(xmlWriter, this);
                xmlWriter.Flush();
            }
        }

        public static World LoadFrom(Stream stream)
        {
            var serializer = CreateSerializer();

            var xmlSettings = new XmlReaderSettings
            {
                CloseInput = false,
            };

            using (var xmlReader = XmlReader.Create(stream, xmlSettings))
            {
                var deserializedObject = serializer.ReadObject(xmlReader);
                return (World) deserializedObject;
            }
        }
        #endregion
    }
}
