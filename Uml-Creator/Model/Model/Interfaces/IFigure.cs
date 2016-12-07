using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;



namespace Uml_Creator.Model.Interfaces
{
    public interface IFigure : ISerializable
    {
        bool IsSelected { get; set; }
        EFigure Type { get; }
        double Height { get; set; }
        double Width { get; set; }
        int FigureNr { get; }
        double X { get; }
        double Y { get; }
        string Data { get; set; }
        string ToString();
        string Name { get; set; }

        ObservableCollection<MethodViewModel> MethodCollection { get; set; }
        ObservableCollection<AttributeViewModel> AttributeCollection { get; set; }


    }
}