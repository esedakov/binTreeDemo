using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btree_demo.bintree
{
    /// <summary>
    /// Desc: information used to describe deletion process (it is most often used due to tracing)
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-17-2017
    /// </summary>
    /// </summary>
    public class deletingState
    {
        /// <summary>
        /// node to be deleted
        /// </summary>
        public node _del;
        /// <summary>
        /// key of node that is intended to be deleted
        /// </summary>
        public Object _key;
        /// <summary>
        /// node that is currently searched (i.e. when tracing we often need to
        /// traverse tree, and this is currently traversed node)
        /// </summary>
        public node _searched;
        /// <summary>
        /// replacing node (if any) - it can be one of child nodes OR inorder successor node
        /// </summary>
        public node _replacement;
        /// <summary>
        /// construct deleting state
        /// </summary>
        /// <param name="key">key that points at the deleted node</param>
        /// <param name="deletedNode">node to be deleted</param>
        /// <param name="searchedNode">currently traversed/searched node</param>
        /// <param name="replacementNode">node that intends to replace deleted node</param>
        public deletingState(Object key, node deletedNode = null, node searchedNode = null, node replacementNode = null)
        {
            //assign fields
            this._key = key;
            this._del = deletedNode;
            this._searched = searchedNode;
            this._replacement = replacementNode;
        }
    }
}
