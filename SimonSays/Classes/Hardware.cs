using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SimonSays.Classes
{
    public abstract class Hardware
    {
        private GpioPin _gpioPin;
        private GpioController _gpio;
        private int ColorID;

        public Hardware(int RPiPin, int ColorID, GpioPinDriveMode DriveMode)
        {
            this.ColorID = ColorID;
            _gpio = GpioController.GetDefault();
            _gpioPin = _gpio.OpenPin(RPiPin);
            _gpioPin.SetDriveMode(DriveMode);
            if(DriveMode == GpioPinDriveMode.Output)
            {
                _gpioPin.Write(GpioPinValue.High);
            }
        }

        //Maakt colorID leesbaar voor andere classen
        public int myColorID
        {
            get { return ColorID; }
        }
        //Maakt _gpioPin leesbaar voor andere classen
        public GpioPin myGpioPin
        {
            get { return _gpioPin; }
        }
    }
}
