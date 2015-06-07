using System.Collections.Generic;

namespace DvdPlotter.Fonts
{
    public class FontRu : IFont
    {
        public List<PlotChar> SupportedCharacters => new List<PlotChar>()
        {
            new PlotChar('Х', new List<LineInstruction>()
            {
                new LineInstruction(true, 100, 100),
                new LineInstruction(false, -100, 0),
                new LineInstruction(true, 100, -100),
            }),
            new PlotChar('А', new List<LineInstruction>()
            {
                new LineInstruction(true, 50, 100),
                new LineInstruction(true, 50, -100),
                new LineInstruction(false, -83, 33),
                new LineInstruction(true, 66, 0),
            }),
            new PlotChar('Б', new List<LineInstruction>()
            {
                new LineInstruction(false, 0, 50),
                new LineInstruction(true, 100, 0),
                new LineInstruction(true, 0, -50),
                new LineInstruction(true, -100, 0),
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, 0),
            }),
            new PlotChar('Р', new List<LineInstruction>()
            {
                new LineInstruction(true, 0, 100),
                new LineInstruction(true, 100, 0),
                new LineInstruction(true, 0, -50),
                new LineInstruction(true, -100, 0),
            }),
        };
    }
}
