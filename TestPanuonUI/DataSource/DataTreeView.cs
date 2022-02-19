using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsbPcbaTestSpace
{
    public class DataTreeViewNodeItem
    {
        public string Icon { get; set; }
        public string EditIcon { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }

        public List<DataTreeViewNodeItem> Children { get; set; }
        public DataTreeViewNodeItem()
        {
            Children = new List<DataTreeViewNodeItem>();
        }
       
    }
}
