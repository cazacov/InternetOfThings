using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Drivers;
using MotorHat;

namespace DvdPlotter
{
    public class Plotter
    {
        private PwmDriverPCA9685 motorDriver;
        private ILogger logger;
        private StepperMotor motorX;
        private StepperMotor motorY;
        private readonly Servo servo;
        private int x;
        private int y;
        private readonly PwmDriverPCA9685 servoDriver;
        private readonly SyncDelay syncDelay = new SyncDelay();

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public Plotter(ILogger logger)
        {
            this.logger = logger;
            syncDelay.Calibrate();

            this.motorDriver = new PwmDriverPCA9685(logger, 0x60, 1600);
            this.servoDriver = new PwmDriverPCA9685(logger, 0x41, 50);
            this.motorX = new StepperMotor(logger, motorDriver, 1, 40);
            this.motorY = new StepperMotor(logger, motorDriver, 2, 40);

            this.servo = new Servo(logger, servoDriver);
        }

        public async Task Init()
        {
            await servoDriver.Init();
            await PenUp();

            await motorDriver.Init();
            motorX.SetSpeed(200);
            motorY.SetSpeed(200);
        } 

        public async Task PenUp()
        {
            servo.SetAngle(120);
            await Task.Delay(300);
        }

        public async Task PenDown()
        {
            servo.SetAngle(0);
            await Task.Delay(500);
        }

        public async Task Calibrate()
        {
            var gpio = GpioController.GetDefault();
            GpioPin switchX;
            GpioPin switchY;

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                logger.WriteLn("There is no GPIO controller on this device.");
                return;
            }

            switchX = gpio.OpenPin(5);
            switchY = gpio.OpenPin(6);
            switchX.SetDriveMode(GpioPinDriveMode.Input);
            switchY.SetDriveMode(GpioPinDriveMode.Input);

            while(switchX.Read() == GpioPinValue.High)
            {
                motorX.Step(1, Direction.Backward, StepStyle.Interleave);
            }
            await Task.Delay(100);

            while (switchY.Read() == GpioPinValue.High)
            {
                motorY.Step(1, Direction.Backward, StepStyle.Interleave);
            }
            motorX.Step(5, Direction.Forward, StepStyle.Interleave);
            motorY.Step(5, Direction.Forward, StepStyle.Interleave);
            logger.WriteLn("Calibration done");
            x = 0;
            y = 0;
            await Task.Delay(100);
            Stop();
        }

        internal void Stop()
        {
            motorDriver.SetAllPwm(0, 0);
        }

        public void GoToXY(int newX, int newY)
        {
            if (newX > x)
            {
                motorX.Step(newX - x, Direction.Forward, StepStyle.Interleave);
            }
            else if (newX < x)
            {
                motorX.Step(x - newX, Direction.Backward, StepStyle.Interleave);
            }
            x = newX;

            if (newY > y)
            {
                motorY.Step(newY - y, Direction.Forward, StepStyle.Interleave);
            }
            else if (newY < y)
            {
                motorY.Step(y - newY, Direction.Backward, StepStyle.Interleave);
            }
            y = newY;
        }

        public void GoToDiagonal(int newX, int newY)
        {
            if (newX == X || newY == Y)
            {
                GoToXY(newX, newY);
            }

            var deltaX = Math.Abs(newX - X);
            var deltaY = Math.Abs(newY - Y);

            bool incX = newX > X;
            bool incY = newY > Y;

            var posX = motorX.CurrentStep;
            var posY = motorY.CurrentStep;

            if (deltaX >= deltaY)
            {
                var xyRatio = (float) deltaY/deltaX;
                for (var i = 0; i < deltaX*4; i++)
                {
                    motorX.MicrostepCoils(posX);
                    motorY.MicrostepCoils(posY + i * (incY ? xyRatio : -xyRatio));
                    posX += incX ? 1 : -1;
                    syncDelay.Sleep(1);
                }
            }
            else
            {
                var xyRatio = (float)deltaX / deltaY;
                for (var i = 0; i < deltaY * 4; i++)
                {
                    motorX.MicrostepCoils(posX + i * (incX ? xyRatio : -xyRatio));
                    motorY.MicrostepCoils(posY);
                    posY += incY ? 1 : -1;
                    syncDelay.Sleep(1);
                }
            }
            motorX.CurrentStep = newX;
            motorY.CurrentStep = newY;
            x = newX;
            y = newY;
        }
    }
}
