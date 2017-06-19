using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btree_demo.bintree;
using System.Drawing;

namespace btree_demo.drawing
{
    /// <summary>
    /// Desc: graphic engine for depicting binary tree at its different states (insertion, deletion, search)
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-18-2017
    /// </summary>
    class engine
    {
        /// <summary>
        /// tree states that are depicted
        /// </summary>
        tree _tree;
        /// <summary>
        /// size of margin
        /// </summary>
        int _margin;
        /// <summary>
        /// size of node's (circle) radius
        /// </summary>
        int _rad;
        /// <summary>
        /// construct graphic engine for drawing states of binary tree
        /// </summary>
        /// <param name="curTree">current tree instance</param>
        /// <param name="margin">size of margin</param>
        /// <param name="radius">node's radius</param>
        public engine(tree curTree, int margin, int radius)
        {
            //assign fields
            this._tree = curTree;
            this._margin = margin;
            this._rad = radius;
        }   //end engine constructor
        /// <summary>
        /// draw tree during/after deletion process
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public Bitmap draw(deletingState ds)
        {
            //map nodes to their position
            KeyValuePair<Dictionary<node, Point>, Size> treeNodeInfo = this.locateNodes();
            //create resulting bitmap
            Bitmap res = new Bitmap(treeNodeInfo.Value.Width, treeNodeInfo.Value.Height);
            //obtain graphics object for created bitmap
            using (Graphics g = Graphics.FromImage(res))
            {
                //string align in center
                StringFormat centerFormat = new StringFormat();
                centerFormat.LineAlignment = StringAlignment.Center;
                centerFormat.Alignment = StringAlignment.Center;
                //create default font
                Font curFont = createDefFont(res.Width, res.Height);
                //for each node
                foreach (KeyValuePair<node, Point> kvp in treeNodeInfo.Key)
                {
                    //init brush to no color
                    Brush br = null;
                    //if this node is deleted
                    if( ds._del != null && node._keyComparator(ds._del.KEY, kvp.Key.KEY) == 0)
                    {
                        //set color to be red
                        br = Brushes.Red;
                    }
                    //else, if this node is searched
                    else if( ds._searched != null && node._keyComparator(ds._searched.KEY, kvp.Key.KEY) == 0 )
                    {
                        //set color to be green
                        br = Brushes.Green;
                    }
                    //else, if this node is replacement node
                    else if( ds._replacement != null && node._keyComparator(ds._replacement.KEY, kvp.Key.KEY) == 0 )
                    {
                        //set color to be orange
                        br = Brushes.Orange;
                    }
                    //if this node is special
                    if (br != null)
                    {
                        //fill out the circle with indicated color
                        g.FillEllipse(
                            br,
                            kvp.Value.X, kvp.Value.Y,
                            this._rad * 2, this._rad * 2
                        );
                    }   //end if this node is special
                    //draw node
                    this.drawNode(g, kvp.Value, kvp.Key.KEY.ToString(), curFont, centerFormat);
                    //if there is parent
                    if (kvp.Key.PARENT != null)
                    {
                        //draw connection to parent
                        this.drawConnection(g, treeNodeInfo.Key[kvp.Key.PARENT], kvp.Value);
                    }   //end if there is parent
                }   //end loop for each node
            }   //end using - obtain graphics object for created bitmap
            //return resulting bitmap
            return res;
        }   //end function 'draw' for node deletion
        /// <summary>
        /// create default font for this program
        /// </summary>
        /// <param name="w">width of resulting bitmap</param>
        /// <param name="h">height of resulting bitmap</param>
        /// <returns>created default font object</returns>
        protected Font createDefFont(int w, int h)
        {
            //get arial font
            return new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
        }   //end function 'createDefFont'
        /// <summary>
        /// draw node
        /// </summary>
        /// <param name="g">graphics component for drawing on resulting bitmap</param>
        /// <param name="lt">left-top corner of node</param>
        /// <param name="nodeName">node's caption/name</param>
        /// <param name="f">font to use for drawing node's caption/name</param>
        /// <param name="format">string alignment information for drawing node's caption/name</param>
        protected void drawNode(Graphics g, Point lt, String nodeName, Font f, StringFormat format)
        {
            //draw circle
            g.DrawEllipse(
                Pens.Black,
                lt.X, lt.Y,
                this._rad * 2, this._rad * 2
            );
            //draw node's key
            g.DrawString(
                nodeName,
                f,
                Brushes.Black,
                new RectangleF(lt.X * 1.0f, lt.Y * 1.0f, this._rad * 2.0f, this._rad * 2.0f),
                format
            );
        }   //end function 'drawNode'
        /// <summary>
        /// draw tree during/after node insertion process
        /// </summary>
        /// <param name="insertedNodeInfo">node insertion info</param>
        /// <returns>resulting image of binary tree state</returns>
        public Bitmap draw(node insertedNodeInfo)
        {
            //map nodes to their position
            KeyValuePair<Dictionary<node, Point>, Size> treeNodeInfo = this.locateNodes();
            //create resulting bitmap
            Bitmap res = new Bitmap(treeNodeInfo.Value.Width, treeNodeInfo.Value.Height);
            //obtain graphics object for created bitmap
            using (Graphics g = Graphics.FromImage(res))
            {
                //string align in center
                StringFormat centerFormat = new StringFormat();
                centerFormat.LineAlignment = StringAlignment.Center;
                centerFormat.Alignment = StringAlignment.Center;
                //create default font
                Font curFont = createDefFont(res.Width, res.Height);
                //for each node
                foreach(KeyValuePair<node, Point> kvp in treeNodeInfo.Key)
                {
                    //if this node is special
                    if(insertedNodeInfo != null && node._keyComparator(kvp.Key.KEY, insertedNodeInfo.KEY) == 0)
                    {
                        //init brush for newly created node
                        Brush br = Brushes.Red;
                        //if we are searching for right position where to insert a node
                        if( this._tree.DONE_TRACING == false )
                        {
                            //reset brush for traversed node
                            br = Brushes.Green;
                        }   //end if we are searching for right position where to insert a node
                        //fill out the circle with indicated color
                        g.FillEllipse(
                            br,
                            kvp.Value.X, kvp.Value.Y,
                            this._rad * 2, this._rad * 2
                        );
                    }   //end if this node is special
                    //draw node
                    this.drawNode(g, kvp.Value, kvp.Key.KEY.ToString(), curFont, centerFormat);
                    //if there is parent
                    if (kvp.Key.PARENT != null)
                    {
                        //draw connection to parent
                        this.drawConnection(g, treeNodeInfo.Key[kvp.Key.PARENT], kvp.Value);
                    }   //end if there is parent
                }   //end loop for each node
            }   //end using - obtain graphics object for created bitmap
            //return resulting bitmap
            return res;
        }   //end function 'draw' for node insertion
        /// <summary>
        /// draw line connecting two nodes, represented by top-left positions p1 and p2
        /// </summary>
        /// <param name="g">graphics component for resulting bitmap that depicts binary tree</param>
        /// <param name="p1">top-left coordinate of first node's circle</param>
        /// <param name="p2">top-left coordinate of second node's circle</param>
        protected void drawConnection(Graphics g, Point p1, Point p2)
        {
            //recalculate positions p1 and p2 to point to the centers of circles, rather than to top-left corner
            p1.X += this._rad;
            p1.Y += this._rad;
            p2.X += this._rad;
            p2.Y += this._rad;
            //compute vector from p1 to p2
            Point p1_to_p2 = new Point(p2.X - p1.X, p2.Y - p1.Y);
            //compute length of resulting vector
            double len = Math.Sqrt(p1_to_p2.X * p1_to_p2.X + p1_to_p2.Y * p1_to_p2.Y);
            //compute positions p1' and p2' that are shifted towards each other by radius of circle
            Point _p1 = new Point((int)(p1.X + (1.0f * this._rad / len) * p1_to_p2.X), (int)(p1.Y + (1.0f * this._rad / len) * p1_to_p2.Y));
            Point _p2 = new Point((int)(p2.X - (1.0f * this._rad / len) * p1_to_p2.X), (int)(p2.Y - (1.0f * this._rad / len) * p1_to_p2.Y));
            //draw line
            g.DrawLine(Pens.Black, _p1, _p2);
        }   //end function 'drawConnection'
        /// <summary>
        /// find locations for current binary tree nodes
        /// </summary>
        /// <returns>collection that maps node to its location</returns>
        protected KeyValuePair<Dictionary<node, Point>, Size> locateNodes()
        {
            //init resulting collection
            Dictionary<node, Point> res = new Dictionary<node, Point>();
            //if tree is empty
            if( this._tree.ROOT == null )
            {
                //return immediately
                return new KeyValuePair<Dictionary<node, Point>, Size>(res, new Size(this._margin * 2, this._margin * 2));
            }
            //get tree levels
            List<List<node>> levels = this._tree.getLevels();
            //count number leaf nodes
            int numLeafNodes = this._tree.numberOfLeaves();
            //compute width of tree in pixels
            int width = (2 * numLeafNodes - 1) * (2 * this._rad) + 2 * this._margin;
            //comput height of tree in pixels
            int height = (2 * levels.Count - 1) * (2 * this._rad) + 2 * this._margin;
            //calculate position of root node
            Point rootPos = new Point(width / 2 - this._rad, this._margin);
            //add this point to resulting set
            res.Add(this._tree.ROOT, rootPos);
            //loop thru levels of tree starting from second level (root position has been determined)
            for( int level = 1; level < levels.Count; level++)
            {
                //loop thru nodes of current level
                for (int i = 0; i < levels[level].Count; i++)
                {
                    //get current node
                    node cur = levels[level][i];
                    //get position of parent node
                    Point parentPos = res[cur.PARENT];
                    //get next node, if any
                    node next = i + 1 < levels[level].Count ? levels[level][i + 1] : null;
                    //if next node is not sibling
                    if (next == null || node._keyComparator(next.PARENT.KEY, cur.PARENT.KEY) != 0)
                    {
                        //reset node reference
                        next = null;
                        //determine position of single node from its parent position
                        //  x would be the same for both
                        //  y would be lowered by two diameters
                        res.Add(cur, new Point(parentPos.X, parentPos.Y + 4 * this._rad));
                    }
                    //else, next node is sibling
                    else
                    {
                        //increment index by 1
                        i++;
                        //compute width that is allocated to our nodes
                        int allocWidth = width / level;
                        //      *---P---*
                        //      |       |
                        //      C       C
                        //
                        //   T      T      T
                        //+-----*-------*-----+ = 3 * T + 2 * diameter(i.e. 'D') = 3 * T + 4 * R
                        //|     D       D     |
                        //+-------------------+
                        //     allocWidth
                        //compute T = (allocWidth - 4 * R) / 3
                        int t = (allocWidth - 4 * this._rad) / 3;
                        //compute left-top position of first (presumably left) node
                        Point leftNodePos = new Point(parentPos.X - t / 2 - this._rad, parentPos.Y + 4 * this._rad);
                        //compute left-top position of second (right) node
                        Point rightNodePos = new Point(parentPos.X + t / 2 + this._rad, parentPos.Y + 4 * this._rad);
                        //add both nodes to resulting collection
                        res.Add(cur.PARENT.LEFT, leftNodePos);
                        res.Add(cur.PARENT.RIGHT, rightNodePos);
                    }   //end if next node is not sibling
                }   //end loop thru nodes of current level
            }   //end loop thru levels of tree
            //return resulting collection that maps nodes to their position
            return new KeyValuePair<Dictionary<node, Point>, Size>(res, new Size(width, height));
        }   //end function 'locateNodes'
    }
}
