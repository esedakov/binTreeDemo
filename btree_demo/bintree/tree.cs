using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btree_demo.bintree
{
    /// <summary>
    /// Desc: binary tree
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-17-2017
    /// </summary>
    class tree
    {
        /// <summary>
        /// collection of all existing nodes in a tree
        /// </summary>
        List<node> _nodes;
        /// <summary>
        /// tree root node (has no parent)
        /// </summary>
        node _root;
        /// <summary>
        /// getter for tree root node
        /// </summary>
        public node ROOT
        {
            get
            {
                return this._root;
            }
        }
        /// <summary>
        /// should we trace tree insertion/deletion processes (do it steps illustrating process), or perform all at once
        /// </summary>
        bool _doTrace;
        /// <summary>
        /// flag that indicates whether tracing operation is done or not; it will be always set TRUE if there is no tracing (doTrace=false)
        /// </summary>
        bool _doneTracingOp;
        /// <summary>
        /// getter/setter for flag that indicates if we are tracing insertion/deletion process, or expecting it to be single operation
        /// </summary>
        public bool DO_TRACE
        {
            get
            {
                return this._doTrace;
            }
            set
            {
                //set tracing flags
                this._doTrace = value;
                this._doneTracingOp = !this._doTrace;
            }
        }
        /// <summary>
        /// getter for flag that indicates if last operation finished
        /// </summary>
        public bool DONE_TRACING
        {
            get
            {
                return this._doneTracingOp;
            }
        }
        /// <summary>
        /// construct binary tree
        /// </summary>
        /// <param name="keyNodeComparator">key node comparator</param>
        /// <param name="doTrace">should we illustrate process of insertion/deletion of a node in a tree (TRUE) or just perform an operation in single step (FALSE)</param>
        public tree(Func<Object, Object, int> keyNodeComparator, bool doTrace = true)
        {
            //create collection of all existing nodes in a tree
            this._nodes = new List<bintree.node>();
            //assign comparator
            node._keyComparator = keyNodeComparator;
            //assign no root -- empty tree
            this._root = null;
            //assign trace flag
            this._doTrace = doTrace;
            //set flag for completeness of tracing operation
            this._doneTracingOp = !this._doTrace;
        }
        /// <summary>
        /// insert new node in binary tree with the given key
        /// </summary>
        /// <param name="parentNode">parent node where to try inserting new key</param>
        /// <param name="key">key for new node</param>
        /// <returns>new node in binary tree</returns>
        public node insert(node parentNode, Object key)
        {
            //set tracing completness operation flag
            this._doneTracingOp = !this._doTrace;
            //init resulting node reference
            node res = null;
            //if creating root node
            if (parentNode == null)
            {
                //create new (root) node and store it inside tree
                this._root = new node(key);
                //add new root node to collection of nodes in a tree
                this._nodes.Add(this._root);
                //set resulting node reference
                res = this._root;
                //set tracing completness operation flag to true (insertion is done)
                this._doneTracingOp = true;
            }
            //else, creating non-root node
            else
            {
                //compare new key with the current node's key to determine which subtree will contain new key
                int compRes = node._keyComparator(key, parentNode.KEY);
                //determine subtree
                node subTree = compRes < 0 ? parentNode.LEFT : parentNode.RIGHT;
                //if chosen subtree does not exist
                if (subTree == null)
                {
                    //store new key in this node
                    res = new node(key, parentNode);
                    //add new node to collection of nodes in a tree
                    this._nodes.Add(res);
                    //if left child
                    if (compRes < 0)
                    {
                        parentNode.LEFT = res;
                    }
                    //else, right child
                    else
                    {
                        parentNode.RIGHT = res;
                    }   //end if left child
                    //set tracing completness operation flag to true (insertion is done)
                    this._doneTracingOp = true;
                }
                //else, if chosen subtree exists
                else if (this._doTrace == false)
                {
                    //go deeper
                    res = insert(subTree, key);
                }
                //else, if need to go deeper AND also we are tracing insertion
                else
                {
                    //return the subtree node that we need to explore next
                    return subTree;
                }   //end if chosen subtree does not exist
            }   //end if creating root node
            //return resulting node
            return res;
        }   //end function 'insert'
        /// <summary>
        /// remove node with the given key from binary tree
        /// </summary>
        /// <param name="parentNode">parent node where try removing child node with specified key</param>
        /// <param name="ds">current deletion state information</param>
        /// <returns>if operation failed then NULL is returned, otherwise, it returns node that was removed</returns>
        public deletingState remove(deletingState ds)
        {
            //set tracing completness operation flag
            this._doneTracingOp = !this._doTrace;
            //init resulting node reference
            node res = null;
            //if deleted node has not been found
            if (ds._del == null)
            {
                //if started searching
                if( ds._searched == null )
                {
                    //set searched node
                    ds._searched = this._root;
                }   //end if started searching
                //if parent node has no specified key
                if (node._keyComparator(ds._key, ds._searched.KEY) != 0)
                {
                    //try to locate node with specified key
                    ds._searched = find(ds._searched, ds._key);
                    //if operation is traced
                    if (this._doTrace)
                    {
                        //flag that indicates if desired node (to be removed) was found
                        bool isFound = node._keyComparator(ds._key, ds._searched.KEY) == 0;
                        //set tracing completness flag to false -- we only found the node, but have not deleted it
                        this._doneTracingOp = false;
                        //return node that we located so far
                        return new deletingState(ds._key, isFound ? ds._searched : null, isFound ? null : ds._searched, null);
                    }   //end if operation is traced
                }   //end if parent node has no specified key
                //save found node
                res = ds._searched;
                //save it in deleting state
                ds._del = res;
                ds._searched = null;
            }
            //else, node to be deleted has been found OR find function returned NULL (i.e. node with specified key does not exist)
            else
            {
                //save in resulting var
                res = ds._del;
            }   //end if deleted node has not been found
            //if node was found
            if( res != null )
            {
                //if node is a leaf
                if( res.isLeaf() )
                {
                    //if not a root node
                    if( res.isRoot() == false)
                    {
                        //remove this node from its parent record
                        res.PARENT.removeChild(res);
                        //remove deleted node from collection of existing nodes in a tree
                        this._nodes.Remove(res);
                        //set trace completness flag
                        this._doneTracingOp = true;
                    }   //end if not a root node
                }
                //else, if node has one child
                else if( res.countChildren() == 1 )
                {
                    //if tracing
                    if (this._doTrace)
                    {
                        //if state has replacement node specified
                        if (ds._replacement != null)
                        {
                            //if deleted node is root
                            if (res.PARENT == null)
                            {
                                //replace root
                                this._root = ds._replacement;
                                //remove parent of replaced root
                                this._root.PARENT = null;
                            }
                            //else, deleted node is not root
                            else
                            {
                                //promote this child to the level of its parent (replace parent, which is deleted node)
                                res.PARENT.replaceChildWithAnotherNode(res, ds._replacement);
                            }
                            //remove deleted node from collection of existing nodes in a tree
                            this._nodes.Remove(res);
                            //set trace completness flag
                            this._doneTracingOp = true;
                            //reset
                            ds._replacement = null;
                        }
                        else
                        {
                            //get the only child of deleting node, which will become its successor
                            ds._replacement = res.RIGHT == null ? res.LEFT : res.RIGHT;
                        }   //end if state has replacement node specified
                    }
                    //else, do deletion in one step
                    else
                    {
                        //get the only child of deleting node, which will become its successor
                        ds._replacement = res.RIGHT == null ? res.LEFT : res.RIGHT;
                        //if deleted node is root
                        if (res.PARENT == null)
                        {
                            //replace root
                            this._root = ds._replacement;
                            //remove parent of replaced root
                            this._root.PARENT = null;
                        }
                        else
                        {
                            //promote this child to the level of its parent (replace parent, which is deleted node)
                            res.PARENT.replaceChildWithAnotherNode(res, ds._replacement);
                        }   //end if deleted node is root
                        //remove deleted node from collection of existing nodes in a tree
                        this._nodes.Remove(res);
                    }   //end if tracing
                }
                //else, node has two children
                else
                {
                    //if tracing
                    if (this._doTrace)
                    {
                        //if inorder successor has been found
                        if (ds._replacement != null)
                        {
                            //replace deleted node information
                            ds._del.replaceNodeInformationWithAnotherNode(ds._replacement);
                            //change deletion state -- need to remove replacement node
                            ds = new deletingState(ds._replacement.KEY, ds._replacement);
                        }
                        //else, need to continue searching for inorder successor
                        else
                        {
                            //if searched node has not been set
                            if (ds._searched == null)
                            {
                                //set it to be right node of current deleted node
                                ds._searched = ds._del.RIGHT;
                            }   //end if searched node has not been set
                            //if currently searched node has left child
                            else if (ds._searched.LEFT != null)
                            {
                                //go one level deeper
                                ds._searched = ds._searched.LEFT;
                            }
                            //else, we found inorder successor
                            else
                            {
                                //set replacement node
                                ds._replacement = ds._searched;
                                //finish searching
                                ds._searched = null;
                            }   //end if currently searched node has left child
                        }   //end if inorder successor has been found
                    }
                    //else, delete in one step
                    else
                    {
                        //set starting searched node
                        ds._searched = ds._del.RIGHT;
                        //loop until searched node has no left child
                        while( ds._searched.LEFT != null)
                        {
                            //go deeper in node hierarchy
                            ds._searched = ds._searched.LEFT;
                        }   //end loop until searched node has no left child
                        //replace deleted node with replacement node
                        ds._del.replaceNodeInformationWithAnotherNode(ds._searched);
                        //delete replacement node
                        this.remove(new deletingState(ds._searched.KEY, ds._searched));
                    }   //end if tracing
                }   //end if node is a leaf
            }   //end if node was found
            //return current deletion state
            return ds;
        }   //end function 'remove'
        /// <summary>
        /// find node by specified key
        /// </summary>
        /// <param name="cur">parent node where to look for key</param>
        /// <param name="key">key to look for inside binary tree</param>
        /// <returns>if operation failed then NULL is returned, otherwise, it returns node that has specified key</returns>
        public node find(node cur, Object key)
        {
            //if item was not found
            if( cur == null )
            {
                //fail
                return null;
            }   //end if item was not found
            //compare searched key with this node's key
            int compRes = node._keyComparator(key, cur.KEY);
            //if item was found
            if( compRes == 0 )
            {
                //return this node
                return cur;
            }
            //return values delivered by recursive call
            return find(
                compRes < 0 ? cur.LEFT : cur.RIGHT,
                key
            );
        }   //end function 'find'
        /// <summary>
        /// count number of nodes that leaves
        /// </summary>
        /// <returns></returns>
        public int numberOfLeaves()
        {
            //count and return number of leaves
            return this._nodes.Count(n => n.isLeaf());
        }   //end function 'numberOfLeaves'
        /// <summary>
        /// get list of nodes on each level
        /// </summary>
        /// <returns>list of nodes on each level from root to leaves</returns>
        public List<List<node>> getLevels()
        {
            //init resulting collection of node levels
            List<List<node>> res = new List<List<node>>();
            //if tree is empty
            if( this._root == null )
            {
                //return empty list
                return res;
            }
            //collection of nodes to expand
            Stack<KeyValuePair<node, int>> expand = new Stack<KeyValuePair<node, int>>();
            //add root
            expand.Push(new KeyValuePair<node, int>(this._root, 0));
            //loop while there are nodes to expand
            while( expand.Count > 0 )
            {
                //get current node
                KeyValuePair<node, int> cur = expand.Pop();
                //if current has children
                if( cur.Key.isLeaf() == false )
                {
                    //if left child exists
                    if( cur.Key.LEFT != null )
                    {
                        //add left to expand collection
                        expand.Push(new KeyValuePair<node, int>(cur.Key.LEFT, cur.Value + 1));
                    }   //end if left child exists
                    //if right child exists
                    if( cur.Key.RIGHT != null )
                    {
                        //add right to expand collection
                        expand.Push(new KeyValuePair<node, int>(cur.Key.RIGHT, cur.Value + 1));
                    }   //end if right child exists
                }   //end if current has children
                //if this node's level is not created in resulting collection
                if( cur.Value >= res.Count)
                {
                    //create new level
                    res.Add(new List<bintree.node>());
                }
                //add current node to resulting collection
                res[cur.Value].Add(cur.Key);
            }   //end loop while there are nodes to expand
            //return levels
            return res;
        }   //end function 'getLevels'
    }
}
