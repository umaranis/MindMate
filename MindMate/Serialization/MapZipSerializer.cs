using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using System.Collections;

namespace MindMate.Serialization
{
    public class MapZipSerializer
    {
        private readonly string MAPXMLFILE = "MindMate.xml";

        public void SerializeMap(MapTree tree, IEnumerable<KeyValuePair<string, byte[]>> largeObjects, string fileName, bool overwrite)
        {
            var fileMode = overwrite ? FileMode.Create : FileMode.OpenOrCreate;
            using (var fileStream = new FileStream(fileName, fileMode))
            {
                using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    zip.GetEntry(MAPXMLFILE)?.Delete();

                    if (largeObjects != null)
                    {
                        foreach (var o in largeObjects)
                        {
                            var objectEntry = zip.CreateEntry(o.Key, CompressionLevel.NoCompression);
                            using (var stream = new BinaryWriter(objectEntry.Open()))
                            {
                                stream.Write(o.Value);
                            }
                        }
                    }

                    var xmlEntry = zip.CreateEntry(MAPXMLFILE, CompressionLevel.NoCompression);
                    using (Stream stream = xmlEntry.Open())
                    {
                        new MindMapSerializer().Serialize(stream, tree);
                    }

                }
            }
        }

        public void DeserializeMap(MapTree tree, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    var entry = zip.GetEntry(MAPXMLFILE);
                    using (var reader = new StreamReader(entry.Open()))
                    {
                        var xmlString = reader.ReadToEnd();
                        new MindMapSerializer().Deserialize(xmlString, tree);
                    }
                }
            }
        }

        public byte[] DeserializeLargeObject(string fileName, string objectKey)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    var entry = zip.GetEntry(objectKey);
                    if (entry != null)
                    {
                        using (var reader = new BinaryReader(entry.Open()))
                        {
                            return reader.ReadBytes((int)entry.Length);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
