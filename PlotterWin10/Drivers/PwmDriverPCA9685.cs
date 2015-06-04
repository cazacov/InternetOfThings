using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Drivers
{
    public class PwmDriverPCA9685
    {
        private readonly ILogger logger;
        private I2cDevice pca9685;

        // Registers/etc.
        private const byte MODE1 = 0x00;
        private const byte MODE2 = 0x01;
        private const byte SUBADR1 = 0x02;
        private const byte SUBADR2 = 0x03;
        private const byte SUBADR3 = 0x04;
        private const byte PRESCALE = 0xFE;
        // Bits
        private const byte RESTART = 0x80;
        private const byte SLEEP = 0x10;
        private const byte ALLCALL = 0x01;
        private const byte INVRT = 0x10;
        private const byte OUTDRV = 0x04;
        private const byte LED0_ON_L = 0x06;
        private const byte LED0_ON_H = 0x07;
        private const byte LED0_OFF_L = 0x08;
        private const byte LED0_OFF_H = 0x09;
        private const byte ALL_LED_ON_L = 0xFA;
        private const byte ALL_LED_ON_H = 0xFB;
        private const byte ALL_LED_OFF_L = 0xFC;
        private const byte ALL_LED_OFF_H = 0xFD;

        private readonly int[] pwmChannelFrom;
        private readonly int[] pwmChannelTo;
        private readonly int address;
        private readonly int frequency;

        public PwmDriverPCA9685(ILogger logger, int address, int frequency)
        {
            this.logger = logger;
            this.address = address;
            this.frequency = frequency;
            pwmChannelFrom = new int[16];
            pwmChannelTo = new int[16];
        }


        public async Task Init()
        {
            var aqs = I2cDevice.GetDeviceSelector();                     /* Get a selector string that will return all I2C controllers on the system */
            var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */
            if (dis.Count == 0)
            {
                logger.WriteLn("No I2C controllers were found on the system");
                return;
            }

            var settings = new I2cConnectionSettings(address)
            {
                BusSpeed = I2cBusSpeed.FastMode
            };
            pca9685 = await I2cDevice.FromIdAsync(dis[0].Id, settings);    /* Create an I2cDevice with our selected bus controller and I2C settings */
            if (pca9685 == null)
            {
                logger.WriteLn(string.Format(
                    "Slave address {0} on I2C Controller {1} is currently in use by " +
                    "another application. Please ensure that no other applications are using I2C.",
                    settings.SlaveAddress,
                    dis[0].Id));
                return;
            }

            SetAllPwm(0, 0);
            I2Cwrite8(MODE2, OUTDRV);
            I2Cwrite8(MODE1, ALLCALL);
            await Task.Delay(5);

            var mode1 = I2CreadU8(MODE1);
            mode1 = (byte)(mode1 & ~SLEEP);       //  # wake up (reset sleep)
            I2Cwrite8(MODE1, mode1);
            await Task.Delay(5);
            await SetPwmFreq(frequency);
            logger.WriteLn("Driver initialized");
        }

        public void SetAllPwm(int on, int off)
        {
            I2Cwrite8(ALL_LED_ON_L, (byte)(on & 0xFF));
            I2Cwrite8(ALL_LED_ON_H, (byte)(on >> 8));
            I2Cwrite8(ALL_LED_OFF_L, (byte)(off & 0xFF));
            I2Cwrite8(ALL_LED_OFF_H, (byte)(off >> 8));

            for (var i = 0; i< 16; i++)
            {
                pwmChannelFrom[i] = on;
                pwmChannelTo[i] = off;
            }
        }

        private byte I2CreadU8(byte register)
        {
            var regAddrBuf = new byte[] { register }; /* Register address we want to read from                                         */
            var readBuf = new byte[1];
            pca9685.WriteRead(regAddrBuf, readBuf);
            return readBuf[0];
        }

        private void I2Cwrite8(int register, int value)
        {
            var buf = new byte[] { (byte)register, (byte)value };
            pca9685.Write(buf);
        }

        public async Task SetPwmFreq(int freq) {
            var prescaleval = 25000000.0;   //25MHz
            prescaleval /= 4096.0;           //12-bit
            prescaleval /= freq;
            prescaleval -= 1.0;
            //logger.WriteLn(String.Format("Setting PWM frequency to {0} Hz", freq));
            //logger.WriteLn(String.Format("Estimated pre-scale: {0}", prescaleval));

            var prescale = (int)Math.Floor(prescaleval + 0.5);
            //logger.WriteLn(String.Format("Final pre-scale: {0}", prescale));

            var oldmode = I2CreadU8(MODE1);
            var newmode = (oldmode & 0x7F) | 0x10;             // sleep
            I2Cwrite8(MODE1, newmode);                         // go to sleep
            I2Cwrite8(PRESCALE, prescale);
            I2Cwrite8(MODE1, oldmode);
            await Task.Delay(5);
            I2Cwrite8(MODE1, oldmode | 0x80);
        }

        public void SetPwm(int channel, int on, int off)
        {
            if (pwmChannelFrom[channel] != on)
            {
                pwmChannelFrom[channel] = on;
                I2Cwrite8(LED0_ON_L + 4 * channel, on & 0xFF);
                I2Cwrite8(LED0_ON_H + 4 * channel, on >> 8);
            }

            if (pwmChannelTo[channel] != off)
            {
                pwmChannelTo[channel] = off;
                I2Cwrite8(LED0_OFF_L + 4 * channel, off & 0xFF);
                I2Cwrite8(LED0_OFF_H + 4 * channel, off >> 8);
            }
        }

        public void SetPin(int pin, bool value)
        {
    		if (pin < 0 || pin > 15)
            {
                throw new ArgumentOutOfRangeException("PWM pin must be between 0 and 15 inclusive");
            }
            if (!value) {
                this.SetPwm(pin, 0, 4096);
            }
            if (value)
            {
                this.SetPwm(pin, 4096, 0);
            }
        }
    }
}

