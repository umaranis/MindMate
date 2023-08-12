﻿using MindMate.Model;
using System.Drawing;

namespace MindMate.Modules.Undo.Changes
{
    internal class NoteEditorBackColorChange : IChange
    {
        private readonly MapTree tree;
        private readonly Color oldValue;

        public NoteEditorBackColorChange(MapTree tree, Color oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Note Editor Back Color Change";

        public void Undo()
        {
            tree.NoteBackColor = oldValue;
        }
    }
}