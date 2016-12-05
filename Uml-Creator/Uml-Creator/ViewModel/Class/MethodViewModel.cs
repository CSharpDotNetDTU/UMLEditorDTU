using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace Uml_Creator.ViewModel.Class
{
    [ImplementPropertyChanged]
    public class MethodViewModel
    {
        public string Modiferer { get; set; } = "public";

        public string ReturnType { get; set; } = "string";

        public string Name { get; set; } = "MethodName";
    }
}
