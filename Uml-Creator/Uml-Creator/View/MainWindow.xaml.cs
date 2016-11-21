using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Uml_Creator.ViewModel;


namespace Uml_Creator.View
{
    //
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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


        private ViewModel.MainViewModel MainViewModel
        {
            get
            {
                return (ViewModel.MainViewModel)this.DataContext;
            }
        }


        ///<summary>
        /// This is the events that happens when mouse is pressed down on a figure
        /// </summary>
        private void Figure_MouseDown(object sender, MouseButtonEventArgs e)
        {

            ///
            /// Anything but left click is disabled for now.....
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            var figure = (FrameworkElement) sender;
            var figureViewModel = (FigureViewModel) figure.DataContext;


            _isLeftMouseDownOnFigure = true;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                //
                // Control key was held down.
                // This means that the figure is being added to or removed from the existing selection.
                // Don't do anything yet, we will act on this later in the MouseUp event handler.
                //
                isLeftMouseAndControlDownOnFigure = true;
            }
            else
            {
                //
                // Control key is not held down.
                //
                isLeftMouseAndControlDownOnFigure = false;

                if (FigureListBox.SelectedItems.Count == 0)
                {
                    //
                    // Nothing already selected, select the item.
                    //
                    FigureListBox.SelectedItems.Add(figureViewModel);
                }
                else if (FigureListBox.SelectedItems.Contains(figureViewModel))
                {
                    // 
                    // Item is already selected, do nothing.
                    // We will act on this in the MouseUp if there was no drag operation.
                    //
                }
                else
                {
                    //
                    // Item is not selected.
                    // Deselect all, and select the item.
                    //
                    FigureListBox.SelectedItems.Clear();
                    FigureListBox.SelectedItems.Add(figureViewModel);
                }
            }

            figure.CaptureMouse();
            _origMouseDownPoint = e.GetPosition(this);

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Figure_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isLeftMouseDownOnFigure)
            {
                var figure = (FrameworkElement)sender;
                var figureViewModel = (FigureViewModel)figure.DataContext;

                
                if (!_isDraggingFigure)
                {
                    //
                    // Execute mouse up selection logic only if there was no drag operation.
                    //
                    if (isLeftMouseAndControlDownOnFigure)
                    {
                        //
                        // Control key was held down.
                        // Toggle the selection.
                        //
                        if (this.FigureListBox.SelectedItems.Contains(figureViewModel))
                        {
                            //
                            // Item was already selected, control-click removes it from the selection.
                            //
                            this.FigureListBox.SelectedItems.Remove(figureViewModel);
                        }
                        else
                        {
                            // 
                            // Item was not already selected, control-click adds it to the selection.
                            //
                            this.FigureListBox.SelectedItems.Add(figureViewModel);
                        }
                    }
                    else
                    {
                        //
                        // Control key was not held down.
                        //
                        if (this.FigureListBox.SelectedItems.Count == 1 &&
                            this.FigureListBox.SelectedItem == figureViewModel)
                        {
                            //
                            // The item that was clicked is already the only selected item.
                            // Don't need to do anything.
                            //
                        }
                        else
                        {
                            //
                            // Clear the selection and select the clicked item as the only selected item.
                            //
                            this.FigureListBox.SelectedItems.Clear();
                            this.FigureListBox.SelectedItems.Add(figureViewModel);
                        }
                    }
                }

                figure.ReleaseMouseCapture();
                _isLeftMouseDownOnFigure = false;
                isLeftMouseAndControlDownOnFigure = false;

                e.Handled = true;
            }

            _isDraggingFigure = false;


        }
        /// <summary>
        /// 
        /// </summary>
        private void Figure_MouseMove(object sender, MouseEventArgs e)
        {


            if (_isDraggingFigure)
            {
                //
                // Drag-move selected rectangles.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                Point dragDelta = new Point(curMouseDownPoint.X - _origMouseDownPoint.X, curMouseDownPoint.Y - _origMouseDownPoint.Y);
                _origMouseDownPoint = curMouseDownPoint;
                InitMoveSelectionRect(dragDelta);

            }
            else if (_isLeftMouseDownOnFigure)
            {
                //
                // The user is left-dragging the rectangle,
                // but don't initiate the drag operation until
                // the mouse cursor has moved more than the threshold value.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - _origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence dragging the rectangle.
                    //
                    _isDraggingFigure = true;
                }

                e.Handled = true;
            }
        }





    /// <summary>
    /// Event raised when the user presses down the left mouse-button.
    /// </summary>
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _isLeftMouseButtonDownOnWindow = true;
               // origMouseDownPoint = e.GetPosition(this);
                _origMouseDownPoint = e.GetPosition(FigureListBox);

                this.CaptureMouse();

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised when the user releases the left mouse-button.
        /// </summary>
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (_isDraggingSelectionRect)
                {
                    //
                    // Drag selection has ended, apply the 'selection rectangle'.
                    //

                    _isDraggingSelectionRect = false;
                    ApplyDragSelectionRect();

                    e.Handled = true;
                }
               

                if (_isLeftMouseButtonDownOnWindow)
                {
                    _isLeftMouseButtonDownOnWindow = false;
                    this.ReleaseMouseCapture();

                    e.Handled = true;
                }
                
            }
        }

        /// <summary>
        /// Event raised when the user moves the mouse button.
        /// </summary>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggingSelectionRect)
            {
                //
                // Drag selection is in progress.
                //
               Point curMouseDownPoint = e.GetPosition(FigureListBox);
                //Point curMouseDownPoint = e.GetPosition(this);
                UpdateDragSelectionRect(_origMouseDownPoint, curMouseDownPoint);

                e.Handled = true;
            }
            if (_isLeftMouseButtonDownOnWindow)
            {
                
                Point curMouseDownPoint = e.GetPosition(FigureListBox);
                //Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - _origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);

                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence drag selection.
                    //
                    _isDraggingSelectionRect = true;

                    //
                    //  Clear selection immediately when starting drag selection.
                    //
                    FigureListBox.SelectedItems.Clear();

                    InitDragSelectionRect(_origMouseDownPoint, curMouseDownPoint);
                }
                
                e.Handled = true;
            }
        }

        private void InitMoveSelectionRect(Point dragDelta)
        {
            UpdateMoveSelection(dragDelta);
        }
        /// <summary>
        /// moves all selected figures with the mouse positions
        /// 
        /// </summary>
        private void UpdateMoveSelection(Point dragDelta)
        {
            //1. we figure out the vector of movement that each object must move
            //2. we have a loop that spans over all objects that are selected.
            //3. we change the pos of each object that is selected

            foreach (FigureViewModel figure in FigureListBox.SelectedItems)
            {
                figure.X += dragDelta.X;
                figure.Y += dragDelta.Y;


            }
          
        }

        ///<summary>
        ///This method is used to determine if the mouse is currently on top of any of the objects on the canvas.
        /// Only works on figures, lines and other objects need to be added... 
        ///</summary>


        public bool IsMouseOnFigure(Point mousePoint)
        {
            

            foreach (FigureViewModel figure in FigureListBox.Items)
            {
                
                ///
                /// Tolerance is used because the mouse can move outside the area, so it helps to have a little extra area to hold on to while moving
                double TOLERANCE = 20.0;
                //Vi skal se om den ligger inden for figurens område.
                if (mousePoint.X >= figure.X- TOLERANCE && mousePoint.X < (figure.Width + figure.X + TOLERANCE))
                {
                    if (mousePoint.Y >= figure.Y - TOLERANCE && mousePoint.Y < (figure.Height + figure.Y + TOLERANCE))
                    {
                        
                        return true;
                    }
                }



            }
            return false;


        }

        /// <summary>
        /// Initialize the rectangle used for drag selection.
        /// </summary>
        private void InitDragSelectionRect(Point pt1, Point pt2)
        {
            UpdateDragSelectionRect(pt1, pt2);

            dragSelectionCanvas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Update the position and size of the rectangle used for drag selection.
        /// </summary>
        private void UpdateDragSelectionRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Determine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle used for drag selection.
            //
            Canvas.SetLeft(dragSelectionBorder, x);
            Canvas.SetTop(dragSelectionBorder, y);
            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = height;
        }

        /// <summary>
        /// Select all nodes that are in the drag selection rectangle.
        /// </summary>
        private void ApplyDragSelectionRect()
        {
            dragSelectionCanvas.Visibility = Visibility.Collapsed;

            double x = Canvas.GetLeft(dragSelectionBorder);
            double y = Canvas.GetTop(dragSelectionBorder);
            double width = dragSelectionBorder.Width;
            double height = dragSelectionBorder.Height;
            Rect dragRect = new Rect(x, y, width, height);

            //
            // Inflate the drag selection-rectangle by 1/10 of its size to 
            // make sure the intended item is selected.
            //
            dragRect.Inflate(width / 10, height / 10);

            //
            // Clear the current selection.
            //
            FigureListBox.SelectedItems.Clear();

            //
            // Find and select all the list box items.
            //
            foreach (FigureViewModel figure in this.MainViewModel.FiguresViewModels)
            {
                Rect itemRect = new Rect(figure.X, figure.Y, figure.Width, figure.Height);
                if (dragRect.Contains(itemRect))
                {
                    FigureListBox.SelectedItems.Add(figure);
                }
            }
        }


    }
}
