namespace MindMate.View.Search
{
    partial class SearchControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.ckbCase = new System.Windows.Forms.CheckBox();
            this.ckbSelectedNode = new System.Windows.Forms.CheckBox();
            this.ckbExcludeNote = new System.Windows.Forms.CheckBox();
            this.grbIcons = new System.Windows.Forms.GroupBox();
            this.ckbAnyIcon = new System.Windows.Forms.CheckBox();
            this.pnlIcons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddIcon = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.grbIcons.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(15, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(327, 20);
            this.txtSearch.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtSearch, "Enter text to search");
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(267, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // lstResults
            // 
            this.lstResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(15, 170);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(327, 459);
            this.lstResults.TabIndex = 2;
            this.toolTip1.SetToolTip(this.lstResults, "Search Results");
            // 
            // ckbCase
            // 
            this.ckbCase.AutoSize = true;
            this.ckbCase.Location = new System.Drawing.Point(15, 41);
            this.ckbCase.Name = "ckbCase";
            this.ckbCase.Size = new System.Drawing.Size(96, 17);
            this.ckbCase.TabIndex = 3;
            this.ckbCase.Text = "Case Sensitive";
            this.ckbCase.UseVisualStyleBackColor = true;
            // 
            // ckbSelectedNode
            // 
            this.ckbSelectedNode.AutoSize = true;
            this.ckbSelectedNode.Location = new System.Drawing.Point(15, 65);
            this.ckbSelectedNode.Name = "ckbSelectedNode";
            this.ckbSelectedNode.Size = new System.Drawing.Size(192, 17);
            this.ckbSelectedNode.TabIndex = 4;
            this.ckbSelectedNode.Text = "Selected Node\'s Descendents only";
            this.toolTip1.SetToolTip(this.ckbSelectedNode, "Only search within the selected node and it\'s descendents");
            this.ckbSelectedNode.UseVisualStyleBackColor = true;
            // 
            // ckbExcludeNote
            // 
            this.ckbExcludeNote.AutoSize = true;
            this.ckbExcludeNote.Location = new System.Drawing.Point(15, 89);
            this.ckbExcludeNote.Name = "ckbExcludeNote";
            this.ckbExcludeNote.Size = new System.Drawing.Size(90, 17);
            this.ckbExcludeNote.TabIndex = 5;
            this.ckbExcludeNote.Text = "Exclude Note";
            this.toolTip1.SetToolTip(this.ckbExcludeNote, "Exclude Note from search (onty search node\'s text)");
            this.ckbExcludeNote.UseVisualStyleBackColor = true;
            // 
            // grbIcons
            // 
            this.grbIcons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbIcons.Controls.Add(this.ckbAnyIcon);
            this.grbIcons.Controls.Add(this.pnlIcons);
            this.grbIcons.Controls.Add(this.btnAddIcon);
            this.grbIcons.Location = new System.Drawing.Point(15, 113);
            this.grbIcons.Name = "grbIcons";
            this.grbIcons.Size = new System.Drawing.Size(327, 51);
            this.grbIcons.TabIndex = 6;
            this.grbIcons.TabStop = false;
            this.grbIcons.Text = "Icons";
            // 
            // ckbAnyIcon
            // 
            this.ckbAnyIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbAnyIcon.AutoSize = true;
            this.ckbAnyIcon.Location = new System.Drawing.Point(230, 23);
            this.ckbAnyIcon.Name = "ckbAnyIcon";
            this.ckbAnyIcon.Size = new System.Drawing.Size(44, 17);
            this.ckbAnyIcon.TabIndex = 2;
            this.ckbAnyIcon.Text = "Any";
            this.toolTip1.SetToolTip(this.ckbAnyIcon, "If checked, node may have anyone of the selected icons to be included in results." +
        "\r\nIf unchecked, node must have all selected icons.");
            this.ckbAnyIcon.UseVisualStyleBackColor = true;
            // 
            // pnlIcons
            // 
            this.pnlIcons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIcons.Location = new System.Drawing.Point(7, 19);
            this.pnlIcons.Name = "pnlIcons";
            this.pnlIcons.Size = new System.Drawing.Size(217, 23);
            this.pnlIcons.TabIndex = 1;
            // 
            // btnAddIcon
            // 
            this.btnAddIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddIcon.Location = new System.Drawing.Point(277, 19);
            this.btnAddIcon.Name = "btnAddIcon";
            this.btnAddIcon.Size = new System.Drawing.Size(44, 23);
            this.btnAddIcon.TabIndex = 0;
            this.btnAddIcon.Text = "Icons";
            this.toolTip1.SetToolTip(this.btnAddIcon, "Add/Remove icons");
            this.btnAddIcon.UseVisualStyleBackColor = true;
            this.btnAddIcon.Click += new System.EventHandler(this.btnAddIcon_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(267, 65);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnClear, "Clear search results");
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(267, 89);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "Select";
            this.toolTip1.SetToolTip(this.btnSelect, "Select all nodes found in search results");
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grbIcons);
            this.Controls.Add(this.ckbExcludeNote);
            this.Controls.Add(this.ckbSelectedNode);
            this.Controls.Add(this.ckbCase);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.txtSearch);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(355, 634);
            this.grbIcons.ResumeLayout(false);
            this.grbIcons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.ListBox lstResults;
        public System.Windows.Forms.CheckBox ckbCase;
        public System.Windows.Forms.CheckBox ckbSelectedNode;
        public System.Windows.Forms.CheckBox ckbExcludeNote;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button btnAddIcon;
        public System.Windows.Forms.FlowLayoutPanel pnlIcons;
        private System.Windows.Forms.GroupBox grbIcons;
        public System.Windows.Forms.CheckBox ckbAnyIcon;
        public System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.Button btnSelect;
    }
}
