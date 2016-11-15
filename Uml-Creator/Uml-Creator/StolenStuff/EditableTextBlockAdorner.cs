using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Uml_Creator.StolenStuff
{
    /// <summary>
    /// Taken from here: http://www.codeproject.com/Articles/72544/Editable-Text-Block-in-WPF
    /// Edits made by Martin
    /// 
    /// Adorner class which shows textbox over the text block when the Edit mode is on.
    /// </summary>
    public class EditableTextBlockAdorner : Adorner
    {
        private readonly VisualCollection _collection;

        private readonly TextBox _textBox;

        private readonly TextBlock _textBlock;


        private bool _wasEnterLastButtonPressed;
        public EditableTextBlockAdorner(EditableTextBlock adornedElement)
            : base(adornedElement)
        {
            _collection = new VisualCollection(this);
            _textBox = new TextBox();
            _textBlock = adornedElement;
            Binding binding = new Binding("Text") {Source = adornedElement};
            _textBox.SetBinding(TextBox.TextProperty, binding);
            _textBox.AcceptsReturn = true;
            _textBox.MaxLength = adornedElement.MaxLength;
            _textBox.KeyUp += _textBox_KeyUp;
            _collection.Add(_textBox);
           
        }

        void _textBox_KeyUp(object sender, KeyEventArgs e)
        {
            BindingExpression expression;
            if (e.Key == Key.Enter)
            {
                //we want it to be able to contain new lines, but not add the newline made when enter is pressed with shift.
                //We do this b
                //_textBox.Text = _textBox.Text.Replace("\r\n", string.Empty{
                _wasEnterLastButtonPressed = true;
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                   
                        int i = _textBox.Text.LastIndexOf("\r\n");
                        if (i >= 0 && _wasEnterLastButtonPressed)
                        {
                            _textBox.Text = _textBox.Text.Substring(0, i) + _textBox.Text.Substring(i + 2);
                        }
                    
               
                    _wasEnterLastButtonPressed = false;
                    return;
                }

                expression = _textBox.GetBindingExpression(TextBox.TextProperty);
                if (null != expression)
                {
                    expression.UpdateSource();
                }
                
            }
            else 
            {
                _wasEnterLastButtonPressed = false;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _collection[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _collection.Count;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBox.Arrange(new Rect(0, 0, _textBlock.DesiredSize.Width + 50, _textBlock.DesiredSize.Height * 1.5));
            _textBox.Focus();
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(null, new Pen
                                                   {
                Brush = Brushes.Gold,
                Thickness = 2
            }, new Rect(0, 0, _textBlock.DesiredSize.Width + 50, _textBlock.DesiredSize.Height * 1.5));
        }

        public event RoutedEventHandler TextBoxLostFocus
        {
            add
            {
                _textBox.LostFocus += value;
            }
            remove
            {
                _textBox.LostFocus -= value;
            }
        }

        public event KeyEventHandler TextBoxKeyUp
        {
            add
            {
                _textBox.KeyUp += value;
            }
            remove
            {
                _textBox.KeyUp -= value;
            }
        }
    }
}
