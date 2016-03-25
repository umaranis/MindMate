/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MindMate.MetaModel
{
    /// <summary>
    /// ModelIcon is an icon which can be selected by user on MapNode.
    /// </summary>
    //[ProtoBuf.ProtoContract]
    public class ModelIcon : IIcon
    {
        public ModelIcon(string name, string title, string shortcut)
        {
            this.Name = name; 
            this.title = title;
            this.shortcut = shortcut;            
        }

        /// <summary>
        /// Constructor used by xml serializer
        /// </summary>
        private ModelIcon()
        {

        }

        private string name;

        //[ProtoBuf.ProtoMember(1)]
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                this.bitmap = MindMate.Serialization.IconImageLoader.GetIcon(name);
            }
        }
        private string title;

        //[ProtoBuf.ProtoMember(2)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string shortcut;

        //[ProtoBuf.ProtoMember(3)]
        public string Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }

        private Bitmap bitmap;

        public Bitmap Bitmap
        {
            get { return bitmap; }
        }
    }
}
