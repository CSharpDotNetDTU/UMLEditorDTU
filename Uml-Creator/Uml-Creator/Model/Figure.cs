using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.Model
{
    public class Figure :IFigure
    {
        private static int _figureNr;
        public int FigureNr { get; set; } = _figureNr++;
        public EFigure Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Data { get; set; }
    }
}
