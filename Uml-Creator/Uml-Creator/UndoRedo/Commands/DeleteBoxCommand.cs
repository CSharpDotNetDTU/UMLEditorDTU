using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteBoxCommand : IUndoCommand
    {
        private ObservableCollection<FigureViewModel> boxes;
        private FigureViewModel box;

        public DeleteBoxCommand(ObservableCollection<FigureViewModel> boxes, FigureViewModel box)
        {
            this.boxes = boxes;
            this.box = box;
        }


        public void Execute()
        {
            boxes.Remove(box);
        }

        public void Unexecute()
        {
            boxes.Add(box);
        }
    }
}
