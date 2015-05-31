using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvdPlotter
{
    public class Painter
    {
        private Plotter plotter;

        public Painter(Plotter plotter)
        {
            this.plotter = plotter;
        }

        public async Task Squares()
        {
            await this.plotter.PenUp();
            
            for (var x = 0; x < 140; x+= 10)
            {
                plotter.GoTo(x, x);
                await this.plotter.PenDown();
                plotter.GoTo(300 - x, x);
                plotter.GoTo(300 - x, 300 - x);
                plotter.GoTo(x, 300 - x);
                plotter.GoTo(x, x);
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Hilbert()
        {
            const int order = 5;

            await this.plotter.PenUp();
            this.plotter.GoTo(0, 0);
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
            this.plotter.GoTo(plotter.X, plotter.Y + lineLength);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoTo(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoTo(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
        }

        private void HilbertLeft(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertDown(order - 1, lineLength);
            this.plotter.GoTo(plotter.X - lineLength, plotter.Y);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoTo(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoTo(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
        }

        private void HilbertDown(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoTo(plotter.X, plotter.Y - lineLength);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoTo(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoTo(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
        }

        private void HilbertRight(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertUp(order - 1, lineLength);
            this.plotter.GoTo(plotter.X + lineLength, plotter.Y);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoTo(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoTo(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
        }

    }
}
