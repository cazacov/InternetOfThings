using System.Collections.Generic;
using System.Linq;

namespace DvdPlotter.Fonts
{
    public class FontEnRu : IFont
    {
        public List<PlotChar> SupportedCharacters => 
            new FontEn().SupportedCharacters.Union(new FontRu().SupportedCharacters).ToList();
    }
}