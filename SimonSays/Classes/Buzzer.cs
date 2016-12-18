using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SimonSays.Classes
{
    public class Buzzer : Hardware
    {
        private bool speakerOn;

        public Buzzer(bool SpeakerOn,int RPiPin, int ColorID, GpioPinDriveMode Drivemode) : base(RPiPin, ColorID, Drivemode)
        {
            this.speakerOn = SpeakerOn;
            myGpioPin.Write(GpioPinValue.Low);
        }

        /// <summary>
        /// Deze functie laat het speakertje aan gaan.
        /// </summary>
        public void zetBuzzerAan()
        {
            if (speakerOn)
            {
                myGpioPin.Write(GpioPinValue.High);
            }
        }

        /// <summary>
        /// Deze functie laad het speakertje uit gaan.
        /// </summary>
        public void zetBuzzerUit()
        {
            if (speakerOn)
            {
                myGpioPin.Write(GpioPinValue.Low);
            }
        }

        /// <summary>
        /// Deze functie laat het speakertje piepen.
        /// </summary>
        /// <param name="aantal">Hoevaak moet het speakertje knipperen</param>
        /// <param name="hoelangAan">Hoelang moet het speakertje per piep aan zijn</param>
        /// <param name="hoelangUit">Hoelang moet het speakertje per piep uit zijn</param>
        /// <param name="delay">Hoelang moet er gewacht worden als het speakertje gepiept heeft</param>
        public void piepBuzzer(int aantal, int hoelangAan, int hoelangUit, int delay)
        {
            if (speakerOn)
            {
                for (int i = 0; i < aantal; i++)
                {
                    zetBuzzerAan();
                    Task.Delay(hoelangAan).Wait();
                    zetBuzzerUit();
                    Task.Delay(hoelangUit).Wait();
                }
                Task.Delay(delay).Wait();
            }
        }
    }
}
