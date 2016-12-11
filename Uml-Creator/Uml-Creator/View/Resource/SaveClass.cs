using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Uml_Creator.ViewModel;

namespace Uml_Creator.View.Resource
{
    public class SaveClass : ISerializable
    {
        public ObservableCollection<FigureViewModel> Figures;
        public ObservableCollection<LineViewModel> Lines;
        public SaveClass() { }

        public SaveClass(ObservableCollection<FigureViewModel> figures, ObservableCollection<LineViewModel> lines)
        {
            this.Figures = figures;
            this.Lines = lines;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
