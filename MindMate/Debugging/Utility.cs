/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace MindMate.Debugging
{
    
    public static class Utility
    {
        private static readonly Hashtable performanceCounters = new Hashtable();

        static Utility()
        {
            WriteToFile("\n");
            WriteToFile(DateTime.Now.ToString() + "--------------------------");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void WriteToFile(string str)
        {
            //StreamWriter file = new StreamWriter(
            //    System.Windows.Forms.Application.StartupPath + "/SystemLog.txt", true);
            //file.WriteLine(" - " + str);
            //file.Close();

            System.Diagnostics.Debug.WriteLine(str);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void ResetCallCounter()
        {
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void StartTimeCounter(string counterName)
        {
            StartTimeCounter(counterName, "");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void StartTimeCounter(string counterName, string description)
        {
            WriteToFile("Start time counter ..." + counterName + " (" + description + ")");
            performanceCounters.Add(counterName, DateTime.Now.Ticks);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void EndTimeCounter(string counterName)
        {
            WriteToFile("End time counter ..." + counterName + " ... Ticks = " +
                (DateTime.Now.Ticks - (long)performanceCounters[counterName]).ToString());
            performanceCounters.Remove(counterName);
        }
    }
}
