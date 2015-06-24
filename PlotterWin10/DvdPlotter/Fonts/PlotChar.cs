using System.Collections.Generic;

namespace DvdPlotter
{
    public class PlotChar
    {
        public readonly char Character;
        public readonly List<LineInstruction> Instructions;

        public PlotChar(char character, List<LineInstruction> instructions)
        {
            this.Character = character;
            this.Instructions = instructions;
        }
    }
}