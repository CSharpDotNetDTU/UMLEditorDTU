using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.Model
{
    public class Diagram
    {
        public List<IFigure> Shapes { get; set; }
        public List<ILine> Lines { get; set; }
    }
}
