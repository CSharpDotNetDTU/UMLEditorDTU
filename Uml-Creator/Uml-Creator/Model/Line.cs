using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.Model
{
    public class Line : ILine
    {
        public int TargetFigureNr { get; }
        public int OriginFigureNr { get; }
        public string TargetLabel { get; set; }
        public string originLabel { get; set; }
        public ELine Type { get; set; }
    }
}
