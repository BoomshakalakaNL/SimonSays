using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SimonSays.Classes
{
    public class Pushbutton : Hardware
    {
        public Pushbutton(int RPiPin, int ColorID, GpioPinDriveMode Drivemode) : base(RPiPin, ColorID, Drivemode)
        {
        }

        /// <summary>
        /// Deze functie controleerd of de PushButton is ingedrukt.
        /// </summary>
        /// <returns>True wanneer de PushButton is ingedrukt en False wanneer dit niet zo is.</returns>
        public bool IsPushbuttonPressed()
        {
            return (myGpioPin.Read() == GpioPinValue.Low);
        }
    }
}
