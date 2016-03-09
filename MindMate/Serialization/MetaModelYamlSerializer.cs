using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.MetaModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace MindMate.Serialization
{
    public class MetaModelYamlSerializer
    {
        public const string Icons = "icons";
        public const string Name = "name";
        public const string Title = "title";
        public const string Shortcut = "shortcut";
        public const string RecentFiles = "recentfiles";
        public const string LastOpenedFile = "lastopenedfile";
        public const string MapBackColor = "mapbackcolor";
        public const string NoteBackColor = "notebackcolor";


        public void Serialize(MetaModel.MetaModel metaModel, TextWriter writer)
        {
            Emitter emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());
            emitter.Emit(new MappingStart());

            //icons
            emitter.Emit(new Scalar(Icons));
                emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Block));
                foreach (var modelIcon in metaModel.IconsList)
                {
                    emitter.Emit(new MappingStart());
                    emitter.Emit(new Scalar(Name));
                    emitter.Emit(new Scalar(modelIcon.Name));
                    emitter.Emit(new Scalar(Title));
                    emitter.Emit(new Scalar(modelIcon.Title));
                    emitter.Emit(new Scalar(Shortcut));
                    emitter.Emit(new Scalar(modelIcon.Shortcut));
                    emitter.Emit(new MappingEnd());
                }
                emitter.Emit(new SequenceEnd());
            //recent files
            emitter.Emit(new Scalar(RecentFiles));
                emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Block));
                foreach (var recentFile in metaModel.RecentFiles)
                {
                    emitter.Emit(new Scalar(recentFile));
                }
                emitter.Emit(new SequenceEnd());
            //last opened file
            emitter.Emit(new Scalar(LastOpenedFile));
            emitter.Emit(new Scalar(metaModel.LastOpenedFile));
            //map back color
            emitter.Emit(new Scalar(MapBackColor));
            emitter.Emit(new Scalar(Convert.ToColorHexValue(metaModel.MapEditorBackColor)));
            //note back color
            emitter.Emit(new Scalar(NoteBackColor));
            emitter.Emit(new Scalar(Convert.ToColorHexValue(metaModel.NoteEditorBackColor)));

            emitter.Emit(new MappingEnd());
            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

        }

        /// <summary>
        /// Throws YamlException
        /// </summary>
        /// <param name="metaModel"></param>
        /// <param name="textReader"></param>
        public void Deserialize(MetaModel.MetaModel metaModel, TextReader textReader)
        {
            Parser p = new Parser(textReader);
            EventReader r = new EventReader(p);

            r.Expect<StreamStart>();
            r.Expect<DocumentStart>();
            r.Expect<MappingStart>();

            string section = r.Peek<Scalar>().Value;
            if (section.Equals(Icons))
            {
                r.Expect<Scalar>();
                DeserializeIcons(metaModel, r);
                section = r.Peek<Scalar>().Value;
            }

            if (section != null && section.Equals(RecentFiles))
            {
                r.Expect<Scalar>();
                DeserializeRecentFiles(metaModel, r);
                section = r.Peek<Scalar>().Value;
            }

            if (section != null && section.Equals(LastOpenedFile))
            {
                r.Expect<Scalar>();
                metaModel.LastOpenedFile = r.Expect<Scalar>().Value;
                section = r.Peek<Scalar>().Value;
            }

            if (section != null && section.Equals(MapBackColor))
            {
                r.Expect<Scalar>();
                metaModel.MapEditorBackColor = (Color)(new ColorConverter().ConvertFromString(r.Expect<Scalar>().Value));
                section = r.Peek<Scalar>().Value;
            }

            if (section != null && section.Equals(NoteBackColor))
            {
                r.Expect<Scalar>();
                metaModel.NoteEditorBackColor = (Color)(new ColorConverter().ConvertFromString(r.Expect<Scalar>().Value));
                //section = r.Peek<Scalar>().Value; //uncomment when adding another section
            }
            
            r.Expect<MappingEnd>();
            r.Expect<DocumentEnd>();
            r.Expect<StreamEnd>();
        }

        private void DeserializeIcons(MetaModel.MetaModel metaModel, EventReader r)
        {
            r.Expect<SequenceStart>();

            while (r.Accept<MappingStart>())
            {
                DeserializeIcon(metaModel, r);
            }

            r.Expect<SequenceEnd>();
        }

        /// <summary>
        /// Name and Title are mandatory while shortcut is optional
        /// </summary>
        /// <param name="metaModel"></param>
        /// <param name="r"></param>
        private void DeserializeIcon(MetaModel.MetaModel metaModel, EventReader r)
        {
            string name = null, title = null, shortcut = null;
            r.Expect<MappingStart>();
            if (r.Expect<Scalar>().Value.Equals(Name))
            {
                name = r.Expect<Scalar>().Value;
            }
            if (r.Expect<Scalar>().Value.Equals(Title))
            {
                title = r.Expect<Scalar>().Value;
            }
            if (r.Accept<Scalar>())
            {
                if(r.Expect<Scalar>().Value.Equals(Shortcut))
                {
                    shortcut = r.Expect<Scalar>().Value;
                }
            }
            r.Expect<MappingEnd>();

            metaModel.IconsList.Add(new ModelIcon(name, title, shortcut));
        }

        private void DeserializeRecentFiles(MetaModel.MetaModel metaModel, EventReader r)
        {
            r.Expect<SequenceStart>();

            while (r.Accept<Scalar>())
            {
                metaModel.RecentFiles.Add(r.Expect<Scalar>().Value);
            }

            r.Expect<SequenceEnd>();
        }
    }
}
