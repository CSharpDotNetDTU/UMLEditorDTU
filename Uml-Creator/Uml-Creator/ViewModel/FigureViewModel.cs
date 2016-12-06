using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.ViewModel
{
    public class FigureViewModel :ISerializable, INotifyPropertyChanged, IFigure
    {
        public double CenterX => Figure.Width/2 + X;
        public double CenterY => Figure.Height / 2 + Y;
        public ObservableCollection<AttributeViewModel> attributes { get; }
        public ObservableCollection<MethodViewModel> methods { get; }

        protected Figure Figure { get; }

        protected FigureViewModel(Figure figure)
        {
            Figure = figure;
        }

        /*public FigureViewModel() : this(new Figure())    
        {
        }
        */

        public FigureViewModel(double x, double y, double width, double height, string data, EFigure type, bool isSelected) : this(new Figure())
        {
            Figure.X = x;
            Figure.Y = y;
            Figure.Width = width;
            Figure.Height = height;
            Figure.Data = data;
            Figure.Type = type;
            Figure.IsSelected = isSelected;

        }

        public FigureViewModel(FigureViewModel figure)
        {
            Figure.X = figure.X;
            Figure.Y = figure.Y;
            Figure.Width = figure.Width;
            Figure.Height = figure.Height;
            Figure.Data = figure.Data;
            Figure.Type = figure.Type;
            Figure.IsSelected = false;
        }

        public FigureViewModel()
        {
            Figure = new Figure();
            
        }


        public bool IsSelected
        {
            get { return Figure.IsSelected; }
            set
            {
                Figure.IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
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
                OnPropertyChanged(nameof(CenterX));
            }
        }



        public double Y
        {
            get { return Figure.Y; }
            set
            {
                Figure.Y = value;
                OnPropertyChanged("Y");
                OnPropertyChanged(nameof(CenterY));
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)Figure).GetObjectData(info, context);
        }
    }
}
