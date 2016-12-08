using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Model.Model
{
    public class Figure :IFigure
    {
        private static int _figureNr;

        public int FigureNr { get; } = _figureNr++;
        public EFigure Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Data { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<Object> MethodCollection {get; set; } = new ObservableCollection<Object>();
        public ObservableCollection<Object> AttributeCollection { get; set; } = new ObservableCollection<Object>();


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
