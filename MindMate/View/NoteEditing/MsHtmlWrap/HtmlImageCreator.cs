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
    /// <summary>
    /// Inserts Html Image Element at the current location in MsHtml Web Browser
    /// </summary>
    public class HtmlImageCreator
    {
        NoteEditor editor;

        private HtmlDocument Document
        { get { return (HtmlDocument)editor.Document.DomDocument; } }

        public HtmlImageCreator(NoteEditor editor)
        {
            this.editor = editor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src">image source / url</param>
        /// <param name="alt">alternate text for image</param>
        public void InsertImage(string src, string alt)
        {
            try
            {
                using (new UndoUnit(Document, "Insert Image"))
                {
                    // obtain a reference to the body node
                    HtmlDomNode bodyNode = (HtmlDomNode)Document.body;


                    HtmlImageElement img = (HtmlImageElement)Document.createElement("IMG");
                    img.src = src;
                    img.alt = alt;

                    // table processing all complete so insert into the DOM
                    HtmlSelection selection = Document.selection;
                    HtmlTextRange textRange = SelectionHelper.GetTextRange(Document);


                    if (textRange != null)
                    {
                        textRange.pasteHTML(((HtmlElement)img).outerHTML);//"<img src=\"" + src + "\" alt=\"" + alt + "\">");

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
                                controlRange.item(0).outerHTML = ((HtmlElement)img).outerHTML;
                                // this should work with initial count set to zero
                                // controlRange.add((HtmlControlElement)table);
                            }
                            catch (Exception ex)
                            {
                                throw new HtmlEditorException("Cannot Delete all previously Controls selected.", "InsertImage", ex);
                            }
                        }
                        else
                        {
                            // insert the table at the end of the HTML
                            HtmlDomNode imgNode = (HtmlDomNode)img;
                            bodyNode.appendChild(imgNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HtmlEditorException("Unable to insert image.", "InsertImage", ex);
            }

        }
    }
}
