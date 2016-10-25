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
        int TargetFigureNr { get; }
        int OriginFigureNr { get; }
        string TargetLabel { get; }
        string originLabel { get; }
        ELine Type { get; set; }


    }
}
