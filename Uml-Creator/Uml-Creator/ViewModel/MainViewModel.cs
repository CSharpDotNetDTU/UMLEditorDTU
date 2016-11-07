using System;
using System.ComponentModel;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace Uml_Creator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _x;
        private object _content;
        private string filename;


        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public ICommand BtnLoadCommand { get; }

        public ICommand BtnGemCommand { get; }

        public ICommand BtnExportCommand { get; }

        public double Y { get; set; }

        public MainViewModel()
        {
            Content = new Gem_Load();
            BtnLoadCommand = new RelayCommand(Load_Click);
            BtnGemCommand = new RelayCommand(Save_Click);
            BtnExportCommand = new RelayCommand<Canvas>(Export_Click);

        }


        private void Load_Click()
        {
            OpenFileDialog loadfildialog = new OpenFileDialog();
            loadfildialog.Filter = "XML files (*.xml)|*.xml";
            if (loadfildialog.ShowDialog() == DialogResult.OK)
            {
                //  textBox1.Text = File.ReadAllText(loadfildialog.FileName);
            }
        }

        private void Save_Click()
        {
            SaveFileDialog gemfildialog = new SaveFileDialog();
            gemfildialog.Filter = "XML files (*.xml)|*.xml";
            if (gemfildialog.ShowDialog() == DialogResult.OK)
            {
                // File.WriteAllText(gemfildialog.FileName, textBox1.Text);
            }

        }

        private void Export_Click(Canvas canvas)
        {
            SaveFileDialog exportfildialog = new SaveFileDialog();
            exportfildialog.Filter = "PNG files (*.png)|*.png";

            if (exportfildialog.ShowDialog() == DialogResult.OK)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                                            (int)canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);

                var crop = new CroppedBitmap(rtb, new Int32Rect(50, 50, 250, 250));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                using (var fs = File.OpenWrite(filename))
                {
                    pngEncoder.Save(fs);
                }
            }
            }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
