using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Model.Model
{
    public class Line : ILine
    {
        public int TargetFigureNr { get; set; }
        public int OriginFigureNr { get; set; }
        public string TargetLabel { get; set; }
        public string OriginLabel { get; set; }
        public ELine Type { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
}
