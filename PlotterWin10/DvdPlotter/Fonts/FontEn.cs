using System.Collections.Generic;

namespace DvdPlotter.Fonts
{
    public class FontEn : IFont
    {
        public List<PlotChar> SupportedCharacters => new List<PlotChar>()
        {
            new PlotChar('H', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(false, 0, -50),
                new LineInstruction(true, 100, 0),
                new LineInstruction(false, 0, 50),
                new LineInstruction(true, 0, -100),
            }),
            new PlotChar('E', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, 0),
                new LineInstruction(false, 0, -50),
                new LineInstruction(true, -100, 0),
                new LineInstruction(false, 0, -50),
                new LineInstruction(true, 100, 0),
            }),
            new PlotChar('L', new List<LineInstruction>()
            {
                new LineInstruction(false, 0, 100),
                new LineInstruction(true, 0, -100),
                new LineInstruction(true, 100, 0),
            }),
            new PlotChar('O', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, 0),
                new LineInstruction(true, 0, -100),
                new LineInstruction(true, -100, 0),
            }),
            new PlotChar('W', new List<LineInstruction>()
            {
                new LineInstruction(false, 0, 100),
                new LineInstruction(true, 0, -100),
                new LineInstruction(true, 50, 50),
                new LineInstruction(true, 50, -50),
                new LineInstruction(true, 0, 100),
            }),
            new PlotChar('R', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, -25),
                new LineInstruction(true, -100, -25),
                new LineInstruction(true, 100, -50),
            }),
            new PlotChar('D', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, -25),
                new LineInstruction(true, 0, -50),
                new LineInstruction(true, -100, -25),
            }),
            new PlotChar('U', new List<LineInstruction>()
            {
                new LineInstruction(false, 0, 100),
                new LineInstruction(true, 0, -100),
                new LineInstruction(true, 100, 0),
                new LineInstruction(true, 0, 100),
            }),
            new PlotChar('!', new List<LineInstruction>()
            {
                new LineInstruction(false, 50, 0),
                new LineInstruction(true, 0, 20),
                new LineInstruction(false, 0, 30),
                new LineInstruction(true, 0, 50),
            }),
            new PlotChar('.', new List<LineInstruction>()
            {
                new LineInstruction(false, 45, 0),
                new LineInstruction(true, 0, 10),
                new LineInstruction(true, 10, 0),
                new LineInstruction(true, 0, -10),
                new LineInstruction(true, -10, 0),
            }),
        };
    }
}
