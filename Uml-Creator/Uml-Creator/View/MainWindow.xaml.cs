using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
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
        private bool isLeftMouseButtonDownOnWindow = false;

        /// <summary>
        /// Set to 'true' when dragging the 'selection rectangle'.
        /// Dragging of the selection rectangle only starts when the left mouse-button is held down and the mouse-cursor
        /// is moved more than a threshold distance.
        /// </summary>
        private bool isDraggingSelectionRect = false;

        /// <summary>
        /// Records the location of the mouse (relative to the window) when the left-mouse button has pressed down.
        /// </summary>
        private Point origMouseDownPoint;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private static readonly double DragThreshold = 5;

        /// <summary>
        /// Set to 'true' when we are trying to move the objects on the canvas
        /// Dragging starts when the mouse is held down on top of an object, then dragged around
        /// all selected objects will be moved.
        /// </summary>
        private bool isMovingFigures;


        #endregion Data Members



        public MainWindow()
        {
            InitializeComponent();
        }


        private ViewModel.MainViewModel mainViewModel
        {
            get
            {
                return (ViewModel.MainViewModel)this.DataContext;
            }
        }

        /// <summary>
        /// Event raised when the user presses down the left mouse-button.
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isLeftMouseButtonDownOnWindow = true;
                origMouseDownPoint = e.GetPosition(this);

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
                if (isDraggingSelectionRect)
                {
                    //
                    // Drag selection has ended, apply the 'selection rectangle'.
                    //

                    isDraggingSelectionRect = false;
                    ApplyDragSelectionRect();

                    e.Handled = true;
                }
                if (isMovingFigures)
                {
                    isMovingFigures = false;
                    e.Handled = true;
                }

                if (isLeftMouseButtonDownOnWindow)
                {
                    isLeftMouseButtonDownOnWindow = false;
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
            if (isDraggingSelectionRect)
            {
                //
                // Drag selection is in progress.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                UpdateDragSelectionRect(origMouseDownPoint, curMouseDownPoint);

                e.Handled = true;
            }
            else if (isLeftMouseButtonDownOnWindow)
            {
                /*
                 The user is left-dragging the mouse,
                 but don't initiate drag selection until
                 they have dragged past the threshold value and their mouse position isnt
                 on any object on screen, if mouse is on object, then move object.    
                 */
                Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);

                if (IsMouseOnFigure(curMouseDownPoint))
                {
                    isMovingFigures = true;

                    InitMoveSelectionRect(origMouseDownPoint,curMouseDownPoint);
                }
                if (dragDistance > DragThreshold && !IsMouseOnFigure(curMouseDownPoint))
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence drag selection.
                    //
                    isDraggingSelectionRect = true;

                    //
                    //  Clear selection immediately when starting drag selection.
                    //
                    figureListBox.SelectedItems.Clear();

                    InitDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                }
                
                e.Handled = true;
            }
        }

        private void InitMoveSelectionRect(Point origMouseDownPoint, Point curMouseDownPoint)
        {
            updateMoveSelection(origMouseDownPoint,curMouseDownPoint);
        }
        /// <summary>
        /// moves all selected figures with the mouse positions
        /// 
        /// </summary>
        /// <param name="origMouseDownPoint"></param>
        /// <param name="curMouseDownPoint"></param>
        private void updateMoveSelection(Point origMouseDownPoint, Point curMouseDownPoint)
        {
            //1. we figure out the vector of movement that each object must move
            //2. we have a loop that spans over all objects that are selected.
            //3. we change the pos of each object

            double xDirection = origMouseDownPoint.X - curMouseDownPoint.X;
            double  yDirection = origMouseDownPoint.Y - curMouseDownPoint.Y;


            foreach (FigureViewModel figure in this.mainViewModel.FiguresViewModels)
            {
                figure.X = figure.X + xDirection;
                figure.Y = figure.Y + yDirection;

            }
        }

        ///<summary>
        ///This method is used to determine if the mouse is currently on top of any of the objects on the canvas.
        /// Only works on figures, lines and other objects need to be added... 
        ///</summary>


        public bool IsMouseOnFigure(Point mousePoint)
        {

            foreach (FigureViewModel figure in this.mainViewModel.FiguresViewModels)
            {
                //Vi skal se om den ligger inden for figurens område.
                if (mousePoint.X >= figure.X && mousePoint.X < (figure.Width + figure.X))
                {
                    
                }
                if (figure.X == mousePoint.X && figure.Y == mousePoint.Y)
                {
                    return true;
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
            figureListBox.SelectedItems.Clear();

            //
            // Find and select all the list box items.
            //
            foreach (FigureViewModel figure in this.mainViewModel.FiguresViewModels)
            {
                Rect itemRect = new Rect(figure.X, figure.Y, figure.Width, figure.Height);
                if (dragRect.Contains(itemRect))
                {
                    figureListBox.SelectedItems.Add(figure);
                }
            }
        }


    }
}
