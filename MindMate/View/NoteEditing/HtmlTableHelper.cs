using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using HtmlDocument = mshtml.HTMLDocument;
using HtmlBody = mshtml.HTMLBody;
using HtmlStyleSheet = mshtml.IHTMLStyleSheet;
using HtmlStyle = mshtml.IHTMLStyle;
using HtmlDomNode = mshtml.IHTMLDOMNode;
using HtmlDomTextNode = mshtml.IHTMLDOMTextNode;
using HtmlTextRange = mshtml.IHTMLTxtRange;
using HtmlSelection = mshtml.IHTMLSelectionObject;
using HtmlControlRange = mshtml.IHTMLControlRange;

using HtmlElement = mshtml.IHTMLElement;
using HtmlElementCollection = mshtml.IHTMLElementCollection;
using HtmlControlElement = mshtml.IHTMLControlElement;
using HtmlAnchorElement = mshtml.IHTMLAnchorElement;
using HtmlImageElement = mshtml.IHTMLImgElement;
using HtmlFontElement = mshtml.IHTMLFontElement;
using HtmlLineElement = mshtml.IHTMLHRElement;
using HtmlSpanElement = mshtml.IHTMLSpanFlow;
using HtmlScriptElement = mshtml.IHTMLScriptElement;

using HtmlTable = mshtml.IHTMLTable;
using HtmlTableCaption = mshtml.IHTMLTableCaption;
using HtmlTableRow = mshtml.IHTMLTableRow;
using HtmlTableCell = mshtml.IHTMLTableCell;
using HtmlTableRowMetrics = mshtml.IHTMLTableRowMetrics;
using HtmlTableColumn = mshtml.IHTMLTableCol;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    public class HtmlTableHelper
    {

        HtmlDocument document;

        public HtmlTableHelper(HtmlDocument doc)
        {
            document = doc;
        }

        #region Table Processing Operations

        // public function to create a table class
        // insert method then works on this table
        public void TableInsert(HtmlTableProperty tableProperties)
        {
            // call the private insert table method with a null table entry
            ProcessTable(null, tableProperties);

        } //TableInsert

        // public function to modify a tables properties
        // ensure a table is currently selected or insertion point is within a table
        public bool TableModify(HtmlTableProperty tableProperties)
        {
            // define the Html Table element
            HtmlTable table = GetTableElement();

            // if a table has been selected then process
            if (table != null)
            {
                ProcessTable(table, tableProperties);
                return true;
            }
            else
            {
                return false;
            }

        } //TableModify

        // present to the user the table properties dialog
        // using all the default properties for the table based on an insert operation
        public void TableInsertPrompt()
        {
            // if user has selected a table create a reference
            HtmlTable table = GetFirstControl() as HtmlTable;
            ProcessTablePrompt(table);

        } //TableInsertPrompt


        // present to the user the table properties dialog
        // ensure a table is currently selected or insertion point is within a table
        public bool TableModifyPrompt()
        {
            // define the Html Table element
            HtmlTable table = GetTableElement();

            // if a table has been selected then process
            if (table != null)
            {
                ProcessTablePrompt(table);
                return true;
            }
            else
            {
                return false;
            }

        } //TableModifyPrompt


        // will insert a new row into the table
        // based on the current user row and insertion after
        public void TableInsertRow()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            GetTableElement(out table, out row);

            // process according to table being defined
            if (table != null && row != null)
            {
                try
                {
                    // find the existing row the user is on and perform the insertion
                    int index = row.rowIndex + 1;
                    HtmlTableRow insertedRow = table.insertRow(index) as HtmlTableRow;
                    // add the new columns to the end of each row
                    int numberCols = row.cells.length;
                    for (int idxCol = 0; idxCol < numberCols; idxCol++)
                    {
                        insertedRow.insertCell(-1);
                    }
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Unable to insert a new Row", "TableinsertRow", ex);
                }
            }
            else
            {
                throw new HtmlEditorException("Row not currently selected within the table", "TableInsertRow");
            }

        } //TableInsertRow


        // will delete the currently selected row
        // based on the current user row location
        public void TableDeleteRow()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            GetTableElement(out table, out row);

            // process according to table being defined
            if (table != null && row != null)
            {
                try
                {
                    // find the existing row the user is on and perform the deletion
                    int index = row.rowIndex;
                    table.deleteRow(index);
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Unable to delete the selected Row", "TableDeleteRow", ex);
                }
            }
            else
            {
                throw new HtmlEditorException("Row not currently selected within the table", "TableDeleteRow");
            }

        } //TableDeleteRow


        // present to the user the table properties dialog
        // using all the default properties for the table based on an insert operation
        private void ProcessTablePrompt(HtmlTable table)
        {
            using (TablePropertyForm dialog = new TablePropertyForm())
            {
                // define the base set of table properties
                HtmlTableProperty tableProperties = GetTableProperties(table);

                // set the dialog properties
                dialog.TableProperties = tableProperties;
                //DefineDialogProperties(dialog);
                // based on the user interaction perform the neccessary action
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tableProperties = dialog.TableProperties;
                    if (table == null) TableInsert(tableProperties);
                    else ProcessTable(table, tableProperties);
                }
            }

        } // ProcessTablePrompt


        // function to insert a basic table
        // will honour the existing table if passed in
        private void ProcessTable(HtmlTable table, HtmlTableProperty tableProperties)
        {
            try
            {
                // obtain a reference to the body node and indicate table present
                HtmlDomNode bodyNode = (HtmlDomNode)document.body;
                bool tableCreated = false;

                // ensure a table node has been defined to work with
                if (table == null)
                {
                    // create the table and indicate it was created
                    table = (HtmlTable)document.createElement("TABLE");
                    tableCreated = true;
                }

                // define the table border, width, cell padding and spacing
                table.border = tableProperties.BorderSize;
                if (tableProperties.TableWidth > 0) table.width = (tableProperties.TableWidthMeasurement == MeasurementOption.Pixel) ? string.Format("{0}", tableProperties.TableWidth) : string.Format("{0}%", tableProperties.TableWidth);
                else table.width = string.Empty;
                if (tableProperties.TableAlignment != HorizontalAlignOption.Default) table.align = tableProperties.TableAlignment.ToString().ToLower();
                else table.align = string.Empty;
                table.cellPadding = tableProperties.CellPadding.ToString();
                table.cellSpacing = tableProperties.CellSpacing.ToString();

                // define the given table caption and alignment
                string caption = tableProperties.CaptionText;
                HtmlTableCaption tableCaption = table.caption;
                if (caption != null && caption != string.Empty)
                {
                    // ensure table caption correctly defined
                    if (tableCaption == null) tableCaption = table.createCaption();
                    ((HtmlElement)tableCaption).innerText = caption;
                    if (tableProperties.CaptionAlignment != HorizontalAlignOption.Default) tableCaption.align = tableProperties.CaptionAlignment.ToString().ToLower();
                    if (tableProperties.CaptionLocation != VerticalAlignOption.Default) tableCaption.vAlign = tableProperties.CaptionLocation.ToString().ToLower();
                }
                else
                {
                    // if no caption specified remove the existing one
                    if (tableCaption != null)
                    {
                        // prior to deleting the caption the contents must be cleared
                        ((HtmlElement)tableCaption).innerText = null;
                        table.deleteCaption();
                    }
                }

                // determine the number of rows one has to insert
                int numberRows, numberCols;
                if (tableCreated)
                {
                    numberRows = Math.Max((int)tableProperties.TableRows, 1);
                }
                else
                {
                    numberRows = Math.Max((int)tableProperties.TableRows, 1) - (int)table.rows.length;
                }

                // layout the table structure in terms of rows and columns
                table.cols = (int)tableProperties.TableColumns;
                if (tableCreated)
                {
                    // this section is an optimization based on creating a new table
                    // the section below works but not as efficiently
                    numberCols = Math.Max((int)tableProperties.TableColumns, 1);
                    // insert the appropriate number of rows
                    HtmlTableRow tableRow;
                    for (int idxRow = 0; idxRow < numberRows; idxRow++)
                    {
                        tableRow = table.insertRow(-1) as HtmlTableRow;
                        // add the new columns to the end of each row
                        for (int idxCol = 0; idxCol < numberCols; idxCol++)
                        {
                            tableRow.insertCell(-1);
                        }
                    }
                }
                else
                {
                    // if the number of rows is increasing insert the decrepency
                    if (numberRows > 0)
                    {
                        // insert the appropriate number of rows
                        for (int idxRow = 0; idxRow < numberRows; idxRow++)
                        {
                            table.insertRow(-1);
                        }
                    }
                    else
                    {
                        // remove the extra rows from the table
                        for (int idxRow = numberRows; idxRow < 0; idxRow++)
                        {
                            table.deleteRow(table.rows.length - 1);
                        }
                    }
                    // have the rows constructed
                    // now ensure the columns are correctly defined for each row
                    HtmlElementCollection rows = table.rows;
                    foreach (HtmlTableRow tableRow in rows)
                    {
                        numberCols = Math.Max((int)tableProperties.TableColumns, 1) - (int)tableRow.cells.length;
                        if (numberCols > 0)
                        {
                            // add the new column to the end of each row
                            for (int idxCol = 0; idxCol < numberCols; idxCol++)
                            {
                                tableRow.insertCell(-1);
                            }
                        }
                        else
                        {
                            // reduce the number of cells in the given row
                            // remove the extra rows from the table
                            for (int idxCol = numberCols; idxCol < 0; idxCol++)
                            {
                                tableRow.deleteCell(tableRow.cells.length - 1);
                            }
                        }
                    }
                }

                // if the table was created then it requires insertion into the DOM
                // otherwise property changes are sufficient
                if (tableCreated)
                {
                    // table processing all complete so insert into the DOM
                    HtmlDomNode tableNode = (HtmlDomNode)table;
                    HtmlElement tableElement = (HtmlElement)table;
                    HtmlSelection selection = document.selection;
                    HtmlTextRange textRange = GetTextRange();
                    // final insert dependant on what user has selected
                    if (textRange != null)
                    {
                        // text range selected so overwrite with a table
                        try
                        {
                            string selectedText = textRange.text;
                            if (selectedText != null)
                            {
                                // place selected text into first cell
                                HtmlTableRow tableRow = table.rows.item(0, null) as HtmlTableRow;
                                (tableRow.cells.item(0, null) as HtmlElement).innerText = selectedText;
                            }
                            textRange.pasteHTML(tableElement.outerHTML);
                        }
                        catch (Exception ex)
                        {
                            throw new HtmlEditorException("Invalid Text selection for the Insertion of a Table.", "ProcessTable", ex);
                        }
                    }
                    else
                    {
                        HtmlControlRange controlRange = GetAllControls();
                        if (controlRange != null)
                        {
                            // overwrite any controls the user has selected
                            try
                            {
                                // clear the selection and insert the table
                                // only valid if multiple selection is enabled
                                for (int idx = 1; idx < controlRange.length; idx++)
                                {
                                    controlRange.remove(idx);
                                }
                                controlRange.item(0).outerHTML = tableElement.outerHTML;
                                // this should work with initial count set to zero
                                // controlRange.add((HtmlControlElement)table);
                            }
                            catch (Exception ex)
                            {
                                throw new HtmlEditorException("Cannot Delete all previously Controls selected.", "ProcessTable", ex);
                            }
                        }
                        else
                        {
                            // insert the table at the end of the HTML
                            bodyNode.appendChild(tableNode);
                        }
                    }
                }
                else
                {
                    // table has been correctly defined as being the first selected item
                    // need to remove other selected items
                    HtmlControlRange controlRange = GetAllControls();
                    if (controlRange != null)
                    {
                        // clear the controls selected other than than the first table
                        // only valid if multiple selection is enabled
                        for (int idx = 1; idx < controlRange.length; idx++)
                        {
                            controlRange.remove(idx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // throw an exception indicating table structure change error
                throw new HtmlEditorException("Unable to modify Html Table properties.", "ProcessTable", ex);
            }

        } //ProcessTable


        // determine if the current selection is a table
        // return the table element
        private void GetTableElement(out HtmlTable table, out HtmlTableRow row)
        {
            table = null;
            row = null;
            HtmlTextRange range = GetTextRange();

            try
            {
                // first see if the table element is selected
                table = GetFirstControl() as HtmlTable;
                // if table not selected then parse up the selection tree
                if (table == null && range != null)
                {
                    HtmlElement element = (HtmlElement)range.parentElement();
                    // parse up the tree until the table element is found
                    while (element != null && table == null)
                    {
                        element = (HtmlElement)element.parentElement;
                        // extract the Table properties
                        if (element is HtmlTable)
                        {
                            table = (HtmlTable)element;
                        }
                        // extract the Row  properties
                        if (element is HtmlTableRow)
                        {
                            row = (HtmlTableRow)element;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                table = null;
                row = null;
            }

        } //GetTableElement

        private HtmlTable GetTableElement()
        {
            // define the table and row elements and obtain there values
            HtmlTable table = null;
            HtmlTableRow row = null;
            GetTableElement(out table, out row);

            // return the defined table element
            return table;

        }


        // given an HtmlTable determine the table properties
        private HtmlTableProperty GetTableProperties(HtmlTable table)
        {
            // define a set of base table properties
            HtmlTableProperty tableProperties = new HtmlTableProperty(true);

            // if user has selected a table extract those properties
            if (table != null)
            {
                try
                {
                    // have a table so extract the properties
                    HtmlTableCaption caption = table.caption;
                    // if have a caption persist the values
                    if (caption != null)
                    {
                        tableProperties.CaptionText = ((HtmlElement)table.caption).innerText;
                        if (caption.align != null) tableProperties.CaptionAlignment = (HorizontalAlignOption)TryParseEnum(typeof(HorizontalAlignOption), caption.align, HorizontalAlignOption.Default);
                        if (caption.vAlign != null) tableProperties.CaptionLocation = (VerticalAlignOption)TryParseEnum(typeof(VerticalAlignOption), caption.vAlign, VerticalAlignOption.Default);
                    }
                    // look at the table properties
                    if (table.border != null) tableProperties.BorderSize = TryParseByte(table.border.ToString(), tableProperties.BorderSize);
                    if (table.align != null) tableProperties.TableAlignment = (HorizontalAlignOption)TryParseEnum(typeof(HorizontalAlignOption), table.align, HorizontalAlignOption.Default);
                    // define the table rows and columns
                    int rows = Math.Min(table.rows.length, Byte.MaxValue);
                    int cols = Math.Min(table.cols, Byte.MaxValue);
                    if (cols == 0 && rows > 0)
                    {
                        // cols value not set to get the maxiumn number of cells in the rows
                        foreach (HtmlTableRow tableRow in table.rows)
                        {
                            cols = Math.Max(cols, (int)tableRow.cells.length);
                        }
                    }
                    tableProperties.TableRows = (byte)Math.Min(rows, byte.MaxValue);
                    tableProperties.TableColumns = (byte)Math.Min(cols, byte.MaxValue);
                    // define the remaining table properties
                    if (table.cellPadding != null) tableProperties.CellPadding = TryParseByte(table.cellPadding.ToString(), tableProperties.CellPadding);
                    if (table.cellSpacing != null) tableProperties.CellSpacing = TryParseByte(table.cellSpacing.ToString(), tableProperties.CellSpacing);
                    if (table.width != null)
                    {
                        string tableWidth = table.width.ToString();
                        if (tableWidth.TrimEnd(null).EndsWith("%"))
                        {
                            tableProperties.TableWidth = TryParseUshort(tableWidth.Remove(tableWidth.LastIndexOf("%"), 1), tableProperties.TableWidth);
                            tableProperties.TableWidthMeasurement = MeasurementOption.Percent;
                        }
                        else
                        {
                            tableProperties.TableWidth = TryParseUshort(tableWidth, tableProperties.TableWidth);
                            tableProperties.TableWidthMeasurement = MeasurementOption.Pixel;
                        }
                    }
                    else
                    {
                        tableProperties.TableWidth = 0;
                        tableProperties.TableWidthMeasurement = MeasurementOption.Pixel;
                    }
                }
                catch (Exception ex)
                {
                    // throw an exception indicating table structure change be determined
                    throw new HtmlEditorException("Unable to determine Html Table properties.", "GetTableProperties", ex);
                }
            }

            // return the table properties
            return tableProperties;

        } //GetTableProperties


        // based on the user selection return a table definition
        // if table selected (or insertion point within table) return these values
        public void GetTableDefinition(out HtmlTableProperty table, out bool tableFound)
        {
            // see if a table selected or insertion point inside a table
            HtmlTable htmlTable = GetTableElement();

            // process according to table being defined
            if (htmlTable == null)
            {
                table = new HtmlTableProperty(true);
                tableFound = false;
            }
            else
            {
                table = GetTableProperties(htmlTable);
                tableFound = true;
            }

        } //GetTableDefinition


        // Determine if the insertion point or selection is a table
        private bool IsParentTable()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable htmlTable = GetTableElement();

            // process according to table being defined
            if (htmlTable == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        } //IsParentTable


        #endregion

        // get the selected range object
        private HtmlTextRange GetTextRange()
        {
            // define the selected range object
            HtmlSelection selection;
            HtmlTextRange range = null;

            try
            {
                // calculate the text range based on user selection
                selection = document.selection;
                if (selection.type.Equals("text", StringComparison.OrdinalIgnoreCase) || selection.type.Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    range = selection.createRange() as HtmlTextRange;
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                range = null;
            }

            return range;

        } // GetTextRange

        // get the selected range object
        private HtmlElement GetFirstControl()
        {
            // define the selected range object
            HtmlSelection selection;
            HtmlControlRange range;
            HtmlElement control = null;

            try
            {
                // calculate the first control based on the user selection
                selection = document.selection;
                if (selection.type.Equals("control", StringComparison.OrdinalIgnoreCase))
                {
                    range = selection.createRange() as HtmlControlRange; 
                    if (range.length > 0) control = range.item(0);
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                control = null;
            }

            return control;

        } // GetFirstControl

        // obtains a control range to be worked with
        private HtmlControlRange GetAllControls()
        {
            // define the selected range object
            HtmlSelection selection;
            HtmlControlRange range = null;

            try
            {
                // calculate the first control based on the user selection
                selection = document.selection;
                if (selection.type.Equals("control", StringComparison.OrdinalIgnoreCase))
                {
                    range = selection.createRange() as HtmlControlRange;
                }
            }
            catch (Exception)
            {
                // have unknow error so set return to null
                range = null;
            }

            return range;

        } //GetAllControls

        #region Utility Methods

        private object TryParseEnum(Type enumType, string stringValue, object defaultValue)
        {
            // try the enum parse and return the default 
            object result = defaultValue;
            try
            {
                // try the enum parse operation
                result = Enum.Parse(enumType, stringValue, true);
            }
            catch (Exception)
            {
                // default value will be returned
                result = defaultValue;
            }

            // return the enum value
            return result;

        } //TryParseEnum


        // perform of a string into a byte number
        private byte TryParseByte(string stringValue, byte defaultValue)
        {
            byte result = defaultValue;
            double doubleValue;
            // try the conversion to a double number
            if (Double.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out doubleValue))
            {
                try
                {
                    // try a cast to a byte
                    result = (byte)doubleValue;
                }
                catch (Exception)
                {
                    // default value will be returned
                    result = defaultValue;
                }
            }

            // return the byte value
            return result;

        } //TryParseByte


        // perform of a string into a byte number
        private ushort TryParseUshort(string stringValue, ushort defaultValue)
        {
            ushort result = defaultValue;
            double doubleValue;
            // try the conversion to a double number
            if (Double.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out doubleValue))
            {
                try
                {
                    // try a cast to a byte
                    result = (ushort)doubleValue;
                }
                catch (Exception)
                {
                    // default value will be returned
                    result = defaultValue;
                }
            }

            // return the byte value
            return result;

        } //TryParseUshort

        #endregion Utility Methods

    }
}
