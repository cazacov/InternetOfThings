//Import WinRT into Node.JS
var uwp = require("uwp");
//uwp.projectNamespace("Windows");


var pin5;

function init()
{
    // Turn on the LED on GPIO pin 5     
    var gpioController = Windows.Devices.Gpio.GpioController.getDefault();
    pin5 = gpioController.openPin(5);
    pin5.setDriveMode(Windows.Devices.Gpio.GpioPinDriveMode.output)
    pin5.write(Windows.Devices.Gpio.GpioPinValue.low);   
}

exports.init = init;
