using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorHat
{
    public class Servo
    {
        private readonly PwmDriver driver;
        private readonly ILogger logger;

        public Servo(ILogger logger, PwmDriver pwmDriver)
        {
            this.logger = logger;
            this.driver = pwmDriver;
        }

        public void SetAngle(double degree)
        {
            //this.driver.SetPWM(0, 2048, 2048);
        }

        
    }
}
