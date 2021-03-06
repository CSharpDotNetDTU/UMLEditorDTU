﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.UndoRedo;
using Uml_Creator.UndoRedo.Commands;
using Model.Model;


namespace Uml_Creator.ViewModel
{
    public class FigureViewModel :ISerializable, INotifyPropertyChanged, IFigure
    {
        public double CenterX => Figure.Width/2 + X;
        public double CenterY => Figure.Height / 2 + Y;
        private UndoRedoController undoRedoController = UndoRedoController.Instance;
        public ICommand AddMethod => new RelayCommand(OnAddMethod);
        public ICommand RemoveMethod => new RelayCommand(OnRemoveMethod);
        public double _width => Figure.Width;
        public double _height => Figure.Height;
        private Point _topPoint;
        private Point _leftPoint;
        private Point _rightPoint;
        private Point _bottomPoint;
        private List<LineViewModel> connectedLines = new List<LineViewModel>();

        public void addLine(LineViewModel line)
        {
            connectedLines.Add(line);
        }


        public Point TopPoint
        {
            get     
            {
                _topPoint.X = X + (Width / 2);
                _topPoint.Y = Y;
                return _topPoint;
            }
            set
            {
                _topPoint = value;
                OnPropertyChanged("topPoint");
            }
        }

        public Point LeftPoint
        {
            get
            {
                _leftPoint.X = X;
                _leftPoint.Y = Y + (Height/2);

                return _leftPoint;
            }
            set
            {
                _leftPoint = value;
                OnPropertyChanged("leftPoint");
            }
        }

        public Point RightPoint
        {
            get
            {
                _rightPoint.X = X + Width;
                _rightPoint.Y = Y + (Height/2);

                return _rightPoint;
            }
            set
            {
                _rightPoint = value;
                OnPropertyChanged("rightPoint");
            }
        }

        public Point BottomPoint
        {
            get
            {
                _bottomPoint.X = X + (Width/2);
                _bottomPoint.Y = Y + Height;

                return _bottomPoint;
            }
            set
            {
                _bottomPoint = value;
                OnPropertyChanged("_bottomPoint");
            }
        }

        #region MouseMembers

        
        private  Point _origMouseDownPoint;
        public Point OrigShapePostion;
        public double XOffset;
        public double YOffset;


        #endregion

        public ObservableCollection<string> AttributesCollection { get; set; } = new ObservableCollection<string>();
        public ICommand AddAttribute => new RelayCommand(OnAddAttribute);
        public ICommand RemoveAttribute => new RelayCommand(OnRemoveAttribute);
        public ClassContent SelectedMethod { get; set; }
        public ClassContent SelectedAttribute { get; set; }
        protected Figure Figure { get; }

        protected FigureViewModel(Figure figure)
        {
            Figure = figure;
        }

        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);

        private void OnMouseLeftBtnDown(MouseButtonEventArgs obj)
        {
            var visual = obj.Source as UIElement;
            if (visual == null) return;
            _origMouseDownPoint = Mouse.GetPosition(visual);
            OrigShapePostion = new Point(X, Y);
            MainViewModel.IsDragging = true;
        }

        private void OnMouseLeftUp(MouseButtonEventArgs obj)
        {
            var visual = obj.Source as UIElement;
            if (visual == null)
            {
                return;
            }
            if (!MainViewModel.IsDragging) return;
            
            UndoRedoController.Instance.DoExecute(new MoveBoxCommand(this, OrigShapePostion, new Point(X, Y)));
            MainViewModel.IsDragging = false;
            Mouse.Capture(null);
            obj.Handled = true;
        }
        
        private void OnAddAttribute()
        {
            undoRedoController.DoExecute(new AddAttributeCommand(this, new ClassContent("Attribute")));
        }

        private void OnRemoveAttribute()
        {
            undoRedoController.DoExecute(new DeleteAttribute(this, SelectedAttribute));
        }

        private void OnAddMethod()
        {
            undoRedoController.DoExecute(new AddMethod(this, new ClassContent("method")));
            
        }

        private void OnRemoveMethod()
        {
            
            undoRedoController.DoExecute(new DeleteMethodCommand(this, SelectedMethod));
        }

        public FigureViewModel(double x, double y, double width, double height, string data, EFigure type, bool isSelected, string name, ObservableCollection<Object> methods, ObservableCollection<Object> attributes) : this(new Figure())
        {
            Figure.X = x;
            Figure.Y = y;
            Figure.Width = width;
            Figure.Height = height;
            Figure.Data = data;
            Figure.Type = type;
            Figure.IsSelected = isSelected;
            Figure.Name = name;
            Figure.MethodCollection = methods;
            Figure.AttributeCollection = attributes;

        }

        public FigureViewModel(FigureViewModel figure)
        {
            Figure = new Figure();
            Figure.X = figure.X;
            Figure.Y = figure.Y;
            Figure.Width = figure.Width;
            Figure.Height = figure.Height;
            Figure.Data = figure.Data;
            Figure.Type = figure.Type;
            Figure.IsSelected = false;
            Figure.Name = figure.Name;
            Figure.AttributeCollection = figure.AttributeCollection;
            Figure.MethodCollection = figure.MethodCollection;
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
                OnPropertyChanged("rightPoint");
                OnPropertyChanged("leftPoint");

                OnPropertyChanged("bottomPoint");

                OnPropertyChanged("topPoint");
                giveLineNewCords();
                OnPropertyChanged(nameof(Height));

            }
        }

        private void giveLineNewCords()
        {

                for (int i = 0; i < connectedLines.Count; i++)
                {
                    connectedLines[i].calculateShortestLine();
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
                OnPropertyChanged("rightPoint");
                OnPropertyChanged("leftPoint");

                OnPropertyChanged("bottomPoint");

                OnPropertyChanged("topPoint");
                giveLineNewCords();
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

        public ObservableCollection<Object> MethodCollection
        {
            get { return Figure.MethodCollection; }

            set
            {
                Figure.MethodCollection = value;
                OnPropertyChanged("MethodCollection");
            }
        }

        public ObservableCollection<Object> AttributeCollection
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
