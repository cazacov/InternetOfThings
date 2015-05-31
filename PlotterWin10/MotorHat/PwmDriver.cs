using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.UI.Xaml;

namespace MotorHat
{
    public class PwmDriver
    {
        private ILogger logger;
        private I2cDevice I2CAccel;

        private const int PWM_I2C_ADDR = 0x60;
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

        private DispatcherTimer timer;

        public PwmDriver(ILogger logger)
        {
            this.logger = logger;
        }


        public async Task Init()
        {
            string aqs = I2cDevice.GetDeviceSelector();                     /* Get a selector string that will return all I2C controllers on the system */
            var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */
            if (dis.Count == 0)
            {
                logger.WriteLn("No I2C controllers were found on the system");
                return;
            }

            var settings = new I2cConnectionSettings(PWM_I2C_ADDR);
            settings.BusSpeed = I2cBusSpeed.FastMode;
            I2CAccel = await I2cDevice.FromIdAsync(dis[0].Id, settings);    /* Create an I2cDevice with our selected bus controller and I2C settings */
            if (I2CAccel == null)
            {
                logger.WriteLn(string.Format(
                    "Slave address {0} on I2C Controller {1} is currently in use by " +
                    "another application. Please ensure that no other applications are using I2C.",
                    settings.SlaveAddress,
                    dis[0].Id));
                return;
            }

            setAllPWM(0, 0);
            i2cwrite8(MODE2, OUTDRV);
            i2cwrite8(MODE1, ALLCALL);
            await Task.Delay(5);

            var mode1 = i2creadU8(MODE1);
            mode1 = (byte)(mode1 & ~SLEEP);       //  # wake up (reset sleep)
            i2cwrite8(MODE1, mode1);
            await Task.Delay(5);
            logger.WriteLn("Driver initialized");

            await setPWMFreq(50);

            this.SetPWM(0, 0, 307);
        }

        private void setAllPWM(int on, int off)
        {
            i2cwrite8(ALL_LED_ON_L, (byte)(on & 0xFF));
            i2cwrite8(ALL_LED_ON_H, (byte)(on >> 8));
            i2cwrite8(ALL_LED_OFF_L, (byte)(off & 0xFF));
            i2cwrite8(ALL_LED_OFF_H, (byte)(off >> 8));
        }

        private byte i2creadU8(byte register)
        {
            byte[] regAddrBuf = new byte[] { register }; /* Register address we want to read from                                         */
            byte[] readBuf = new byte[1];
            I2CAccel.WriteRead(regAddrBuf, readBuf);
            return readBuf[0];
        }

        private void i2cwrite8(int register, int value)
        {
            byte[] buf = new byte[] { (byte)register, (byte)value };
            I2CAccel.Write(buf);
        }

        private async Task setPWMFreq(int freq) {
            var prescaleval = 25000000.0;   //25MHz
            prescaleval /= 4096.0;           //12-bit
            prescaleval /= freq;
            prescaleval -= 1.0;
            logger.WriteLn(String.Format("Setting PWM frequency to {0} Hz", freq));
            logger.WriteLn(String.Format("Estimated pre-scale: {0}", prescaleval));

            var prescale = (int)Math.Floor(prescaleval + 0.5);
            logger.WriteLn(String.Format("Final pre-scale: {0}", prescale));

            var oldmode = i2creadU8(MODE1);
            var newmode = (oldmode & 0x7F) | 0x10;             // sleep
            i2cwrite8(MODE1, newmode);                         // go to sleep
            i2cwrite8(PRESCALE, prescale);
            i2cwrite8(MODE1, oldmode);
            await Task.Delay(5);
            i2cwrite8(MODE1, oldmode | 0x80);
        }

        public void SetPWM(int channel, int on, int off)
        {
            i2cwrite8(LED0_ON_L + 4 * channel, on & 0xFF);
            i2cwrite8(LED0_ON_H + 4 * channel, on >> 8);
            i2cwrite8(LED0_OFF_L + 4 * channel, off & 0xFF);
            i2cwrite8(LED0_OFF_H + 4 * channel, off >> 8);
        }
    }
}

