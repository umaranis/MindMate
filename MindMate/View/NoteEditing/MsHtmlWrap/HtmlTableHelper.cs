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
using MindMate.Modules.Logging;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    public class HtmlTableHelper
    {
        NoteEditor editor;

        private HtmlDocument Document
        { get { return (HtmlDocument)editor.Document.DomDocument; } }

        public HtmlTableHelper(NoteEditor editor)
        {
            this.editor = editor;
        }

        #region Table Processing Operations

        /// <summary>
        /// Returns null if table is not selected.
        /// </summary>
        /// <returns>null if table is not selected</returns>
        public HtmlTable GetSelectedTable()
        {
            return SelectionHelper.GetFirstControl(Document) as HtmlTable;
        }

        // public function to create a table class
        // insert method then works on this table
        public void TableInsert(HtmlTableProperty tableProperties)
        {
            // call the private insert table method with a null table entry
            ProcessTable(null, tableProperties);
            editor._FireCursorMovedEvent();

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

        public void TableModify(HtmlTable table, HtmlTableProperty tableProperties)
        {
            ProcessTable(table, tableProperties);
        }

        public void InsertRowBelow()
        {
            InsertRow(true);
        }

        public void InsertRowAbove()
        {
            InsertRow(false);
        }
                
        // will insert a new row into the table
        // based on the current user row and insertion after
        private void InsertRow(bool below = true)
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            // process according to table being defined
            if (table != null && row != null)
            {
                try
                {
                    using (new SelectionPreserver(GetMarkupRange()))
                    {
                        using (new UndoUnit(Document, "Table row insert"))
                        {
                            // find the existing row the user is on and perform the insertion
                            int index = row.rowIndex + (below ? 1 : 0);
                            HtmlTableRow insertedRow = table.insertRow(index) as HtmlTableRow;
                            // add the new columns to the end of each row
                            int numberCols = row.cells.length;
                            for (int idxCol = 0; idxCol < numberCols; idxCol++)
                            {
                                insertedRow.insertCell(-1);
                            }
                        }
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

        }

        public void InsertColumnRight()
        {
            InsertColumn(true);
        }

        public void InsertColumnLeft()
        {
            InsertColumn(false);
        }

        private void InsertColumn(bool right = true)
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            // process according to table being defined
            if (table != null && row != null && cell != null)
            {
                try
                {
                    using (new UndoUnit(Document, "Table column insert"))
                    {
                        // find the existing row the user is on and perform the insertion
                        int index = cell.cellIndex + (right ? 1 : 0);
                        // add the new columns to the end of each row
                        int numberRows = table.rows.length;
                        for (int i = 0; i < numberRows; i++)
                        {
                            HtmlTableRow r = table.rows.item(i) as HtmlTableRow;
                            r.insertCell(index);
                        }

                        table.cols++;
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

        }

        // will delete the currently selected row
        // based on the current user row location
        public void DeleteRow()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            // process according to table being defined
            if (table != null && row != null)
            {
                try
                {
                    using (new UndoUnit(Document, "Table row delete"))
                    {
                        // find the existing row the user is on and perform the deletion
                        int index = row.rowIndex;
                        table.deleteRow(index); 
                    }

                    editor._FireCursorMovedEvent();
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

        }

        public void DeleteColumn()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            // process according to table being defined
            if (table != null && row != null && cell != null)
            {
                try
                {
                    using (new UndoUnit(Document, "Table column delete"))
                    {
                        foreach (HtmlTableRow r in table.rows)
                        {
                            r.deleteCell(cell.cellIndex);
                        }
                        table.cols--;    
                    }
                    editor._FireCursorMovedEvent();    
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

        }

        public void DeleteTable()
        {
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            if (table != null)
            {
                try
                {
                    using (new UndoUnit(Document, "Table delete"))
                    {
                        (table as HtmlDomNode).removeNode(true); 
                    }

                    editor._FireCursorMovedEvent();
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
        }

        public void RowMoveUp()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            if (table != null && row != null && cell != null)
            {
                if (row.rowIndex == 0) return;
                try
                {
                    using (new SelectionPreserver(GetMarkupRange()))
                    {
                        using (new UndoUnit(Document, "Table row move up"))
                        {
                            HtmlTableRow rowAbove = table.rows.item(row.rowIndex - 1);
                            (row as HtmlDomNode).swapNode(rowAbove as HtmlDomNode);
                        }
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

        }

        public void RowMoveDown()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            if (table != null && row != null && cell != null)
            {
                if (row.rowIndex >= table.rows.length - 1) return;
                try
                {
                    using (new SelectionPreserver(GetMarkupRange()))
                    {
                        using (new UndoUnit(Document, "Table row move down"))
                        {
                            HtmlTableRow rowBelow = table.rows.item(row.rowIndex + 1);
                            (row as HtmlDomNode).swapNode(rowBelow as HtmlDomNode); 
                        }
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

        }

        public void ColumnMoveLeft()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            if (table != null && row != null && cell != null)
            {
                int index = cell.cellIndex;
                if (index == 0) return;
                try
                {
                    using (new SelectionPreserver(GetMarkupRange()))
                    {
                        using (new UndoUnit(Document, "Table column move left"))
                        {
                            for (int i = 0; i < table.rows.length; i++)
                            {
                                HtmlTableRow r = table.rows.item(i);
                                HtmlDomNode c1 = r.cells.item(index);
                                HtmlDomNode c2 = r.cells.item(index - 1);
                                c1.swapNode(c2);
                            } 
                        }
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

        }

        public void ColumnMoveRight()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable table = null;
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            GetTableElement(out table, out row, out cell);

            if (table != null && row != null && cell != null)
            {
                int index = cell.cellIndex;                
                if (index >= row.cells.length - 1) return;
                try
                {
                    using (new SelectionPreserver(GetMarkupRange()))
                    {
                        using (new UndoUnit(Document, "Table column move right"))
                        {
                            for (int i = 0; i < table.rows.length; i++)
                            {
                                HtmlTableRow r = table.rows.item(i);
                                HtmlDomNode c1 = r.cells.item(index);
                                HtmlDomNode c2 = r.cells.item(index + 1);
                                c1.swapNode(c2);
                            } 
                        }
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

        }

        // function to insert a basic table
        // will honour the existing table if passed in
        private void ProcessTable(HtmlTable table, HtmlTableProperty tableProperties)
        {
            try
            {
                using (new UndoUnit(Document, "Table add/modify"))
                {
                    // obtain a reference to the body node and indicate table present
                    HtmlDomNode bodyNode = (HtmlDomNode)Document.body;
                    bool tableCreated = false;

                    MsHtmlWrap.MarkupRange targetMarkupRange = null;

                    // ensure a table node has been defined to work with
                    if (table == null)
                    {
                        // create the table and indicate it was created
                        table = (HtmlTable)Document.createElement("TABLE");
                        tableCreated = true;

                        //markup range for selecting first cell after table creation
                        targetMarkupRange = GetMarkupRange();
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
                        HtmlSelection selection = Document.selection;
                        HtmlTextRange textRange = SelectionHelper.GetTextRange(Document);
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
                            HtmlControlRange controlRange = SelectionHelper.GetAllControls(Document);
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
                        HtmlControlRange controlRange = SelectionHelper.GetAllControls(Document);
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

                    //if table created, then focus the first cell
                    if (tableCreated)
                    {
                        try
                        {
                            HtmlElement cell = targetMarkupRange.GetFirstElement(e => e is HtmlTableCell, true);
                            if (cell != null)
                            {
                                SelectCell(cell as HtmlTableCell);
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Write(e);
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

        private MarkupRange GetMarkupRange()
        {
            MshtmlMarkupServices markupServices = new MshtmlMarkupServices((IMarkupServicesRaw)Document);
            MarkupRange range = markupServices.CreateMarkupRange(Document.selection.createRange());

            range.Start.Gravity = mshtml._POINTER_GRAVITY.POINTER_GRAVITY_Left;
            range.End.Gravity = mshtml._POINTER_GRAVITY.POINTER_GRAVITY_Right;
            range.Start.Cling = false;
            range.End.Cling = false;

            return range;
        }


        // determine if the current selection is a table
        // return the table element
        private void GetTableElement(out HtmlTable table, out HtmlTableRow row, out HtmlTableCell cell)
        {
            table = null;
            row = null;
            cell = null;
            HtmlTextRange range = SelectionHelper.GetTextRange(Document);

            try
            {
                // first see if the table element is selected
                table = SelectionHelper.GetFirstControl(Document) as HtmlTable;
                // if table not selected then parse up the selection tree
                if (table == null && range != null)
                {
                    HtmlElement element = (HtmlElement)range.parentElement();
                    // parse up the tree until the table element is found
                    while (element != null && table == null)
                    {
                        if (element is HtmlTable)
                        {
                            table = (HtmlTable)element;
                        }                        
                        else if (element is HtmlTableRow)
                        {
                            row = (HtmlTableRow)element;
                        }
                        else if(element is HtmlTableCell)
                        {
                            cell = (HtmlTableCell)element;
                        }
                        element = (HtmlElement)element.parentElement;
                    }
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                table = null;
                row = null;
                cell = null;
            }

        } //GetTableElement

        /// <summary>
        /// Get selected or parent table
        /// </summary>
        /// <returns>null if not found</returns>
        public HtmlTable GetTableElement()
        {
            // define the table and row elements and obtain there values
            HtmlTable table = null;
            HtmlTextRange range = SelectionHelper.GetTextRange(Document);

            
            // first see if the table element is selected
            table = SelectionHelper.GetFirstControl(Document) as HtmlTable;
            // if table not selected then parse up the selection tree
            if (table == null && range != null)
            {
                HtmlElement element = (HtmlElement)range.parentElement();
                // parse up the tree until the table element is found
                while (element != null && table == null)
                {
                    if (element is HtmlTable)
                    {
                        table = (HtmlTable)element;
                    }
                    element = (HtmlElement)element.parentElement;
                }
            }

            // return the defined table element                
            return table;
        }


        // given an HtmlTable determine the table properties
        public HtmlTableProperty GetTableProperties(HtmlTable table)
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

        // Determine if the insertion point or selection is a table
        public bool InsideTable()
        {
            // see if a table selected or insertion point inside a table
            HtmlTable htmlTable = GetTableElement();

            // process according to table being defined
            return htmlTable != null;
        } 


        #endregion

        private void SelectCell(HtmlTableCell cell)
        {
            HtmlElement cellElement = cell as HtmlElement;

            MsHtmlWrap.MshtmlMarkupServices markupServices = new MsHtmlWrap.MshtmlMarkupServices((MsHtmlWrap.IMarkupServicesRaw)Document);
            
            if(cellElement.document == Document)
                System.Diagnostics.Debug.WriteLine("same");
            // move the selection to the beginning of the cell
            MsHtmlWrap.MarkupRange markupRange = markupServices.CreateMarkupRange(cellElement);

            //  if the cell is empty then collapse the selection
            if (cellElement.innerHTML == null)
                markupRange.End.MoveToPointer(markupRange.Start);

            HtmlTextRange textRange = markupRange.ToTextRange();
            textRange.select();
        }

        private static HtmlTableCell GetCell(HtmlTable table, int row, int col)
        {
            return table.rows.item(row).cells.item(col) as HtmlTableCell;
        }       

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
