using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.ViewModel
{
    public class FigureViewModel : INotifyPropertyChanged, IFigure
    {

        protected Figure Figure { get; }

        protected FigureViewModel(Figure figure)
        {
            Figure = figure;
        }

        /*public FigureViewModel() : this(new Figure())    
        {
        }
        */

        public FigureViewModel(double x, double y, double width, double height, string data, EFigure type) : this(new Figure())
        {
            Figure.X = x;
            Figure.Y = y;
            Figure.Width = width;
            Figure.Height = height;
            Figure.Data = data;
            Figure.Type = type;

        }


        public EFigure Type => Figure.Type;
        public int FigureNr => Figure.FigureNr;
        public double Height
        {
            get { return Figure.Height; }

            set
            {
                Figure.Height = value;
                OnPropertyChanged("Height");
            }
        }

        public double Width
        {
            get { return Figure.Width; }

            set
            {
                Figure.Width = value;
                OnPropertyChanged("Width");
            }
        }



        public double X
        {
            get { return Figure.X; }
            set
            {
                Figure.X = value;
                OnPropertyChanged("X");

            }
        }



        public double Y
        {
            get { return Figure.Y; }
            set
            {
                Figure.Y = value;
                OnPropertyChanged("Y");
            }
        }

        public string Data
        {
            get { return Figure.Data; }

            set
            {
                Figure.Data = value;
                OnPropertyChanged("Data");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
