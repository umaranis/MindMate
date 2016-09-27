using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    //Exception class for HtmlEditor
    public class HtmlEditorException : ApplicationException
    {
        private string _operationName;

        // property for the operation name
        public string Operation
        {
            get
            {
                return _operationName;
            }
            set
            {
                _operationName = value;
            }

        } //OperationName


        // Default constructor
        public HtmlEditorException() : base()
        {
            _operationName = string.Empty;
        }

        // Constructor accepting a single string message
        public HtmlEditorException(string message) : base(message)
        {
            _operationName = string.Empty;
        }

        // Constructor accepting a string message and an inner exception
        public HtmlEditorException(string message, Exception inner) : base(message, inner)
        {
            _operationName = string.Empty;
        }

        // Constructor accepting a single string message and an operation name
        public HtmlEditorException(string message, string operation) : base(message)
        {
            _operationName = operation;
        }

        // Constructor accepting a string message an operation and an inner exception
        public HtmlEditorException(string message, string operation, Exception inner) : base(message, inner)
        {
            _operationName = operation;
        }

    } //HtmlEditorException
}
