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
        private TextPainter textPainter;

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
            this.textPainter = new TextPainter(plotter, new FontEnRu(), 35, 70, 15);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as UIElement;
            while (ctrl != null && !(ctrl is Border))
            {
                ctrl =  VisualTreeHelper.GetParent(ctrl) as UIElement;
            }

            var ctr = ctrl as Border;
            if (ctr != null)
            {
                ctr.Background = new SolidColorBrush(Colors.DarkOliveGreen);
                await Task.Delay(100);
            }

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
                else if (sender == btnText)
                {
                    await textPainter.DrawText("HELLO", 25,160);
                    await textPainter.DrawText("WORLD!", 5, 20);
                    plotter.GoToXY(5, 290);
                }
                else if (sender == btnTextRu)
                {
                    await textPainter.DrawText("ПРИВЕТ", 5, 160);
                    await textPainter.DrawText("ХАБР!", 25, 20);
                    plotter.GoToXY(5, 290);
                }
                else if (sender == btnPcb)
                {
                    await painter.PCB();
                }
            }
            finally
            {
                plotter.Stop();
                this.isPainting = false;

                if (ctr != null)
                {
                    ctr.Background = null;
                }
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
