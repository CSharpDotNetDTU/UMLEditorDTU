using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.Model
{
    class Method : IMethod
    {
        public string Name { get; set; }
        public EVisibility Visibility { get; set; }
        public string Type { get; set; }
        public List<string> Parameters { get; set; }
    }
}
