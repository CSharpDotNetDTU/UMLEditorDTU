using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;

namespace Uml_Creator.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        #region data members

        public ObservableCollection<FigureViewModel> FiguresViewModels { get; private set; }
        public ObservableCollection<Line> Lines { get; private set; }

        #endregion data members


        public ViewModel()
        {
            FiguresViewModels = new ObservableCollection<FigureViewModel>
            {
               
                //new FigureViewModel() {20.0,20.0,50.0,60.0,"lars",EFigure.ClassSquare},
               
                 new FigureViewModel(50.0,120.0,50.0,60.0,"Peter",EFigure.ClassSquare),

                 new FigureViewModel(30.0,80.0,20.0,30.0,"Lars",EFigure.ClassSquare)
            };
        }


        public ObservableCollection<FigureViewModel> FigureViewModels
        {
            get
            {
                return FiguresViewModels;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
