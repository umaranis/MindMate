using System.Drawing;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    public class NodeContextMenu : ContextMenuStrip
    {
        public ToolStripMenuItem mEditNode;
        public ToolStripMenuItem mInsertChild;
        public ToolStripMenuItem mDeleteNode;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem mSelectIcon;
        public ToolStripSeparator mSepPluginEnd;

        public NodeContextMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.mEditNode = new ToolStripMenuItem();
            this.mInsertChild = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mSelectIcon = new ToolStripMenuItem();
            this.mSepPluginEnd = new ToolStripSeparator();
            this.mDeleteNode = new ToolStripMenuItem();

            // 
            // contextMenu
            // 
            this.Items.AddRange(new ToolStripItem[] {
            this.mEditNode,
            this.mInsertChild,
            this.toolStripSeparator1,
            this.mSelectIcon,
            this.mSepPluginEnd,
            this.mDeleteNode});
            this.Name = "contextMenuStrip1";
            this.Size = new Size(167, 104);
            // 
            // mEditNode
            // 
            this.mEditNode.Name = "mEditNode";
            this.mEditNode.Size = new Size(166, 22);
            this.mEditNode.Text = "Edit Text";
            // 
            // mInsertChild
            // 
            this.mInsertChild.Name = "mInsertChild";
            this.mInsertChild.Size = new Size(166, 22);
            this.mInsertChild.Text = "Insert Child Node";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(163, 6);
            // 
            // mSelectIcon
            // 
            this.mSelectIcon.Name = "mSelectIcon";
            this.mSelectIcon.Size = new Size(166, 22);
            this.mSelectIcon.Text = "Select Icon ...";
            // 
            // mSepPluginEnd
            // 
            this.mSepPluginEnd.Name = "mSepPluginEnd";
            this.mSepPluginEnd.Size = new Size(163, 6);
            // 
            // mDeleteNode
            // 
            this.mDeleteNode.Name = "mDeleteNode";
            this.mDeleteNode.Size = new Size(166, 22);
            this.mDeleteNode.Text = "Delete Node";
        }

    }
}
