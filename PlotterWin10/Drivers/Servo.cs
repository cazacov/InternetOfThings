namespace Drivers
{
    public class Servo
    {
        private readonly PwmDriverPCA9685 driver;
        private readonly ILogger logger;

        public Servo(ILogger logger, PwmDriverPCA9685 pwmDriver)
        {
            this.logger = logger;
            this.driver = pwmDriver;
        }

        public void SetAngle(double degree)
        {
            var x = 204.8 + degree / 180.0 * 204.8; 
            this.driver.SetPwm(0, 0, (int)x);
        }

        
    }
}
