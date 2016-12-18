using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SimonSays.Classes
{
    public class Led : Hardware
    {
        public Led(int RPiPin, int ColorID, GpioPinDriveMode Drivemode) : base (RPiPin, ColorID, Drivemode)
        {
        }

        /// <summary>
        /// Deze functie laat het lampje aan gaan.
        /// </summary>
        public void zetLedAan()
        {
            myGpioPin.Write(GpioPinValue.Low);
        }

        /// <summary>
        /// Deze functie laad het led lampje uit gaan.
        /// </summary>
        public void zetLedUit()
        {
            myGpioPin.Write(GpioPinValue.High);
        }

        /// <summary>
        /// Deze functie laat het led lampje knipperen.
        /// </summary>
        /// <param name="aantal">Hoevaak moet het lampje knipperen</param>
        /// <param name="hoelangAan">Hoelang moet het lampje per knipper aan zijn</param>
        /// <param name="hoelangUit">Hoelang moet het lampje per knipper uit zijn</param>
        /// <param name="delay">Hoelang moet er gewacht worden als het lampje geknippert heeft</param>
        public void knipperLed(int aantal, int hoelangAan, int hoelangUit, int delay)
        {
            for (int i = 0; i < aantal; i++)
            {
                zetLedAan();
                Task.Delay(hoelangAan).Wait();
                zetLedUit();
                Task.Delay(hoelangUit).Wait();
            }
            Task.Delay(delay).Wait();
        }
    }
}
