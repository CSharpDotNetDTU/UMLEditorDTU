using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;

namespace Uml_Creator.Model.Interfaces
{
    public interface IFigure
    {
        EFigure Type { get; }
        double Height { get; set; }
        double Width { get; set; }
        int FigureNr { get; }
        double X { get; }
        double Y { get; }
        string Data { get; set; }
        string ToString();



    }
}
