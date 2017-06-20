using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btree_demo.dlg
{
    public static class NewNodeDialog
    {
        /// <summary>
        /// create dialog box that would prompt user to input key for tree node
        /// see: https://stackoverflow.com/a/9569546
        /// Author: Eduard Sedakov (ES)
        /// Date: 06-17-2017
        /// </summary>
        /// <param name="title">title in the dialog header</param>
        /// <param name="caption">dialog caption message</param>
        /// <returns>key for the new node</returns>
        public static String ShowDialog(String title, String caption)
        {
            //create dialog form
            Form dlg = new Form();
            //set dialog form dimensions
            dlg.Width = 500;
            dlg.Height = 200;
            //set dialog header's title
            dlg.Text = title;
            //create label for caption message
            Label cap = new Label();
            //set label location
            cap.Left = 50;
            cap.Top = 50;
            //set label message
            cap.Text = caption;
            //create input box, where user would input key for new tree node
            TextBox keyBox = new TextBox();
            //set location
            keyBox.Left = 50;
            keyBox.Top = 100;
            keyBox.Width = 300;
            //create confirmation button that sends back user output
            Button confBtn = new Button();
            //set location and width
            confBtn.Left = 350;
            confBtn.Top = 100;
            confBtn.Width = 100;
            //set button caption
            confBtn.Text = "Ok";
            //attach event handler to confirmation button to close dialog
            confBtn.Click += (sender, e) => { dlg.Close(); };
            //add controls to dialog form
            dlg.Controls.Add(confBtn);
            dlg.Controls.Add(cap);
            dlg.Controls.Add(keyBox);
            //assign focus to textbox
            //see: https://stackoverflow.com/a/6597309
            dlg.ActiveControl = keyBox;
            //show dialog
            dlg.ShowDialog();
            //return user typed value
            return keyBox.Text;
        }
    }
}
