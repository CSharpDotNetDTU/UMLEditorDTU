using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace Uml_Creator.ViewModel.Class
{
    [ImplementPropertyChanged]
    public class UmlControl
    {
        public double X { get; set; }

        public double Y { get; set; }
    }
}
