using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;

namespace Uml_Creator.Model.Interfaces
{
    public interface ILine
    {
        int TargetFigureNr { get; set; }
        int OriginFigureNr { get; set; }
        string TargetLabel { get; set; }
        string OriginLabel { get; set; }
        ELine Type { get; set; }


    }
}
