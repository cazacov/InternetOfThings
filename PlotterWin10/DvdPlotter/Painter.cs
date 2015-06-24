using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drivers;
using DvdPlotter.Extension;

namespace DvdPlotter
{
    public class Painter
    {
        private Plotter plotter;
        private ILogger logger;

        public Painter(Plotter plotter, ILogger logger)
        {
            this.plotter = plotter;
            this.logger = logger;
        }

        public async Task Squares()
        {
            await this.plotter.PenUp();
            
            for (var x = 0; x < 140; x+= 10)
            {
                plotter.GoToXY(x, x);
                await this.plotter.PenDown();
                plotter.GoToXY(310 - x, x);
                plotter.GoToXY(310 - x, 310 - x);
                plotter.GoToXY(x, 310 - x);
                plotter.GoToXY(x, x);
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Diagonal()
        {
            await this.plotter.PenUp();

            plotter.GoToXY(50, 50);
            await this.plotter.PenDown();
            plotter.GoToXY(250, 50);
            plotter.GoToXY(250, 250);
            plotter.GoToXY(50, 250);
            plotter.GoToXY(50, 50);
            await this.plotter.PenUp();

            plotter.GoToDiagonal(250, 250);

            await this.plotter.PenUp();
            this.plotter.Stop();
        }

        public async Task Sun()
        {
            await this.plotter.PenUp();
            for (var phi = 0.0; phi < 2*Math.PI; phi += Math.PI/6)
            {
                plotter.GoToXY(150, 150);
                await this.plotter.PenDown();
                plotter.GoToDiagonal((int)(150 + 150.0 * Math.Cos(phi)), (int)(150 + 150.0 * Math.Sin(phi)));
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Hilbert()
        {
            const int order = 4;

            await this.plotter.PenUp();
            this.plotter.GoToXY(0, 0);
            await this.plotter.PenDown();


            HilbertUp(order, 320 >> order);
            await this.plotter.PenUp();
            this.plotter.Stop();
        }

        private void HilbertUp(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
        }

        private void HilbertLeft(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
        }

        private void HilbertDown(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
        }

        private void HilbertRight(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
        }

        public async Task Test()
        {
            plotter.GoToXY(0, 0);
            await this.plotter.PenDown();
            plotter.GoToXY(310, 0);
            plotter.GoToXY(310, 310);
            plotter.GoToXY(0, 310);
            plotter.GoToXY(0, 0);
            await this.plotter.PenUp();
            plotter.GoToXY(0, 100);
            await this.plotter.PenDown();
            plotter.GoToXY(300, 100);
            await this.plotter.PenUp();

            for (int i = 0; i < 3; i++)
            {
                plotter.GoToXY(i*100, 100);
                await this.plotter.PenDown();
                plotter.GoToDiagonal((i+1) * 100, 200);
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Star()
        {
            const int corners = 9;
            const int step = 4;
            const int r = 145;

            logger.WriteLn("Drawing a star with {0} corners.".Fmt(corners), LogType.Info);

            var angle = 0.0;
            await plotter.PenUp();
            plotter.GoToXY(150 + r, 150);
            await plotter.PenDown();
            for (var i = 0; i < corners; i++)
            {
                angle += step*2*Math.PI/corners;
                plotter.GoToDiagonal(150 + (int)(r * Math.Cos(angle)), 150 + (int)(r * Math.Sin(angle)));
            }
            await plotter.PenUp();
        }

        public async Task DemoXY()
        {
            await plotter.PenUp();
            plotter.GoToXY(0, 150);
            await Task.Delay(3000);
            plotter.GoToXY(300, 150);
            await Task.Delay(500);
            plotter.GoToXY(150, 150);
            await Task.Delay(500);
            plotter.GoToXY(150, 0);
            await Task.Delay(500);
            plotter.GoToXY(150, 300);
            await Task.Delay(500);
            plotter.GoToXY(150, 150);
        }

        public async Task PenDemo()
        {
            await plotter.PenDown();
            await Task.Delay(2000);
            await plotter.PenUp();
        }
    }
}
