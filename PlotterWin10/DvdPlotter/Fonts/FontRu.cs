using System.Collections.Generic;

namespace DvdPlotter.Fonts
{
    public class FontRu : IFont
    {
        public List<PlotChar> SupportedCharacters => new List<PlotChar>()
        {
            new PlotChar('А', new List<LineInstruction>()
            {
                new LineInstruction(true, 50, 100),
                new LineInstruction(true, 50, -100),
                new LineInstruction(false, -83, 33),
                new LineInstruction(true, 66, 0),
            })
        };
    }
}
