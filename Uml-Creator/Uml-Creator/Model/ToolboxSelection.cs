using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uml_Creator.Model
{
    class ToolboxSelection
    {
        public string _selectedList { get; set; }

        public ToolboxSelection(string selectedList)
        {
            _selectedList = selectedList;
        }

        public override string ToString()
        {
            return _selectedList;
        }
    }
}
