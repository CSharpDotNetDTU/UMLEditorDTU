using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{

    class PasteClassCommand : IUndoCommand
    {
        public ObservableCollection<FigureViewModel> boxes;
        public FigureViewModel box;

        public PasteClassCommand(ObservableCollection<FigureViewModel> boxes, FigureViewModel box)
        {
            this.boxes = boxes;
            this.box = new FigureViewModel(box);
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
