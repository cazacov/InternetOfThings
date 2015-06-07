namespace DvdPlotter
{
    public class LineInstruction
    {
        public readonly bool IsLine;
        public readonly int RelX;
        public readonly int RelY;

        public LineInstruction(bool isLine, int relX, int relY)
        {
            this.IsLine = isLine;
            this.RelX = relX;
            this.RelY = relY;
        }
    }
}