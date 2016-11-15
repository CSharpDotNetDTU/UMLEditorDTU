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
        public int TargetFigureNr { get; set; }
        public int OriginFigureNr { get; set; }
        public string TargetLabel { get; set; }
        public string OriginLabel { get; set; }
        public ELine Type { get; set; }
    }
}
