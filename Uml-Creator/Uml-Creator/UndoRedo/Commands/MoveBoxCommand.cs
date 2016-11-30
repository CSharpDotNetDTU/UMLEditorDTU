using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class MoveBoxCommand : IUndoCommand
    {
        private FigureViewModel box;
        private Point oldPosition;
        private Point newPosition;

        public MoveBoxCommand(FigureViewModel box, Point oldPosition, Point newPosition)
        {
            this.box = box;
            this.oldPosition = oldPosition;
            this.newPosition = newPosition;
        }

        public void Execute()
        {
            box.X = newPosition.X;
            box.Y = newPosition.Y;
        }

        public void Unexecute()
        {
            box.X = oldPosition.X;
            box.Y = oldPosition.Y;
        }
    }
}
