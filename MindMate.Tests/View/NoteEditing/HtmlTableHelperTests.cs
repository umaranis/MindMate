using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using MindMate.View.NoteEditing.MsHtmlWrap;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class HtmlTableHelperTests
    {
        [TestMethod()]
        public void HtmlTableHelper()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    editor.HTML = null;
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));                    
                    form.Close();
                };
                
                form.Controls.Add(editor);

                form.ShowDialog();
                
                result = editor.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("</table>"));
        }

        [TestMethod()]
        public void GetSelectedTable_NoSelection()
        {
            bool result = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                var sut = new HtmlTableHelper(editor);
                form.Shown += (sender, args) =>
                {
                    editor.HTML = "Some Text";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();                    
                    sut.TableInsert(new HtmlTableProperty(true));
                    result = sut.GetSelectedTable() == null;
                    form.Close();                    
                };

                form.Controls.Add(editor);

                form.ShowDialog();
                
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }        

        [TestMethod()]
        public void TableInsert()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    editor.HTML = null;
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                result = editor.HTML;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.ToLower().Contains("</table>"));
        }

        [TestMethod()]
        public void TableModify_GivenTable()
        {
            var result = "";
            var tableCount = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = null;
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //find table
                    IHTMLTable table = editor.Document.GetElementsByTagName("table")[0].DomElement as IHTMLTable;
                    //modify table
                    var prop = new HtmlTableProperty(true);
                    prop.CaptionText = "Table modified";
                    sut.TableModify(table, prop);

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                result = editor.HTML;
                tableCount = editor.Document.GetElementsByTagName("table").Count;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.Contains("Table modified"));
            Assert.AreEqual(1, tableCount);
        }

        [TestMethod()]
        public void TableModify_ContainingTable()
        {
            var result = "";
            var tableCount = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();       
                    //modify table             
                    var prop = new HtmlTableProperty(true);
                    prop.CaptionText = "Table modified";
                    sut.TableModify(prop);

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                result = editor.HTML;
                tableCount = editor.Document.GetElementsByTagName("table").Count;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result.Contains("Table modified"));
            Assert.AreEqual(1, tableCount);
        }        

        [TestMethod()]
        public void InsertRowBelow_RowCountIncreased()
        {
            var rowCount = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.InsertRowBelow();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                rowCount = GetTable(editor).rows.length;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(4, rowCount);
        }

        [TestMethod()]
        public void InsertRowBelow_RowIsBelow()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c1");
                    r2.select();
                    //modify table
                    sut.InsertRowBelow();               

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();
                                
                cellValue = GetCellValue(GetTable(editor), 1, 0);
                cellValue2 = GetCellValue(GetTable(editor), 2, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void InsertRowBelow_LastRow()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r2c1");
                    r2.select();
                    //modify table
                    sut.InsertRowBelow();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 3, 0);
                cellValue2 = GetCellValue(GetTable(editor), 2, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r2c0", cellValue2);
        }

        [TestMethod()]
        public void InsertRowAbove_RowCountIncreased()
        {
            var rowCount = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.InsertRowAbove();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                rowCount = GetTable(editor).rows.length;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(4, rowCount);
        }

        [TestMethod()]
        public void InsertRowAbove_InMiddle_RowIsAbove()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r1c1");
                    r2.select();
                    //modify table
                    sut.InsertRowAbove();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 1, 0);
                cellValue2 = GetCellValue(GetTable(editor), 2, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void InsertRowAbove_FirstRow()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c1");
                    r2.select();
                    //modify table
                    sut.InsertRowAbove();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 0);
                cellValue2 = GetCellValue(GetTable(editor), 2, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void InsertColumnRight_ColCountIncreased()
        {
            var rowCount = 0;
            var colCount = 0;
            var tabCols = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.InsertColumnRight();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                rowCount = GetTable(editor).rows.length;
                colCount = (GetTable(editor).rows.item(0) as IHTMLTableRow).cells.length;
                tabCols = GetTable(editor).cols;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(3, rowCount);
            Assert.AreEqual(4, colCount);
            Assert.AreEqual(4, tabCols);
        }

        [TestMethod()]
        public void InsertColumnRight_InMiddle()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c1");
                    r2.select();
                    //modify table
                    sut.InsertColumnRight();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 2);
                cellValue2 = GetCellValue(GetTable(editor), 0, 3);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r0c2", cellValue2);
        }

        [TestMethod()]
        public void InsertColumnRight_RightMost()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c2");
                    r2.select();
                    //modify table
                    sut.InsertColumnRight();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 3);
                cellValue2 = GetCellValue(GetTable(editor), 0, 2);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r0c2", cellValue2);
        }

        [TestMethod()]
        public void InsertColumnLeft_InMiddle()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c2");
                    r2.select();
                    //modify table
                    sut.InsertColumnLeft();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 2);
                cellValue2 = GetCellValue(GetTable(editor), 0, 3);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r0c2", cellValue2);
        }

        [TestMethod()]
        public void InsertColumnLeft_FirstCol()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c0");
                    r2.select();
                    //modify table
                    sut.InsertColumnLeft();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 0);
                cellValue2 = GetCellValue(GetTable(editor), 0, 1);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(string.IsNullOrEmpty(cellValue));
            Assert.AreEqual("r0c0", cellValue2);
        }

        [TestMethod()]
        public void DeleteRow()
        {
            var rowCount = 0;
            var colCount = 0;
            var tabCols = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.DeleteRow();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                rowCount = GetTable(editor).rows.length;
                colCount = (GetTable(editor).rows.item(0) as IHTMLTableRow).cells.length;
                tabCols = GetTable(editor).cols;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(2, rowCount);            
        }

        [TestMethod()]
        public void DeleteColumn()
        {
            var rowCount = 0;
            var colCount = 0;
            var tabCols = 0;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.DeleteColumn();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                rowCount = GetTable(editor).rows.length;
                colCount = (GetTable(editor).rows.item(0) as IHTMLTableRow).cells.length;
                tabCols = GetTable(editor).cols;
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(3, rowCount);
            Assert.AreEqual(2, colCount);
            Assert.AreEqual(2, tabCols);
        }

        [TestMethod()]
        public void DeleteTable()
        {
            var result = "";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    editor.HTML = "Some Text There";
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r = body.createTextRange() as IHTMLTxtRange;
                    r.findText("Text");
                    r.select();
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //move inside table
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("Text");
                    r2.select();
                    //modify table
                    sut.DeleteTable();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                result = editor.HTML;                
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsFalse(result.ToLower().Contains("table"));            
        }

        [TestMethod()]
        public void RowMoveUp()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r1c0");
                    r2.select();
                    //modify table
                    sut.RowMoveUp();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c0", cellValue);
            Assert.AreEqual("r0c0", cellValue2);
        }

        [TestMethod()]
        public void RowMoveUp_FirstRow_NoChange()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c0");
                    r2.select();
                    //modify table
                    sut.RowMoveUp();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r0c0", cellValue);
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void RowMoveDown()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r1c0");
                    r2.select();
                    //modify table
                    sut.RowMoveDown();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 2, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c0", cellValue);
            Assert.AreEqual("r2c0", cellValue2);
        }

        [TestMethod()]
        public void RowMoveDown_LastRow_NoChange()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r2c0");
                    r2.select();
                    //modify table
                    sut.RowMoveDown();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 0, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 0);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r0c0", cellValue);
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void ColumnMoveLeft()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c1");
                    r2.select();
                    //modify table
                    sut.ColumnMoveLeft();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 1, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 1);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c1", cellValue);
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void ColumnMoveLeft_FirstColumn_NoChange()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c0");
                    r2.select();
                    //modify table
                    sut.ColumnMoveLeft();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 1, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 1);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c0", cellValue);
            Assert.AreEqual("r1c1", cellValue2);
        }

        [TestMethod()]
        public void ColumnMoveRight()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r0c0");
                    r2.select();
                    //modify table
                    sut.ColumnMoveRight();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 1, 0);
                cellValue2 = GetCellValue(GetTable(editor), 1, 1);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c1", cellValue);
            Assert.AreEqual("r1c0", cellValue2);
        }

        [TestMethod()]
        public void ColumnMoveRight_LastColumn_NoChange()
        {
            string cellValue = "empty";
            string cellValue2 = "empty";

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r2c2");
                    r2.select();
                    //modify table
                    sut.RowMoveDown();

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();

                cellValue = GetCellValue(GetTable(editor), 1, 2);
                cellValue2 = GetCellValue(GetTable(editor), 1, 1);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("r1c2", cellValue);
            Assert.AreEqual("r1c1", cellValue2);
        }

        private void FillTable(IHTMLTable table)
        {
            foreach(var r in table.rows)
            {
                IHTMLTableRow row = r as IHTMLTableRow;
                foreach(var c in row.cells)
                {
                    IHTMLTableCell cell = c as IHTMLTableCell;
                    IHTMLElement elem = c as IHTMLElement;
                    elem.innerText = "r" + row.rowIndex + "c" + cell.cellIndex;
                }
            }
        }

        /// <summary>
        /// Returns first table
        /// </summary>
        private IHTMLTable GetTable(NoteEditor editor)
        {
            return editor.Document.GetElementsByTagName("table")[0].DomElement as IHTMLTable;
        }

        private string GetCellValue(IHTMLTable table, int row, int col)
        {
            return ((table.rows.item(row) as IHTMLTableRow).cells.item(col) as IHTMLElement).innerText;
        }

        [TestMethod()]
        public void GetTableProperties()
        {
            var tableProperties = new HtmlTableProperty(false);

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Shown += (sender, args) =>
                {
                    //insert table
                    var sut = new HtmlTableHelper(editor);
                    sut.TableInsert(new HtmlTableProperty(true));
                    //fill table
                    FillTable((editor.Document.GetElementsByTagName("table")[0].DomElement) as IHTMLTable);
                    //move inside table
                    var body = editor.Document.Body.DomElement as IHTMLBodyElement;
                    IHTMLTxtRange r2 = body.createTextRange() as IHTMLTxtRange;
                    r2.findText("r2c2");
                    r2.select();
                    //modify table
                    sut.DeleteRow();
                    tableProperties = sut.GetTableProperties(GetTable(editor));

                    form.Close();
                };

                form.Controls.Add(editor);

                form.ShowDialog();                
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(2, tableProperties.TableRows);            
        }

        private Form CreateForm()
        {
            Form form = new Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.ClientSize = new System.Drawing.Size(415, 304);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            form.KeyPreview = true;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Name = "TestForm";
            form.ShowInTaskbar = false;
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            form.Text = "Test Form";
            form.Size = new Size(320, 320);
            return form;
        }
    }
}