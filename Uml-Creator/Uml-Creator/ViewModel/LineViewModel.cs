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
    public class LineViewModel : INotifyPropertyChanged, ILine
    {
        
        protected Line Line { get; }

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
        }



        public int TargetFigureNr { get; set; }
        public int OriginFigureNr { get; set; }
        public string TargetLabel { get; set; }
        public string OriginLabel { get; set; }
        public ELine Type { get; set; }

        public FigureViewModel From;
        public FigureViewModel To;


        public event PropertyChangedEventHandler PropertyChanged;

    }
}
