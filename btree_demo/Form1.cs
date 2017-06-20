using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btree_demo.dlg;
using btree_demo.bintree;
using btree_demo.manager;

namespace btree_demo
{
    /// <summary>
    /// Desc: Binary tree form to depict current interactive state of tree
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-17-2017
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// form ctor
        /// </summary>
        public Form1()
        {
            //init form resources
            InitializeComponent();
            //set tracing flag
            this._isTracing = true;
            //create task scheduler
            this._sch = new scheduler(this.pictureBox1, new tree(comp, this._isTracing));
            //set form's caption
            setFormCaption();
        }

        /// <summary>
        /// set form's caption
        /// </summary>
        private void setFormCaption()
        {
            this.Text = "performing: " + (this._sch.CURRENT != null ? this._sch.CURRENT.TYPE.ToString() : "none") + ", " + (this._isTracing ? "" : "not ") + "tracing";
        }   //end function 'setFormCaption'

        /// <summary>
        /// resize event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            //resize picturebox
            pictureBox1.Width = this.Width - 40;
            pictureBox1.Height = this.Height - 63;
        }

        /// <summary>
        /// keyboard press event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //temporary var for storing key of new/removed node
            String key = "";
            //switch - depending on pressed key do smth
            switch( e.KeyCode)
            {
                //q - quit
                case Keys.Q:
                    Application.Exit();
                    break;

                //r - remove node
                case Keys.R:
                    //ask key to be removed from the tree
                    key = NewNodeDialog.ShowDialog(
                        "Remove Node",
                        "Input key to be removed from binary tree"
                    );
                    //remove node
                    this._sch.removeExistingNode(int.Parse(key), this._isTracing);
                    break;

                //i - insert node
                case Keys.I:
                    //ask key for new node
                    key = NewNodeDialog.ShowDialog(
                        "New Node", 
                        "Input key for new node in binary tree"
                    );
                    //insert new node
                    this._sch.createNewNode(int.Parse(key), this._isTracing);
                    break;

                //[space] - keep performing traced task till it is done
                case Keys.Space:
                    //if tracing
                    if (this._isTracing)
                    {
                        //perform task
                        type__state stateInfo = this._sch.performCurrentTask();
                        //if task is done
                        if (stateInfo != type__state.KEEP_WORKING)
                        {
                            //alert user
                            MessageBox.Show(
                                stateInfo == type__state.CURRENT_TASK_IS_FINISHED ? "current task is finished" : "there are no more tasks to complete"
                            );
                        }   //end if task is done
                    }   //end if tracing
                    break;

                //n - new tree
                case Keys.N:
                    //create new tree
                    this._sch = new scheduler(this.pictureBox1, new tree(comp, this._isTracing));
                    break;

                //s - search node by key
                case Keys.S:
                    //ask user to provide key
                    key = NewNodeDialog.ShowDialog(
                        "Searching node",
                        "Input key for the searching node"
                    );
                    //find a node
                    this._sch.findNode(int.Parse(key), this._isTracing);
                    break;

                //t - switch between tracing/non-tracing modes
                case Keys.T:
                    //change tracing flag
                    this._isTracing = !this._isTracing;
                    this._sch.TREE.DO_TRACE = this._isTracing;
                    break;

                //take picture of tree
                case Keys.P:
                    //save picture
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        //instruct user what to do
                        sfd.Title = "Select file name and path for tree image";
                        //keep asking to select file name and location where to save picture
                        while( sfd.ShowDialog() != DialogResult.OK )
                        {
                            //ask again
                            MessageBox.Show("Please, select file name and location");
                        }   //end loop - keep asking to select file name and location
                        //save picture
                        pictureBox1.Image.Save(sfd.FileName);
                    }   //end using - save picture
                    break;
            }   //end switch - depending on pressed key do smth
            //set form's caption
            this.setFormCaption();
        }   //end keyboard press event handler

        /// <summary>
        /// key comparator function, which is passed in tree
        /// </summary>
        /// <param name="key1">first key</param>
        /// <param name="key2">second key</param>
        /// <returns>-1: key1 less than key2; 0: keys are equal; +1: key1 is greater than key2</returns>
        static int comp(Object key1, Object key2)
        {
            return (int)key1 == (int)key2 ? 0 : ((int)key1 > (int)key2 ? 1 : -1);
        }   //end function for key comparison

        /// <summary>
        /// task scheduler that manages binary tree operations
        /// </summary>
        private scheduler _sch = null;

        /// <summary>
        /// do try to trace binary tree operations or complete them in one step
        /// </summary>
        private bool _isTracing;
    }
}
