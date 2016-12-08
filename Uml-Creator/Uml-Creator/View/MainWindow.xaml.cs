using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Uml_Creator.ViewModel;
using ClassFolder = Uml_Creator.ViewModel.Class;

namespace Uml_Creator.View
{

    public partial class MainWindow : Window
    {
        #region Data Members

        //Source: http://www.codeproject.com/Articles/148503/Simple-Drag-Selection-in-WPF


        /// <summary>
        /// Set to 'true' when the left mouse-button is down.
        /// </summary>
        private bool _isLeftMouseButtonDownOnWindow = false;

        /// <summary>
        /// Set to 'true' when dragging the 'selection rectangle'.
        /// Dragging of the selection rectangle only starts when the left mouse-button is held down and the mouse-cursor
        /// is moved more than a threshold distance.
        /// </summary>
        private bool _isDraggingSelectionRect = false;

        /// <summary>
        /// Records the location of the mouse (relative to the window) when the left-mouse button has pressed down.
        /// </summary>
        private Point _origMouseDownPoint;

        ///<summary>
        /// 
        /// </summary>
        private bool _isLeftMouseDownOnFigure = false;

        ///<summary>
        /// 
        /// </summary>
        private bool isLeftMouseAndControlDownOnFigure = false;

        /// <summary>
        /// 
        /// </summary>
        private bool _isDraggingFigure = false;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private static readonly double DragThreshold = 5;



        #endregion Data Members

        public MainWindow()
        {
            InitializeComponent();
        }

    }

}