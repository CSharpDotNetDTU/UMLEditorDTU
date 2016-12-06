using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.UndoRedo;
using Uml_Creator.UndoRedo.Commands;

namespace Uml_Creator.ViewModel
{
    public class FigureViewModel :ISerializable, INotifyPropertyChanged, IFigure
    {
        public double CenterX => Figure.Width/2 + X;
        public double CenterY => Figure.Height / 2 + Y;
        private UndoRedoController undoRedoController = UndoRedoController.Instance;
        public ICommand AddMethod => new RelayCommand(OnAddMethod);
        public ICommand RemoveMethod => new RelayCommand(OnRemoveMethod);
        public ICommand AddAttribute => new RelayCommand(OnAddAttribute);
        public ICommand RemoveAttribute => new RelayCommand(OnRemoveAttribute);

        public MethodViewModel SelectedMethod { get; set; }

        public AttributeViewModel SelectedAttribute { get; set; }

        protected Figure Figure { get; }

        protected FigureViewModel(Figure figure)
        {
            Figure = figure;
        }

        /*public FigureViewModel() : this(new Figure())    
        {
        }
        */
        
        private void OnAddAttribute()
        {
            undoRedoController.DoExecute(new AddAttributeCommand(this, new AttributeViewModel()));
        }

        private void OnRemoveAttribute()
        {
            undoRedoController.DoExecute(new DeleteAttribute(this, SelectedAttribute));
        }

        private void OnAddMethod()
        {
            undoRedoController.DoExecute(new AddMethod(this, new MethodViewModel()));
            
        }

        private void OnRemoveMethod()
        {
            
            undoRedoController.DoExecute(new DeleteMethodCommand(this, SelectedMethod));
        }

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

        public int FigureNr
        {
            get { return Figure.FigureNr; }
        }

        public double Height
        {
            get { return Figure.Height; }

            set
            {
                Figure.Height = value;
                OnPropertyChanged("Height");
                Debug.WriteLine(Height+ " AYYYYYYYY");
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

        public string Name
        {
            get { return Figure.Name; }

            set
            {
                Figure.Name = value; 
                OnPropertyChanged("Name");
            }

        }

        public ObservableCollection<MethodViewModel> MethodCollection
        {
            get { return Figure.MethodCollection; }

            set
            {
                Figure.MethodCollection = value;
                OnPropertyChanged("MethodCollection");
            }
        }

        public ObservableCollection<AttributeViewModel> AttributeCollection
        {
            get { return Figure.AttributeCollection; }

            set
            {
                Figure.AttributeCollection = value;
                OnPropertyChanged("AttributeCollection");
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
