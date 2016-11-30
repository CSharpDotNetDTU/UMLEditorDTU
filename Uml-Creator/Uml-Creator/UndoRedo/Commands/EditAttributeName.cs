﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class EditAttributeName : IUndoCommand
    {
        private AttributeViewModel attribute;
        public string oldName;
        public string newName;

        public EditAttributeName(AttributeViewModel attribute, string oldName, string newName)
        {
            this.attribute = attribute;
            this.oldName = oldName;
            this.newName = newName;
        }

        public void Execute()
        {
            attribute.Name = newName;
        }

        public void Unexecute()
        {
            attribute.Name = oldName;
        }
    }
}