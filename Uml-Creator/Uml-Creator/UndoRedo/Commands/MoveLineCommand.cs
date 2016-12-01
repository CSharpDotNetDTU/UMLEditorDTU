using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class MoveLineCommand : IUndoCommand
    {
        private LineViewModel line;
        private int oldOriginBox;
        private int newOriginBox;
        private int oldTargetBox;
        private int newTargetBox;

        public MoveLineCommand(LineViewModel line, int oldOriginBox, int newOriginBox, int oldTargetBox, int newTargetBox)
        {
            this.line = line;
            this.oldOriginBox = oldOriginBox;
            this.newOriginBox = newOriginBox;
            this.oldTargetBox = oldTargetBox;
            this.newTargetBox = newTargetBox;
        }

        public void Execute()
        {
            line.OriginFigureNr = newOriginBox;
            line.TargetFigureNr = newTargetBox;
        }

        public void Unexecute()
        {
            line.OriginFigureNr = oldOriginBox;
            line.TargetFigureNr = oldTargetBox;
        }
    }
}
