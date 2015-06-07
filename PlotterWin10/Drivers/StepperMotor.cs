using System;
using MotorHat;

namespace Drivers
{
    public class StepperMotor
    {
        private const int MICROSTEPS = 8;
        private readonly int ain1;
        private readonly int ain2;
        private readonly int bin1;
        private readonly int bin2;
        private readonly PwmDriverPCA9685 driver;
        private readonly int pwmA;
        private readonly int pwmB;
        private readonly int revsteps;
        private double secPerStep;
        private int steppingCounter;
        private int currentstep;
        private readonly ILogger logger;
        private readonly SyncDelay syncDelay;

        public StepperMotor(ILogger logger, PwmDriverPCA9685 driver, int motorNr, int steps)
        {
            this.logger = logger;
            this.driver = driver;
            this.revsteps = steps;
            this.currentstep = 0;
            this.syncDelay = new SyncDelay();
            syncDelay.Calibrate();

            switch (motorNr)
            {
                case 1:
                    this.pwmA = 8;
                    this.ain2 = 9;
                    this.ain1 = 10;
                    this.pwmB = 13;
                    this.bin2 = 12;
                    this.bin1 = 11;
                    break;
                case 2:
                    this.pwmA = 2;
                    this.ain2 = 3;
                    this.ain1 = 4;
                    this.pwmB = 7;
                    this.bin2 = 6;
                    this.bin1 = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("MotorHAT Stepper motor-Nr. must be between 1 and 2 inclusive");
            }
        }

        public void SetSpeed(int rpm)
        {
            this.secPerStep = 60.0 / (this.revsteps * rpm);
            this.steppingCounter = 0;
        }


        public int OneStep(Direction dir, StepStyle style)
        {

            switch (style)
            {
                case StepStyle.Single:
                    if ((currentstep / (MICROSTEPS / 2)) % 2 != 0)
                    {
                        //# we're at an odd step, weird
                        if (dir == Direction.Forward)
                        {
                            currentstep += MICROSTEPS / 2;
                        }
                        else
                        {
                            currentstep -= MICROSTEPS / 2;
                        }
                    }
                    else
                    {
                        // go to next even step
                        if (dir == Direction.Forward)
                        {
                            currentstep += MICROSTEPS;
                        }
                        else
                        {
                            currentstep -= MICROSTEPS;
                        }
                    }
                    break;
                case StepStyle.Double:
                    if ((currentstep / (MICROSTEPS / 2) % 2) == 0)
                    {
                        //# we're at an even step, weird      
                        if (dir == Direction.Forward)
                        {
                            currentstep += MICROSTEPS / 2;
                        }
                        else
                        {
                            currentstep -= MICROSTEPS / 2;
                        }
                    }
                    else
                    {
                        // go to next odd step
                        if (dir == Direction.Forward)
                        {
                            currentstep += MICROSTEPS;
                        }
                        else
                        {
                            currentstep -= MICROSTEPS;
                        }
                    }
                    break;
                case StepStyle.Interleave:
                    if (dir == Direction.Forward)
                    {
                        currentstep += MICROSTEPS / 2;
                    }
                    else
                    {
                        currentstep -= MICROSTEPS / 2;
                    }
                    break;
            }

            //go to next 'step' and wrap around
            currentstep += MICROSTEPS * 4;
            currentstep %= MICROSTEPS * 4;

            this.driver.SetPwm(pwmA, 4096, 0);
            this.driver.SetPwm(pwmB, 4096, 0);

            //set up coil energizing!
            bool[][] step2coils = {
                new bool[]{true,    false,  false,  false },
                new bool[]{true,    true,   false,  false },
                new bool[]{false,   true,   false,  false },
                new bool[]{false,   true,   true,   false },
                new bool[]{false,   false,  true,   false },
                new bool[]{false,   false,  true,   true },
                new bool[]{ false,  false,  false,  true },
                new bool[]{ true,   false,  false,  true }
            };

            var coils = step2coils[currentstep / (MICROSTEPS / 2)];

            // print "coils state = " + str(coils)
            driver.SetPin(ain2, coils[0]);
            driver.SetPin(bin1, coils[1]);
            driver.SetPin(ain1, coils[2]);
            driver.SetPin(bin2, coils[3]);

            return currentstep;
        }

        public void Step(int steps, Direction direction, StepStyle stepstyle)
        {
            var s_per_s = (int)(secPerStep * 1000);

            if (stepstyle == StepStyle.Interleave)
            {
                s_per_s = s_per_s / 2;
            }
            

            //this.logger.WriteLn(String.Format("{0} millisec per step", s_per_s));

            for (var s = 0; s < steps; s++)
            {
                this.OneStep(direction, stepstyle);
                syncDelay.Sleep(5);
            }
        }

        public void MicrostepCoils(double position)
        {
            var angle = position*Math.PI/(MICROSTEPS*2.0);
            var powerAC = (int) (Math.Cos(angle) * 4095);
            var powerBD = (int) (Math.Sin(angle) * 4095);

            this.driver.SetPwm(pwmA, 0, Math.Abs(powerAC));
            this.driver.SetPwm(pwmB, 0, Math.Abs(powerBD));

            driver.SetPin(ain2, powerAC > 0);
            driver.SetPin(ain1, powerAC < 0);
            driver.SetPin(bin1, powerBD > 0);
            driver.SetPin(bin2, powerBD < 0);
        }

        public int CurrentStep
        {
            get { return this.currentstep; }
            set { this.currentstep = value; }
        }
    }
}
