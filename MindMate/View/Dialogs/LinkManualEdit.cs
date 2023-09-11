﻿/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System.Windows.Forms;

namespace MindMate.View.Dialogs
{
    public partial class LinkManualEdit : Form
    {
        public LinkManualEdit()
        {
            InitializeComponent();
        }

        public string LinkText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
    }
}
