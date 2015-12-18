using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    public class NodeContextMenu : ContextMenuStrip
    {
        public System.Windows.Forms.ToolStripMenuItem mEditNode;
        public System.Windows.Forms.ToolStripMenuItem mInsertChild;
        public System.Windows.Forms.ToolStripMenuItem mDeleteNode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem mSelectIcon;
        public System.Windows.Forms.ToolStripSeparator mSepPluginEnd;

        public NodeContextMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.mEditNode = new System.Windows.Forms.ToolStripMenuItem();
            this.mInsertChild = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mSelectIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.mSepPluginEnd = new System.Windows.Forms.ToolStripSeparator();
            this.mDeleteNode = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // contextMenu
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mEditNode,
            this.mInsertChild,
            this.toolStripSeparator1,
            this.mSelectIcon,
            this.mSepPluginEnd,
            this.mDeleteNode});
            this.Name = "contextMenuStrip1";
            this.Size = new System.Drawing.Size(167, 104);
            // 
            // mEditNode
            // 
            this.mEditNode.Name = "mEditNode";
            this.mEditNode.Size = new System.Drawing.Size(166, 22);
            this.mEditNode.Text = "Edit Node";
            // 
            // mInsertChild
            // 
            this.mInsertChild.Name = "mInsertChild";
            this.mInsertChild.Size = new System.Drawing.Size(166, 22);
            this.mInsertChild.Text = "Insert Child Node";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // mSelectIcon
            // 
            this.mSelectIcon.Name = "mSelectIcon";
            this.mSelectIcon.Size = new System.Drawing.Size(166, 22);
            this.mSelectIcon.Text = "Select Icon ...";
            // 
            // mSepPluginEnd
            // 
            this.mSepPluginEnd.Name = "mSepPluginEnd";
            this.mSepPluginEnd.Size = new System.Drawing.Size(163, 6);
            // 
            // mDeleteNode
            // 
            this.mDeleteNode.Name = "mDeleteNode";
            this.mDeleteNode.Size = new System.Drawing.Size(166, 22);
            this.mDeleteNode.Text = "Delete Node";
        }

    }
}
