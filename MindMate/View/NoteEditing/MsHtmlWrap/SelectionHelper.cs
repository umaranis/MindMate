using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    static class SelectionHelper
    {
        // obtains a control range to be worked with
        public static HtmlControlRange GetAllControls(HtmlDocument document)
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

        }

        // get the selected range object
        public static HtmlElement GetFirstControl(HtmlDocument document)
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

        }

        // get the selected range object
        public static HtmlTextRange GetTextRange(HtmlDocument document)
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

        }
    }
}
