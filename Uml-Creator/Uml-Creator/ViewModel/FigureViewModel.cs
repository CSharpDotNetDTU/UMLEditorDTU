using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        #region MouseMembers

        private bool _isDraggingFigure = false;
        private  Point _origMouseDownPoint;
        private Point _origShapePostion;


        #endregion

        public ObservableCollection<string> AttributesCollection { get; set; } = new ObservableCollection<string>();

        protected Figure Figure { get; }

        protected FigureViewModel(Figure figure)
        {
            Figure = figure;
        }

        /*public FigureViewModel() : this(new Figure())    
        {
        }
        */

        public ICommand AddMethod => new RelayCommand(OnAddMethod);

        public ICommand RemoveMethod => new RelayCommand(OnRemoveMethod);

        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);

        public ICommand OnMouseMoveCommand => new RelayCommand<UIElement>(OnMouseMove);

        private void OnMouseMove(UIElement obj)
        {
            if (!_isDraggingFigure) return;
            if (obj == null)
            {
                return;
            }

            var pos = Mouse.GetPosition(VisualTreeHelper.GetParent(obj) as IInputElement);

            X = X + pos.X - _origMouseDownPoint.X;
            Y = Y + pos.Y - _origMouseDownPoint.Y;
        }

        private void OnMouseLeftBtnDown(MouseButtonEventArgs obj)
        {
            var visual = obj.Source as UIElement;
            if (visual == null) return;



            if (!IsSelected)
            {
                IsSelected = true;
                visual.Focus();
                obj.Handled = true;
                return;
            }

            if (!IsSelected && obj.MouseDevice.Target.IsMouseCaptured) return;
            obj.MouseDevice.Target.CaptureMouse();

            _origMouseDownPoint = Mouse.GetPosition(visual);
            _origShapePostion = new Point(X, Y);
            _isDraggingFigure = true;
        }


        private void OnMouseLeftUp(MouseButtonEventArgs obj)
        {
            if (!_isDraggingFigure)
            {
                return;
            }

            UndoRedoController.Instance.DoExecute(new MoveBoxCommand(this, _origShapePostion, new Point(X, Y)));
            _isDraggingFigure = false;
            IsSelected = false;
            //Ikke sikker på hvad dette gør...
            Mouse.Capture(null);
            obj.Handled = true;
        }

        private void OnAddMethod()
        {
            Console.WriteLine("HEEEEEJ");
            undoRedoController.DoExecute(new AddMethod(this, new MethodViewModel()));
        }

        private void OnRemoveMethod()
        {
            undoRedoController.DoExecute(new DeleteMethodCommand(this, new MethodViewModel()));
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
            Figure.Name = "ExampleClass";

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
