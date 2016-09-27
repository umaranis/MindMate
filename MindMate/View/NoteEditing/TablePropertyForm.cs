using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// Form used to enter an Html Table structure
    /// Input based on the HtmlTableProperty struct
    /// </summary>
    public class TablePropertyForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bInsert;
        private System.Windows.Forms.TextBox textTableCaption;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelCaptionAlign;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.GroupBox groupLayout;
        private System.Windows.Forms.GroupBox groupCaption;
        private System.Windows.Forms.Label labelRowColumn;
        private System.Windows.Forms.NumericUpDown numericRows;
        private System.Windows.Forms.NumericUpDown numericColumns;
        private System.Windows.Forms.Label labelPadding;
        private System.Windows.Forms.NumericUpDown numericCellPadding;
        private System.Windows.Forms.Label labelSpacing;
        private System.Windows.Forms.NumericUpDown numericCellSpacing;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.NumericUpDown numericTableWidth;
        private System.Windows.Forms.ComboBox listCaptionAlignment;
        private System.Windows.Forms.ComboBox listCaptionLocation;
        private System.Windows.Forms.GroupBox groupTable;
        private System.Windows.Forms.NumericUpDown numericBorderSize;
        private System.Windows.Forms.RadioButton radioWidthPercent;
        private System.Windows.Forms.Label labelBorderAlign;
        private System.Windows.Forms.Label labelBorderSize;
        private System.Windows.Forms.Panel groupPercentPixel;
        private System.Windows.Forms.ComboBox listTextAlignment;
        private System.Windows.Forms.RadioButton radioWidthPixel;

        // private variable for the table properties
        private HtmlTableProperty tableProperties;

        // constants for the Maximum values
        private const byte MAXIMUM_CELL_ROW = 250;
        private const byte MAXIMUM_CELL_COL = 50;
        private const byte MAXIMUM_CELL_PAD = 25;
        private const byte MAXIMUM_BORDER = 25;
        private const ushort MAXIMUM_WIDTH_PERCENT = 100;
        private const ushort MAXIMUM_WIDTH_PIXEL = 2500;
        private const ushort WIDTH_INC_DIV = 20;


        // default form constructor
        public TablePropertyForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // define the dropdown list value
            this.listCaptionAlignment.Items.AddRange(Enum.GetNames(typeof(HorizontalAlignOption)));
            this.listCaptionLocation.Items.AddRange(Enum.GetNames(typeof(VerticalAlignOption)));
            this.listTextAlignment.Items.AddRange(Enum.GetNames(typeof(HorizontalAlignOption)));

            // ensure default values are listed in the drop down lists
            this.listCaptionAlignment.SelectedIndex = 0;
            this.listCaptionLocation.SelectedIndex = 0;
            this.listTextAlignment.SelectedIndex = 0;

            // define the new maximum values of the dialogs
            this.numericBorderSize.Maximum = MAXIMUM_BORDER;
            this.numericCellPadding.Maximum = MAXIMUM_CELL_PAD;
            this.numericCellSpacing.Maximum = MAXIMUM_CELL_PAD;
            this.numericRows.Maximum = MAXIMUM_CELL_ROW;
            this.numericColumns.Maximum = MAXIMUM_CELL_COL;
            this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PIXEL;

            // define default values based on a new table
            this.TableProperties = new HtmlTableProperty(true);

        } //TablePropertyForm


        public HtmlTableProperty TableProperties
        {
            get
            {
                // define the appropriate table caption properties
                tableProperties.CaptionText = this.textTableCaption.Text;
                tableProperties.CaptionAlignment = (HorizontalAlignOption)this.listCaptionAlignment.SelectedIndex;
                tableProperties.CaptionLocation = (VerticalAlignOption)this.listCaptionLocation.SelectedIndex;
                // define the appropriate table specific properties
                tableProperties.BorderSize = (byte)Math.Min(this.numericBorderSize.Value, this.numericBorderSize.Maximum);
                tableProperties.TableAlignment = (HorizontalAlignOption)this.listTextAlignment.SelectedIndex;
                // define the appropriate table layout properties
                tableProperties.TableRows = (byte)Math.Min(this.numericRows.Value, this.numericRows.Maximum);
                tableProperties.TableColumns = (byte)Math.Min(this.numericColumns.Value, this.numericColumns.Maximum);
                tableProperties.CellPadding = (byte)Math.Min(this.numericCellPadding.Value, this.numericCellPadding.Maximum);
                tableProperties.CellSpacing = (byte)Math.Min(this.numericCellSpacing.Value, this.numericCellSpacing.Maximum);
                tableProperties.TableWidth = (ushort)Math.Min(this.numericTableWidth.Value, this.numericTableWidth.Maximum);
                tableProperties.TableWidthMeasurement = (this.radioWidthPercent.Checked) ? MeasurementOption.Percent : MeasurementOption.Pixel;
                // return the properties
                return tableProperties;
            }
            set
            {
                // persist the new values
                tableProperties = value;
                // define the dialog caption properties
                this.textTableCaption.Text = tableProperties.CaptionText;
                this.listCaptionAlignment.SelectedIndex = (int)tableProperties.CaptionAlignment;
                this.listCaptionLocation.SelectedIndex = (int)tableProperties.CaptionLocation;
                // define the dialog table specific properties
                this.numericBorderSize.Value = Math.Min(tableProperties.BorderSize, MAXIMUM_BORDER);
                this.listTextAlignment.SelectedIndex = (int)tableProperties.TableAlignment;
                // define the dialog table layout properties
                this.numericRows.Value = Math.Min(tableProperties.TableRows, MAXIMUM_CELL_ROW);
                this.numericColumns.Value = Math.Min(tableProperties.TableColumns, MAXIMUM_CELL_COL);
                this.numericCellPadding.Value = Math.Min(tableProperties.CellPadding, MAXIMUM_CELL_PAD);
                this.numericCellSpacing.Value = Math.Min(tableProperties.CellSpacing, MAXIMUM_CELL_PAD);
                this.radioWidthPercent.Checked = (tableProperties.TableWidthMeasurement == MeasurementOption.Percent);
                this.radioWidthPixel.Checked = (tableProperties.TableWidthMeasurement == MeasurementOption.Pixel);
                this.numericTableWidth.Value = Math.Min(tableProperties.TableWidth, this.numericTableWidth.Maximum);
            }

        } //TableProperties

        // define the measurement values based on the selected mesaurment selected
        private void MeasurementOptionChanged(object sender, System.EventArgs e)
        {
            // define a dialog for a percentage change
            if (this.radioWidthPercent.Checked)
            {
                this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PERCENT;
                this.numericTableWidth.Increment = MAXIMUM_WIDTH_PERCENT / WIDTH_INC_DIV;
            }
            // define a dialog for a pixel change
            if (this.radioWidthPixel.Checked)
            {
                this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PIXEL;
                this.numericTableWidth.Increment = MAXIMUM_WIDTH_PIXEL / WIDTH_INC_DIV;
            }

        } //MeasurementOptionChanged

        public bool UpdateTable
        {
            get
            {
                return !bInsert.Text.Equals("Insert");
            }
            set
            {
                bInsert.Text = value ? "Update" : "Insert";
            }
        }


        // Clean up any resources being used.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);

        } //Dispose

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TablePropertyForm));
            this.bCancel = new System.Windows.Forms.Button();
            this.bInsert = new System.Windows.Forms.Button();
            this.groupCaption = new System.Windows.Forms.GroupBox();
            this.listCaptionLocation = new System.Windows.Forms.ComboBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.listCaptionAlignment = new System.Windows.Forms.ComboBox();
            this.labelCaptionAlign = new System.Windows.Forms.Label();
            this.labelCaption = new System.Windows.Forms.Label();
            this.textTableCaption = new System.Windows.Forms.TextBox();
            this.groupLayout = new System.Windows.Forms.GroupBox();
            this.numericCellSpacing = new System.Windows.Forms.NumericUpDown();
            this.labelSpacing = new System.Windows.Forms.Label();
            this.numericCellPadding = new System.Windows.Forms.NumericUpDown();
            this.labelPadding = new System.Windows.Forms.Label();
            this.numericColumns = new System.Windows.Forms.NumericUpDown();
            this.numericRows = new System.Windows.Forms.NumericUpDown();
            this.labelRowColumn = new System.Windows.Forms.Label();
            this.groupPercentPixel = new System.Windows.Forms.Panel();
            this.radioWidthPixel = new System.Windows.Forms.RadioButton();
            this.radioWidthPercent = new System.Windows.Forms.RadioButton();
            this.numericTableWidth = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.groupTable = new System.Windows.Forms.GroupBox();
            this.listTextAlignment = new System.Windows.Forms.ComboBox();
            this.labelBorderAlign = new System.Windows.Forms.Label();
            this.labelBorderSize = new System.Windows.Forms.Label();
            this.numericBorderSize = new System.Windows.Forms.NumericUpDown();
            this.groupCaption.SuspendLayout();
            this.groupLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCellSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCellPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRows)).BeginInit();
            this.groupPercentPixel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTableWidth)).BeginInit();
            this.groupTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBorderSize)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(320, 304);
            this.bCancel.Name = "bCancel";
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Cancel";
            // 
            // bInsert
            // 
            this.bInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bInsert.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bInsert.Location = new System.Drawing.Point(240, 304);
            this.bInsert.Name = "bInsert";
            this.bInsert.TabIndex = 1;
            this.bInsert.Text = "Insert";
            // 
            // groupCaption
            // 
            this.groupCaption.Controls.Add(this.listCaptionLocation);
            this.groupCaption.Controls.Add(this.labelLocation);
            this.groupCaption.Controls.Add(this.listCaptionAlignment);
            this.groupCaption.Controls.Add(this.labelCaptionAlign);
            this.groupCaption.Controls.Add(this.labelCaption);
            this.groupCaption.Controls.Add(this.textTableCaption);
            this.groupCaption.Location = new System.Drawing.Point(8, 8);
            this.groupCaption.Name = "groupCaption";
            this.groupCaption.Size = new System.Drawing.Size(384, 88);
            this.groupCaption.TabIndex = 2;
            this.groupCaption.TabStop = false;
            this.groupCaption.Text = "Caption Properties";
            // 
            // listCaptionLocation
            // 
            this.listCaptionLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listCaptionLocation.Location = new System.Drawing.Point(264, 56);
            this.listCaptionLocation.Name = "listCaptionLocation";
            this.listCaptionLocation.Size = new System.Drawing.Size(104, 21);
            this.listCaptionLocation.TabIndex = 8;
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(200, 56);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(64, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location :";
            // 
            // listCaptionAlignment
            // 
            this.listCaptionAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listCaptionAlignment.Location = new System.Drawing.Point(80, 56);
            this.listCaptionAlignment.Name = "listCaptionAlignment";
            this.listCaptionAlignment.Size = new System.Drawing.Size(104, 21);
            this.listCaptionAlignment.TabIndex = 6;
            // 
            // labelCaptionAlign
            // 
            this.labelCaptionAlign.Location = new System.Drawing.Point(8, 56);
            this.labelCaptionAlign.Name = "labelCaptionAlign";
            this.labelCaptionAlign.Size = new System.Drawing.Size(64, 23);
            this.labelCaptionAlign.TabIndex = 5;
            this.labelCaptionAlign.Text = "Alignment :";
            // 
            // labelCaption
            // 
            this.labelCaption.Location = new System.Drawing.Point(8, 24);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(64, 23);
            this.labelCaption.TabIndex = 1;
            this.labelCaption.Text = "Caption :";
            // 
            // textTableCaption
            // 
            this.textTableCaption.Location = new System.Drawing.Point(80, 24);
            this.textTableCaption.Name = "textTableCaption";
            this.textTableCaption.Size = new System.Drawing.Size(288, 20);
            this.textTableCaption.TabIndex = 0;
            this.textTableCaption.Text = "";
            // 
            // groupLayout
            // 
            this.groupLayout.Controls.Add(this.numericCellSpacing);
            this.groupLayout.Controls.Add(this.labelSpacing);
            this.groupLayout.Controls.Add(this.numericCellPadding);
            this.groupLayout.Controls.Add(this.labelPadding);
            this.groupLayout.Controls.Add(this.numericColumns);
            this.groupLayout.Controls.Add(this.numericRows);
            this.groupLayout.Controls.Add(this.labelRowColumn);
            this.groupLayout.Location = new System.Drawing.Point(8, 200);
            this.groupLayout.Name = "groupLayout";
            this.groupLayout.Size = new System.Drawing.Size(384, 96);
            this.groupLayout.TabIndex = 3;
            this.groupLayout.TabStop = false;
            this.groupLayout.Text = "Cell Properties";
            // 
            // numericCellSpacing
            // 
            this.numericCellSpacing.Location = new System.Drawing.Point(256, 64);
            this.numericCellSpacing.Name = "numericCellSpacing";
            this.numericCellSpacing.Size = new System.Drawing.Size(56, 20);
            this.numericCellSpacing.TabIndex = 6;
            // 
            // labelSpacing
            // 
            this.labelSpacing.Location = new System.Drawing.Point(168, 64);
            this.labelSpacing.Name = "labelSpacing";
            this.labelSpacing.Size = new System.Drawing.Size(80, 23);
            this.labelSpacing.TabIndex = 5;
            this.labelSpacing.Text = "Cell Spacing :";
            // 
            // numericCellPadding
            // 
            this.numericCellPadding.Location = new System.Drawing.Point(96, 64);
            this.numericCellPadding.Name = "numericCellPadding";
            this.numericCellPadding.Size = new System.Drawing.Size(56, 20);
            this.numericCellPadding.TabIndex = 4;
            // 
            // labelPadding
            // 
            this.labelPadding.Location = new System.Drawing.Point(8, 64);
            this.labelPadding.Name = "labelPadding";
            this.labelPadding.Size = new System.Drawing.Size(80, 23);
            this.labelPadding.TabIndex = 3;
            this.labelPadding.Text = "Cell Padding :";
            // 
            // numericColumns
            // 
            this.numericColumns.Location = new System.Drawing.Point(192, 24);
            this.numericColumns.Name = "numericColumns";
            this.numericColumns.Size = new System.Drawing.Size(56, 20);
            this.numericColumns.TabIndex = 2;
            // 
            // numericRows
            // 
            this.numericRows.Location = new System.Drawing.Point(128, 24);
            this.numericRows.Name = "numericRows";
            this.numericRows.Size = new System.Drawing.Size(56, 20);
            this.numericRows.TabIndex = 1;
            // 
            // labelRowColumn
            // 
            this.labelRowColumn.Location = new System.Drawing.Point(8, 24);
            this.labelRowColumn.Name = "labelRowColumn";
            this.labelRowColumn.Size = new System.Drawing.Size(112, 23);
            this.labelRowColumn.TabIndex = 0;
            this.labelRowColumn.Text = "Rows and Columns :";
            // 
            // groupPercentPixel
            // 
            this.groupPercentPixel.Controls.Add(this.radioWidthPixel);
            this.groupPercentPixel.Controls.Add(this.radioWidthPercent);
            this.groupPercentPixel.Location = new System.Drawing.Point(152, 48);
            this.groupPercentPixel.Name = "groupPercentPixel";
            this.groupPercentPixel.Size = new System.Drawing.Size(144, 32);
            this.groupPercentPixel.TabIndex = 9;
            // 
            // radioWidthPixel
            // 
            this.radioWidthPixel.Location = new System.Drawing.Point(80, 8);
            this.radioWidthPixel.Name = "radioWidthPixel";
            this.radioWidthPixel.Size = new System.Drawing.Size(56, 24);
            this.radioWidthPixel.TabIndex = 1;
            this.radioWidthPixel.Text = "Pixels";
            this.radioWidthPixel.CheckedChanged += new System.EventHandler(this.MeasurementOptionChanged);
            // 
            // radioWidthPercent
            // 
            this.radioWidthPercent.Location = new System.Drawing.Point(8, 8);
            this.radioWidthPercent.Name = "radioWidthPercent";
            this.radioWidthPercent.Size = new System.Drawing.Size(64, 24);
            this.radioWidthPercent.TabIndex = 0;
            this.radioWidthPercent.Text = "Percent";
            this.radioWidthPercent.CheckedChanged += new System.EventHandler(this.MeasurementOptionChanged);
            // 
            // numericTableWidth
            // 
            this.numericTableWidth.Location = new System.Drawing.Point(72, 56);
            this.numericTableWidth.Name = "numericTableWidth";
            this.numericTableWidth.Size = new System.Drawing.Size(64, 20);
            this.numericTableWidth.TabIndex = 8;
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(8, 56);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(56, 23);
            this.labelWidth.TabIndex = 7;
            this.labelWidth.Text = "Width :";
            // 
            // groupTable
            // 
            this.groupTable.Controls.Add(this.listTextAlignment);
            this.groupTable.Controls.Add(this.labelBorderAlign);
            this.groupTable.Controls.Add(this.labelBorderSize);
            this.groupTable.Controls.Add(this.numericBorderSize);
            this.groupTable.Controls.Add(this.labelWidth);
            this.groupTable.Controls.Add(this.numericTableWidth);
            this.groupTable.Controls.Add(this.groupPercentPixel);
            this.groupTable.Location = new System.Drawing.Point(8, 104);
            this.groupTable.Name = "groupTable";
            this.groupTable.Size = new System.Drawing.Size(384, 88);
            this.groupTable.TabIndex = 4;
            this.groupTable.TabStop = false;
            this.groupTable.Text = "Table Properties";
            // 
            // listTextAlignment
            // 
            this.listTextAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listTextAlignment.Location = new System.Drawing.Point(256, 24);
            this.listTextAlignment.Name = "listTextAlignment";
            this.listTextAlignment.Size = new System.Drawing.Size(104, 21);
            this.listTextAlignment.TabIndex = 6;
            // 
            // labelBorderAlign
            // 
            this.labelBorderAlign.Location = new System.Drawing.Point(192, 24);
            this.labelBorderAlign.Name = "labelBorderAlign";
            this.labelBorderAlign.Size = new System.Drawing.Size(64, 23);
            this.labelBorderAlign.TabIndex = 5;
            this.labelBorderAlign.Text = "Alignment :";
            // 
            // labelBorderSize
            // 
            this.labelBorderSize.Location = new System.Drawing.Point(8, 24);
            this.labelBorderSize.Name = "labelBorderSize";
            this.labelBorderSize.Size = new System.Drawing.Size(56, 23);
            this.labelBorderSize.TabIndex = 4;
            this.labelBorderSize.Text = "Border :";
            // 
            // numericBorderSize
            // 
            this.numericBorderSize.Location = new System.Drawing.Point(72, 24);
            this.numericBorderSize.Name = "numericBorderSize";
            this.numericBorderSize.Size = new System.Drawing.Size(104, 20);
            this.numericBorderSize.TabIndex = 3;
            // 
            // TablePropertyForm
            // 
            this.AcceptButton = this.bInsert;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(402, 336);
            this.Controls.Add(this.groupTable);
            this.Controls.Add(this.groupLayout);
            this.Controls.Add(this.groupCaption);
            this.Controls.Add(this.bInsert);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            //TODO: this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TablePropertyForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Table Properties";
            this.groupCaption.ResumeLayout(false);
            this.groupLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericCellSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCellPadding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRows)).EndInit();
            this.groupPercentPixel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericTableWidth)).EndInit();
            this.groupTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericBorderSize)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

    }
}
