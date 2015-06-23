using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Drivers;
using DvdPlotter.Fonts;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DvdPlotter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page, ILogger
    {
        private Plotter plotter;
        private Painter painter;
        private bool isCalibrated = false;
        private bool isPainting = false;

        public MainPage()
        {
            this.InitializeComponent();
            InitAll();
        }

        public void WriteLn(string message, LogType logType)
        {
            this.txt.Select(this.txt.ContentStart, this.txt.ContentStart);
            switch (logType)
            {
                case LogType.Info:
                    this.txt.Foreground = new SolidColorBrush(Colors.LightGray);
                    break;
                case LogType.Success:
                    this.txt.Foreground = new SolidColorBrush(Colors.LightGreen);
                    break;
                case LogType.Warning:
                    this.txt.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case LogType.Error:
                    this.txt.Foreground = new SolidColorBrush(Colors.Red);
                    break;
            }
            this.txt.Text = this.txt.Text.Insert(0,
                $"{DateTime.Now:HH:mm:ss:ff}: {message}{Environment.NewLine}");
        }

        private async void InitAll()
        {
            this.plotter = new Plotter(this);
            await plotter.Init();
            await plotter.PenUp();
            this.painter = new Painter(plotter, this);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isCalibrated)
            {
                await plotter.Calibrate();
                plotter.Stop();
                isCalibrated = true;
            }

            try
            {
                this.isPainting = true;
                if (sender == btnSquares)
                {
                    await painter.Squares();
                }
                else if (sender == btnHilbert)
                {
                    await painter.Hilbert();
                }
                else if (sender == btnLines)
                {
                    await painter.Sun();
                }
                else if (sender == btnStar)
                {
                    await painter.Star();
                }
                else if (sender == btnDemoXY)
                {
                    await painter.DemoXY();
                }
                else if (sender == btnPenDemo)
                {
                    await painter.PenDemo();
                }
            }
            finally
            {
                plotter.Stop();
                this.isPainting = false;
            }
            return;
        }

        private async void btnCalibrate_Click(object sender, RoutedEventArgs e)
        {
            await plotter.Calibrate();
            plotter.Stop();
        }
    }
}
