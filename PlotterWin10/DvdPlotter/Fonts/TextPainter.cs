using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.RemoteDesktop;

namespace DvdPlotter.Fonts
{
    public class TextPainter
    {
        private readonly Plotter plotter;
        private readonly IFont font;
        private readonly int sizeX;
        private readonly int sizeY;
        private readonly int spaceX;

        public TextPainter(Plotter plotter, IFont font, int sizeX, int sizeY, int spaceX)
        {
            this.plotter = plotter;
            this.font = font;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.spaceX = spaceX;
        }

        public async Task DrawText(string text, int startX, int startY)
        {
            await plotter.PenUp();
            plotter.GoToXY(startX, startY);
            var x = startX;
            var y = startY;

            foreach (var ch in text.ToCharArray())
            {
                var character = this.font.SupportedCharacters.FirstOrDefault(c => c.Character == ch);
                if (character != null)
                {
                    plotter.GoToXY(x, y);
                    foreach (var instruction in character.Instructions)
                    {
                        if (instruction.IsLine)
                        {
                            await plotter.PenDown();
                        }
                        else
                        {
                            await plotter.PenUp();
                        }
                        plotter.GoToDiagonal(plotter.X + instruction.RelX * sizeX / 100, plotter.Y + instruction.RelY * sizeY / 100);
                    }
                    await plotter.PenUp();
                }
                x += sizeX + spaceX;
            }
            plotter.Stop();
        }
    }
}
