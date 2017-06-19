using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btree_demo.bintree;
using btree_demo.drawing;

namespace btree_demo.manager
{
    /// <summary>
    /// scheduler that organizes and manages operations on binary tree
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-18-2017
    /// </summary>
    class scheduler
    {
        /// <summary>
        /// picture box on form
        /// </summary>
        PictureBox _formImage;
        /// <summary>
        /// getter for picture box on form
        /// </summary>
        public PictureBox FORM_IMAGE
        {
            get
            {
                return this._formImage;
            }
        }
        /// <summary>
        /// drawing engine for depicting binary tree
        /// </summary>
        engine _draw;
        /// <summary>
        /// getter of drawing engine for depicting binary tree
        /// </summary>
        public engine DRAWING_ENG
        {
            get
            {
                return this._draw;
            }
        }
        /// <summary>
        /// tree instance
        /// </summary>
        tree _tree;
        /// <summary>
        /// get instance of tree
        /// </summary>
        public tree TREE
        {
            get
            {
                return this._tree;
            }
        }
        /// <summary>
        /// stack of operation tasks
        /// </summary>
        Stack<task> _stack;
        /// <summary>
        /// get currently performing task (if any)
        /// </summary>
        public task CURRENT
        {
            get
            {
                return this._stack.Count == 0 ? null : this._stack.Peek();
            }
        }
        /// <summary>
        /// construct task schedule that manages binary tree operations
        /// </summary>
        /// <param name="treeInst">tree instance on which to perform operations</param>
        public scheduler(PictureBox picBox, tree treeInst)
        {
            //assign picture box
            this._formImage = picBox;
            //assign tree
            this._tree = treeInst;
            //create stack
            this._stack = new Stack<task>();
            //create drawing engine
            this._draw = new engine(this._tree, 50, 50);
        }   //end scheduler ctor
        /// <summary>
        /// create task for inserting new node
        /// </summary>
        /// <param name="key">key for new node</param>
        /// <param name="isTraced">is task traced or completed at once</param>
        public void createNewNode(Object key, bool isTraced)
        {
            //create and add task to stack
            this._stack.Push(new task(this._tree, key, type__task.INSERT, isTraced));
            //if tracing
            if( isTraced )
            {
                //draw tree
                this.drawTreeForLastAddedTask();
            }
            //else, not tracing
            else
            {
                //perform task right away
                performCurrentTask();
            }   //end if not tracing
        }   //end function 'createNewNode'
        /// <summary>
        /// create task for removing existing node
        /// </summary>
        /// <param name="key">key associated with node to be removed</param>
        /// <param name="isTraced">is task traced or completed at once</param>
        public void removeExistingNode(Object key, bool isTraced)
        {
            //create and add task to stack
            this._stack.Push(new task(this._tree, key, type__task.DELETE, isTraced));
            //if tracing
            if (isTraced)
            {
                //draw tree
                this.drawTreeForLastAddedTask();
            }
            //else, not tracing
            else
            {
                //perform task right away
                performCurrentTask();
            }   //end if not tracing
        }   //end function 'removeExistingNode'
        /// <summary>
        /// find existing node by specified key
        /// </summary>
        /// <param name="key">key associated with searched node</param>
        /// <param name="isTraced">is task traced or completed at once</param>
        public void findNode(Object key, bool isTraced)
        {
            //create and add task to stack
            this._stack.Push(new task(this._tree, key, type__task.SEARCH));
            //if tracing
            if (isTraced)
            {
                //draw tree
                this.drawTreeForLastAddedTask();
            }
            //else, not tracing
            else
            {
                //perform task right away
                performCurrentTask();
            }   //end if not tracing
        }   //end function 'findNode'
        /// <summary>
        /// perform current task until it is done
        /// </summary>
        /// <returns>state of scheduler</returns>
        public type__state performCurrentTask()
        {
            //if there is no current task (i.e. stack of tasks is empty)
            if( this._stack.Count == 0 )
            {
                //fail
                return type__state.NO_TASKS;
            }   //end if there is no current task
            //get current task
            task cur = this._stack.Peek();
            //init flag for checking if current task is done
            bool isTaskDone = false;
            //if current task has been completed
            if( cur.perform() )
            {
                //remove this task from scheduler stack
                this._stack.Pop();
                //reset flag
                isTaskDone = true;
            }   //end if current task has been completed
            //draw binary tree
            this.drawTree(cur);
            //return state of current task
            return isTaskDone ? type__state.CURRENT_TASK_IS_FINISHED : type__state.KEEP_WORKING;
        }   //end function 'performCurrentTask'
        /// <summary>
        /// draw tree given the task
        /// </summary>
        /// <param name="cur">current task that describes the tree topology and procedure applied to it</param>
        private void drawTree(task cur)
        {
            //depending on type of task
            switch (cur.TYPE)
            {
                //if inserting new node OR searching existing node
                case type__task.INSERT:
                case type__task.SEARCH:
                    this._formImage.Image = this._draw.draw((node)cur.OP_DATA);
                    break;
                //if deleting existing node
                case type__task.DELETE:
                    this._formImage.Image = this._draw.draw((deletingState)cur.OP_DATA);
                    break;
                default:
                    throw new Exception("scheduler : performCurrentTask : unkown task type");
            }   //end switch - depending on type of task
        }   //end function 'drawTree'
        /// <summary>
        /// draw tree using the last added task
        /// </summary>
        private void drawTreeForLastAddedTask()
        {
            //if stack of tasks is not empty
            if( this._stack.Count > 0)
            {
                //reset tracing flag
                this._tree.DONE_TRACING = !this._tree.DO_TRACE;
                //draw tree for the last added task
                this.drawTree(this._stack.Peek());
            }   //end if stack of tasks is not empty
        }   //end function 'drawTreeForLastAddedTask'
    }
}
