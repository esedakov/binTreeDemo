using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btree_demo.manager
{
    /// <summary>
    /// Desc: scheduler state types
    /// Author: Eduard Sedakov (ES)
    /// Date: 06-18-2017
    /// </summary>
    enum type__state
    {
        NO_TASKS,
        CURRENT_TASK_IS_FINISHED,
        KEEP_WORKING
    }
}
