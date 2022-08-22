using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Serialization
{
    public class HtmlSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree">PersistentTree instead of MapTree because on PersistentTree we can do LoadAllLargeObjects (MapTree could be lazy loaded and doesn't have the logic to load from disk)</param>
        /// <param name="filePath"></param>
        public void Serialize(MapTree tree, string filePath)
        {
            tree.LoadAllLargeObjects();
            using (StreamWriter file = new StreamWriter(filePath, append: false))
            {
                string filesDir = CreateDirectories(filePath);
                ExportSystemIcons(filesDir);
                ExportImages(tree, filesDir);

                GenerateHeader(file, tree.RootNode.Text, filesDir);
                SerializeRootNode(file, tree.RootNode);
                foreach(var node in tree.RootNode.ChildNodes)
                {
                    SerializeNode(file, node, filesDir);
                }
                GenerateFooter(file);

                CreateCSSFile(filePath);
            }

        }

        private void SerializeRootNode(StreamWriter file, MapNode node)
        {
            file.Write("<header>");            
            file.Write(node.Text);            
            file.Write("</header>");            
        }

        /// <summary>
        /// Serialize node excecpt root node
        /// </summary>
        /// <param name="file"></param>
        /// <param name="node"></param>
        private void SerializeNode(StreamWriter file, MapNode node, string filesDir)
        {           
            file.Write($"<div class='node'>");

            file.Write("<div>");
            SerializeIcons(file, node, filesDir);            
            file.Write(node.Text);
            SerializeLink(file, node, filesDir);
            SerializeImage(file, node, filesDir);
            SerializeNote(file, node, filesDir);
            file.Write("</div>");

            if (node.HasChildren)
            {
                file.Write("<div 'children'>");
                foreach (var n in node.ChildNodes)
                {
                    SerializeNode(file, n, filesDir);
                }
                file.Write("</div>");
            }

            file.Write("</div>");
        }  

        private void SerializeIcons(StreamWriter file, MapNode node, string filesDir)
        {
            foreach(var i in node.Icons)
            {
                var icon = MetaModel.MetaModel.Instance.GetIcon(i);
                file.Write($"<img src='{filesDir + "/system/" + icon.Name + ".png"}' alt='{icon.Title}' width='16' height='16' class='icon'>");
            }
        }

        private void SerializeLink(StreamWriter file, MapNode node, string filesDir)
        {
            if(node.HasLink)
            {
                file.Write("<span class='node-link'>(");
                file.Write($"<img src='{filesDir + "/system/" + node.GetLinkType().ToString() + ".png"}' alt='link' width='16' height='16' class='icon'>");
                var linkText = !node.Text.Equals(node.Link) ? node.Link : "link";
                file.Write("<a href='" + node.Link + "'>" + linkText + "</a>");
                file.Write(")</span>");
            }
        }

        private void SerializeImage(StreamWriter file, MapNode node, string filesDir)
        {
            if (node.HasImage)
            {
                file.Write("<div class='node-image'>");
                file.Write($"<img src='{filesDir + "/" + node.Image}' >");
                file.Write("</div>");
            }
        }

        private void SerializeNote(StreamWriter file, MapNode node, string filesDir)
        {
            if(node.HasNoteText)
            {
                file.Write("<div class='note'>");
                file.Write("<span class='note-icon'>");
                file.Write($"<img src='{filesDir + "/system/note.png"}' alt='note' >");
                file.Write("</span>");
                file.Write(node.NoteText.Replace("mm://", filesDir + "/"));
                file.Write("</div>");
            }
        }

        private void GenerateHeader(StreamWriter file, string title, string filesDir)
        {
            file.Write($@"<!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        
        <title>{title}</title>

        <link rel=""stylesheet"" href='{filesDir}/system/style.css'  type=""text/css"" />        
    </head>
    <body>");
        }

        private void GenerateFooter(StreamWriter file)
        {
            file.Write("</body></html>");
        }

        private void CreateCSSFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            using (var file = new StreamWriter(Path.Combine(dir, fileName + "_files/system/", "style.css"), append: false))
            {
                file.Write(@"
header {
  text-align: center;
  font-size: 3em;
}

.node {
  padding-left: .5em;
  padding-top: 0.2em;
  padding-bottom: 0.2em;
  border-left: 1px dotted;  
  margin: 0.5em;
}

.icon {
  padding-right: 0.2em;
}

.note {
  margin: 0.2em 2em;
  border: 1px solid #ffeb3b;
  padding: 0.5em;
}

.note-icon {
  margin-right: 0.2em;
}

.node-link {
  margin-left: 0.3em;
}

.node-link > .icon {
    padding-left: 0.2em; 
}
");
            }

        }

        private void ExportImages(MapTree tree, string filesDir)
        {
            foreach(var obj in tree.LargeObjectsDictionary)
            {
                using (var stream = File.Create(filesDir + "/" + obj.Key))
                {
                    obj.Value.SaveToStream(stream);
                }
            }
        }

        private void ExportSystemIcons(string filesDir)
        {
            foreach(var icon in MetaModel.MetaModel.Instance.IconsList)
            {
                icon.Bitmap.Save(filesDir + "/system/" + icon.Name + ".png", ImageFormat.Png);
            }

            foreach(var icon in MetaModel.MetaModel.Instance.SystemIconList)
            {
                icon.Bitmap.Save(filesDir + "/system/" + icon.Name + ".png", ImageFormat.Png);
            }

            MindMate.Properties.Resources.sticky_note_pin.Save(filesDir + "/system/note.png", ImageFormat.Png);

            MindMate.Properties.Resources.LinkLocal.Save(filesDir + "/system/MindMapNode.png", ImageFormat.Png);
            MindMate.Properties.Resources.LinkWeb.Save(filesDir + "/system/InternetLink.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkEmail.Save(filesDir + "/system/EmailLink.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkExecutable.Save(filesDir + "/system/Executable.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkImage.Save(filesDir + "/system/ImageFile.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkVideo.Save(filesDir + "/system/VideoFile.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkFolder.Save(filesDir + "/system/Folder.png", ImageFormat.Png); ;
            MindMate.Properties.Resources.LinkFile.Save(filesDir + "/system/File.png", ImageFormat.Png); ;

        }

        private string CreateDirectories(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string filesDir = Path.Combine(dir, fileName + "_files");
            Directory.CreateDirectory(filesDir);
            Directory.CreateDirectory(filesDir + "/system");

            return filesDir;
        }
    }
}
