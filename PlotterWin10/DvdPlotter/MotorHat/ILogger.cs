using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorHat
{
    public interface ILogger
    {
        void WriteLn(string message);
    }
}
