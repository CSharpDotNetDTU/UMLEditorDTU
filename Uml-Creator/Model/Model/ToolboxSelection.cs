﻿namespace Model.Model
{
    class ToolboxSelection
    {
        public string SelectedList { get; set; }

        public ToolboxSelection(string selectedList)
        {
            SelectedList = selectedList;
        }

        public override string ToString()
        {
            return SelectedList;
        }
    }

    
}
