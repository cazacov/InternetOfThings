using System.Collections.Generic;

namespace DvdPlotter.Fonts
{
    public interface IFont
    {
        List<PlotChar> SupportedCharacters { get; }
    }
}