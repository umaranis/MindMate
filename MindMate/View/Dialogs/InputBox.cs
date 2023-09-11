﻿/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System.Windows.Forms;

namespace MindMate.View.Dialogs
{
    public partial class InputBox : Form
    {
        public InputBox(string question, string caption = null)
        {
            InitializeComponent();
            this.lblQuestion.Text = question;
            if(caption != null) Text = caption;
        }

        public string Answer
        {
            get => txtAnswer.Text;
            set => txtAnswer.Text = value;
        }

    }
}
