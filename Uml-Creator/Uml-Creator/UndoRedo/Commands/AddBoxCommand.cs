using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    internal class AddBoxCommand : IUndoCommand
    {
        private ObservableCollection<FigureViewModel> boxes;
        private FigureViewModel box;

        public AddBoxCommand(ObservableCollection<FigureViewModel> _boxes, FigureViewModel _box)
        {
            boxes = _boxes;
            box = _box;
        }

        public void Execute()
        {
            boxes.Add(box);
        }

        public void Unexecute()
        {
            boxes.Remove(box);
        }
    }
}
