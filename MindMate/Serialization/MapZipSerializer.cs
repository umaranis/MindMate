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
using MindMate.Modules.Logging;

namespace MindMate.Serialization
{
    public class MapZipSerializer
    {
        private readonly string MAPXMLFILE = "MindMate.xml";

        public void SerializeMap(PersistentTree tree, string fileName, bool overwrite)
        {
            if (overwrite)
                SerializeMapOverwrite(tree, fileName);
            else
                SerializeMapCreateOrUpdate(tree, fileName);
        }

        /// <summary>
        /// Serialize a new map or overwrite if file already exists
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="fileName"></param>
        private void SerializeMapOverwrite(PersistentTree tree, string fileName)
        {
            var fileMode = FileMode.Create;
            using (var fileStream = new FileStream(fileName, fileMode))
            {
                using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    foreach (var o in tree.LargeObjectsDictionary)
                    {
                        var objectEntry = zip.CreateEntry(o.Key, CompressionLevel.NoCompression);
                        using (Stream stream = objectEntry.Open()) o.Value.SaveToStream(stream);
                    }

                    var xmlEntry = zip.CreateEntry(MAPXMLFILE, CompressionLevel.NoCompression);
                    using (Stream stream = xmlEntry.Open())
                    {
                        new MindMapSerializer().Serialize(stream, tree);
                    }

                }
            }
        }

        /// <summary>
        /// Serialize a new map or update if already exists
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="fileName"></param>
        private void SerializeMapCreateOrUpdate(PersistentTree tree, string fileName)
        {
            var fileMode = FileMode.OpenOrCreate;
            using (var fileStream = new FileStream(fileName, fileMode))
            {
                using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    zip.GetEntry(MAPXMLFILE)?.Delete();

                    foreach (var a in tree.DeletedLargeObjects)
                    {
                        zip.GetEntry(a)?.Delete();
                    }

                    foreach (var o in tree.NewLargeObjects)
                    {
                        var objectEntry = zip.CreateEntry(o.Key, CompressionLevel.NoCompression);
                        using (Stream stream = objectEntry.Open()) o.Value.SaveToStream(stream);
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

        public T DeserializeLargeObject<T>(string fileName, string objectKey) where T : class, ILargeObject, new()
        {
            try
            {
                T obj = new T();
                if(DeserializeLargeObject(fileName, objectKey, obj))
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                Log.Write($"[MapZipSerializer.DeserializeLargeObject] Cannot load large object for file '{fileName}' and key {objectKey} : {exp.Message}");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objectKey"></param>
        /// <param name="outObject">Pass in empty object. The method will fill it up by deserializing. </param>
        /// <returns>false if fails to deserialize</returns>
        public bool DeserializeLargeObject(string fileName, string objectKey, ILargeObject outObject)
        {
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
                    {
                        var entry = zip.GetEntry(objectKey);

                        using (var stream = entry.Open())
                        {
                            outObject.LoadFromStream(stream, (int)entry.Length);
                            return true;
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                Log.Write($"[MapZipSerializer.DeserializeLargeObject] Cannot load large object for file '{fileName}' and key {objectKey} : {exp.Message}");
                return false;
            }
        }

        public List<string> DeserializeAllLargeObjectKeys(string fileName)
        {
            try
            {
                var list = new List<string>();

                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read))
                    {
                        foreach(var entry in zip.Entries)
                        {
                            if (!entry.Name.Equals(MAPXMLFILE))
                            {
                                list.Add(entry.FullName);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
