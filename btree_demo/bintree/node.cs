using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btree_demo.bintree
{
    /// <summary>
    /// binary tree node
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-17-2017
    /// </summary>
    public class node
    {
        /// <summary>
        /// key of this node
        /// </summary>
        Object _key;
        /// <summary>
        /// getter/setter for node's key
        /// </summary>
        public Object KEY
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }
        /// <summary>
        /// left child (if any)
        /// </summary>
        node _left;
        /// <summary>
        /// getter/setter for left child
        /// </summary>
        public node LEFT
        {
            get
            {
                return this._left;
            }
            set
            {
                this._left = value;
            }
        }
        /// <summary>
        /// right child (if any)
        /// </summary>
        node _right;
        /// <summary>
        /// getter/setter for right child
        /// </summary>
        public node RIGHT
        {
            get
            {
                return this._right;
            }
            set
            {
                this._right = value;
            }
        }
        /// <summary>
        /// parent node (if any)
        /// </summary>
        node _parent;
        /// <summary>
        /// getter/setter for parent node
        /// </summary>
        public node PARENT
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }
        /// <summary>
        /// user-defined key comparator
        /// </summary>
        public static Func<Object, Object, int> _keyComparator;
        /// <summary>
        /// construct node for binary tree
        /// </summary>
        /// <param name="key">node's key</param>
        /// <param name="parent">parent node, if any</param>
        /// <param name="left">left child node, if any</param>
        /// <param name="right">right child node, if any</param>
        public node(Object key, node parent = null, node left = null, node right = null)
        {
            //assign data fields
            this._key = key;
            this._parent = parent;
            this._left = left;
            this._right = right;
        }
        /// <summary>
        /// is this node a leaf
        /// </summary>
        /// <returns>TRUE if it is a leaf, otherwise FALSE</returns>
        public bool isLeaf()
        {
            return this._left == null && this._right == null;
        }   //end function 'isLeaf'
        /// <summary>
        /// is this node a root
        /// </summary>
        /// <returns>TRUE if it is a root node, otherwise FALSE</returns>
        public bool isRoot()
        {
            return this._parent == null;
        }   //end function 'isRoot'
        /// <summary>
        /// remove given child node from this parent node
        /// </summary>
        /// <param name="child">node to be removed from this node record</param>
        public void removeChild(node child)
        {
            //if given child is left
            if( this._left._key == child._key )
            {
                //set left child to NULL
                this._left = null;
            }
            //else, if given child is right
            else if( this._right._key == child._key )
            {
                //set right child to NULL
                this._right = null;
            }   //end if given child is left
        }   //end function 'removeChild'
        /// <summary>
        /// count number of children: 0, 1, or 2
        /// </summary>
        /// <returns>number of children (0, 1, or 2)</returns>
        public int countChildren()
        {
            return (this._right == null ? 0 : 1) + (this._left == null ? 0 : 1);
        }   //end function 'countChildren'
        /// <summary>
        /// replace specified child node, with given another node
        /// </summary>
        /// <param name="child">child node to be replaced</param>
        /// <param name="another">another node that replaces child node</param>
        public void replaceChildWithAnotherNode(node child, node another)
        {
            //if given child is left
            if (this._left._key == child._key)
            {
                //replace left
                this._left = another;
            }
            //else, if given child is right
            else if (this._right._key == child._key)
            {
                //replace right
                this._right = another;
            }   //end if given child is left
            //replace parent
            another._parent = this;
        }   //end function 'replaceChildWithAnotherNode'
        /// <summary>
        /// replace this node's information with another node's information
        /// </summary>
        /// <param name="another">another node, which is going to replace this node</param>
        public void replaceNodeInformationWithAnotherNode(node another)
        {
            //assign key
            this._key = another._key;
        }
    }
}
