using System.Threading.Tasks;

namespace Drivers
{
    public interface IPwmDriver
    {
        Task Init();
        void SetAllPwm(int on, int off);
        Task SetPwmFreq(int freq);
        void SetPwm(int channel, int on, int off);
        void SetPin(int pin, bool value);
    }
}