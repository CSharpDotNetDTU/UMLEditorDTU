using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.ENUM;

namespace Uml_Creator.Model.Interfaces
{
    public interface IClassMethod
    {
        string Name { get; set; }
        EVisibility Visibility { get; set; }
        string Type { get; set; }
        List<string> Parameters { get; set; }
    }
}
