﻿using MindMate.Model;
using System.Drawing;

namespace MindMate.Modules.Undo.Changes
{
    internal class NoteEditorTextColorChange : IChange
    {
        private readonly MapTree tree;
        private readonly Color oldValue;

        public NoteEditorTextColorChange(MapTree tree, Color oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Note Editor Text Color Change";

        public void Undo()
        {
            tree.NoteForeColor = oldValue;
        }
    }
}