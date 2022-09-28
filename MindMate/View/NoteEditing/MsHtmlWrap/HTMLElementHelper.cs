using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.View.NoteEditing.MsHtmlWrap
{
    public class HTMLElementHelper
    {
        /// <summary>
        /// Converts a collection of IHTMLElements into an array.
        /// This utility is used as a replacement for ArrayList.ToArray() because
        /// that operation throws bogus "object not castable" exceptions on IHTMLElements
        /// in release mode.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static IHTMLElement[] ToElementArray(ICollection collection)
        {
            IHTMLElement[] array = new IHTMLElement[collection.Count];
            int i = 0;
            foreach (IHTMLElement e in collection)
            {
                array[i++] = e;
            }
            return array;
        }

        public static bool ElementsAreEqual(IHTMLElement element1, IHTMLElement element2)
        {
            if (element1 == null || element2 == null)
                return false;
            else
                return element1.sourceIndex == element2.sourceIndex;
        }
    }
}
