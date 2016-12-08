using System.Collections.Generic;
using Uml_Creator.Model.Interfaces;

namespace Model.Model
{
    public class Diagram
    {
        public List<IFigure> Shapes { get; set; }
        public List<ILine> Lines { get; set; }
    }
}
