using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model.Model;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.ViewModel
{
    public class LineViewModel : INotifyPropertyChanged, ILine
    {
        
        protected Line Line { get; }
        private Point _fromPoint;
        private Point _toPoint;


        protected LineViewModel(Line line)
        {
            Line = Line;
        }

        public LineViewModel(int targetFigureNr, int originFigureNr, string targetLabel, string originLabel, ELine type) : this(new Line())
        {
            Line.TargetFigureNr = targetFigureNr;
            Line.OriginFigureNr = originFigureNr;
            Line.TargetLabel = targetLabel;
            Line.OriginLabel = originLabel;
            Line.Type = type;

        }

        public LineViewModel(Line line, FigureViewModel _from, FigureViewModel _to, ELine _type) : this(line)
        {
            From = _from;
            To = _to;
            Type = _type;
            From._line = this;
            To._line = this;

        }



        public int TargetFigureNr { get; set; }
        public int OriginFigureNr { get; set; }
        public string TargetLabel { get; set; }
        public string OriginLabel { get; set; }
        public ELine Type { get; set; }
        private FigureViewModel _from;
        private FigureViewModel _to;

        public string StrokeStyle
        {
            get
            {
                if(Type==ELine.Solid) return "10,0";
                return "5, 10";
            }
        }

        public FigureViewModel From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
            }
        }



        public FigureViewModel To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        private double _fromX;

        public double Xfrom
        {
            get
            {
                return _fromPoint.X;
            }

            set
            {
                _fromX = value;
                OnPropertyChanged(nameof(Xfrom));
            }
        }

        private double _fromY;

        public double Yfrom
        {
            get
            {
                return _fromPoint.Y;
            }

            set
            {
                _fromY = value;
                OnPropertyChanged(nameof(Yfrom));
            }
        }

        private double _toX;

        public double Xto
        {
            get
            {
                return _toPoint.X;
            }

            set
            {
                _toX = value;
                OnPropertyChanged(nameof(Xto));
            }
        }

        private double _toY;

        public double Yto
        {
            get
            {
                return _toPoint.Y;
            }

            set
            {
                _toY = value;
                OnPropertyChanged(nameof(Yto));
            }
        }


        public Point FromPoint
        {
            get
            {
                return _fromPoint;
            }

            set
            {

                
                _fromPoint = value;
                OnPropertyChanged("FromPoint");
            }
        }

        public Point ToPoint
        {
            get { return _toPoint;}
            set
            {
                _toPoint = value;
                OnPropertyChanged("ToPoint");
            }
        }


        private double calculateLineLength(Point fromsPoint, Point tosPoint)
        {
            double length = fromsPoint.X - tosPoint.X;
            double height = fromsPoint.Y - tosPoint.Y;

            double lineLength = length*length + height*height;
            lineLength = Math.Sqrt(lineLength);




            return lineLength;
        }


        private void setNewPoints()
        {
            Xfrom = _fromPoint.X;
            Yfrom = _fromPoint.Y;
            Xto = _toPoint.X;
            Yto = _toPoint.Y;
        }

        public void calculateShortestLine()
        {
            List<Point> fromPoint = new List<Point>();
            fromPoint.Add(_from.bottomPoint);
            fromPoint.Add(_from.topPoint);
            fromPoint.Add(_from.leftPoint);
            fromPoint.Add(_from.rightPoint);

            List<Point> toPoint = new List<Point>();
            toPoint.Add(_to.bottomPoint);
            toPoint.Add(_to.topPoint);
            toPoint.Add(_to.leftPoint);
            toPoint.Add(_to.rightPoint);
            double _newLength = 0;
            double _oldLength = 0;

            for (int i = 0; i < fromPoint.Count; i++)
            {
                for (int j = 0; j < toPoint.Count; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        _newLength = calculateLineLength(fromPoint[i], toPoint[j]);
                        _oldLength = _newLength;
                        FromPoint = fromPoint[i];
                        ToPoint = toPoint[j];
                    }
                    else
                    {
                        _newLength = calculateLineLength(fromPoint[i], toPoint[j]);
                        Debug.WriteLine(_newLength);


                        if (_newLength < _oldLength)
                        {
                            _oldLength = _newLength;
                            FromPoint = fromPoint[i];
                            ToPoint = toPoint[j];
                        }

                    }


                }



            
            }


        }


        public double X1 => _to.CenterX;
        public double Y1 => _to.CenterY;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
