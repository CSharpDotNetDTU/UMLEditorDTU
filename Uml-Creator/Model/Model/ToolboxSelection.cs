using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uml_Creator.Model
{
    class ToolboxSelection
    {
        public string SelectedList { get; set; }

        public ToolboxSelection(string selectedList)
        {
            SelectedList = selectedList;
        }

        public override string ToString()
        {
            return SelectedList;
        }
    }

    
}
