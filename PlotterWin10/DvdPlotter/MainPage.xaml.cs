using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Drivers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DvdPlotter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page, ILogger
    {

        public MainPage()
        {
            this.InitializeComponent();
            InitAll();
        }

        public void WriteLn(string message)
        {
            this.txt.Text += message + Environment.NewLine;
        }

        private async void InitAll()
        {
            var plotter = new Plotter(this);
            await plotter.Init();
            plotter.PenUp();
            await plotter.Calibrate();
            plotter.Stop();

            var painter = new Painter(plotter);
            await painter.Sun();
        }
    }
}
