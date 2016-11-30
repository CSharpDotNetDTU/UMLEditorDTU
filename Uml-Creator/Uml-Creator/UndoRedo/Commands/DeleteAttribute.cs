﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteAttribute : IUndoCommand
    {
        private FigureViewModel box;
        private AttributeViewModel attribute;

        public DeleteAttribute(FigureViewModel box, AttributeViewModel attribute)
        {
            this.box = box;
            this.attribute = attribute;
        }

        public void Execute()
        {
            box.attributes.Remove(attribute);
        }

        public void Unexecute()
        {
            box.attributes.Add(attribute);
        }
    }
}