using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btree_demo.bintree;

namespace btree_demo.manager
{
    /// <summary>
    /// binary tree operation task
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-18-2017
    /// </summary>
    class task
    {
        /// <summary>
        /// next id of task
        /// </summary>
        static int _nextId = 1;
        /// <summary>
        /// unique identifier of this task
        /// </summary>
        int _id;
        /// <summary>
        /// getter of unique identifier
        /// </summary>
        public int ID
        {
            get
            {
                return this._id;
            }
        }
        /// <summary>
        /// tree instance on which to perform an operation
        /// </summary>
        tree _tree;
        /// <summary>
        /// getter of tree instance on which performing this operation task
        /// </summary>
        public tree TREE
        {
            get
            {
                return this._tree;
            }
        }
        /// <summary>
        /// key associated with this operation
        /// </summary>
        Object _key;
        /// <summary>
        /// getter of key that is associated with this operation
        /// </summary>
        public Object KEY
        {
            get
            {
                return this._key;
            }
        }
        /// <summary>
        /// type of operation
        /// </summary>
        type__task _type;
        /// <summary>
        /// getter of operation task
        /// </summary>
        public type__task TYPE
        {
            get
            {
                return this._type;
            }
        }
        /// <summary>
        /// data object that is necessary for performing operation
        /// </summary>
        Object _data;
        /// <summary>
        /// getter for data required for performing tree operation
        /// </summary>
        public Object OP_DATA
        {
            get
            {
                return this._data;
            }
        }
        /// <summary>
        /// is this operation traced (performed in small incremental steps) or completed in one step
        /// </summary>
        bool _isTraced;
        /// <summary>
        /// getter for flag that indicates if this operation is traced OR completed in one step
        /// </summary>
        public bool IS_TRACED
        {
            get
            {
                return this._isTraced;
            }
        }
        /// <summary>
        /// is this task done
        /// </summary>
        bool _isDone;
        /// <summary>
        /// getter for flag that indicates if this task is done (TRUE) or still performing or even not started (FALSE)
        /// </summary>
        public bool IS_DONE
        {
            get
            {
                return this._isDone;
            }
        }
        /// <summary>
        /// construct binary tree operation task
        /// </summary>
        /// <param name="treeInst">tree instance for which to perform an operation</param>
        /// <param name="key">key associated with this tree operation</param>
        /// <param name="type">type of binary tree operation</param>
        /// <param name="isTraced">is this operation traced (split in a series of incremental steps) OR completed in one step</param>
        public task(tree treeInst, Object key, type__task type, bool isTraced = false)
        {
            //get id
            this._id = task._nextId++;
            //mark this task as not done
            this._isDone = false;
            //assign fields
            this._tree = treeInst;
            this._key = key;
            this._type = type;
            this._isTraced = isTraced;
            //if inserting or searching node
            if( this._type == type__task.INSERT || this._type == type__task.SEARCH )
            {
                //get root node which will be our data object
                this._data = this._tree.ROOT;
            }
            //else deleting existing node
            else
            {
                //construct deletion state object which is data
                this._data = new deletingState(this._key);
            }   //end if inserting new node
        }   //end task ctor
        /// <summary>
        /// perform this task (either in multiple steps, if it is traced operation OR in one step, if it is non-traced operation)
        /// </summary>
        /// <returns>is this task completed</returns>
        public bool perform()
        {
            //if this task is not done
            if( !this._isDone )
            {
                //depending on type of operation
                switch(this._type)
                {
                    //if inserting new node
                    case type__task.INSERT:
                        //perform a task
                        this._data = this._tree.insert((node)this._data, this._key);
                        break;
                    //if removing existing node
                    case type__task.DELETE:
                        //perform a task
                        this._data = this._tree.remove((deletingState)this._data);
                        break;
                    //if searching existing node
                    case type__task.SEARCH:
                        //perform a task
                        this._data = this._tree.find((node)this._data, this._key);
                        break;
                }   //end switch - depending on the type of operation
                //update IS_DONE flag
                this._isDone = (this._tree.DO_TRACE && this._tree.DONE_TRACING) || (!this._tree.DO_TRACE);
            }   //end if this task is not done
            //return IS_DONE flag to indiciate whether operation completed or not
            return this._isDone;
        }
    }
}
