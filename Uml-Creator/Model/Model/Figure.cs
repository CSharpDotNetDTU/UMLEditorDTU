﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;
using Uml_Creator;

namespace Uml_Creator.Model
{
    public class Figure :IFigure
    {
        private static int _figureNr;
        public void reset() { _figureNr = 0; }
        public int FigureNr { get; } = _figureNr++;
        public EFigure Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Data { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<MethodModel> MethodCollection {get; set; } = new ObservableCollection<MethodModel>();
        public ObservableCollection<AttributeModel> AttributeCollection { get; set; } = new ObservableCollection<AttributeModel>();


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
