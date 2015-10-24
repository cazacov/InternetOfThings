//Import WinRT into Node.JS
var uwp = require("uwp");
uwp.projectNamespace("Windows");

var express = require('express');
var router = express.Router();


var gpioController = Windows.Devices.Gpio.GpioController.getDefault();

var pinRedNr = 5;
var pinRed = gpioController.openPin(pinRedNr);
pinRed.setDriveMode(Windows.Devices.Gpio.GpioPinDriveMode.output);

var pinYellowNr = 6;
var pinYellow = gpioController.openPin(pinYellowNr);
pinYellow.setDriveMode(Windows.Devices.Gpio.GpioPinDriveMode.output);

var pinGreenNr = 13;
var pinGreen = gpioController.openPin(pinGreenNr);
pinGreen.setDriveMode(Windows.Devices.Gpio.GpioPinDriveMode.output);

var high = Windows.Devices.Gpio.GpioPinValue.high;
var low = Windows.Devices.Gpio.GpioPinValue.low;

var state = {
    red: false,
    yellow: false,
    green: false,
}

pinRed.write(low);
pinYellow.write(low);
pinGreen.write(low);



/* GET home page. */
router.get('/', function (req, res) {
    res.send(state);
});

/* GET home page. */
router.post('/reset', function (req, res) {
    pinRed.write(low);
    pinYellow.write(low);
    pinGreen.write(low);
    
    state = {
        red: false,
        yellow: false,
        green: false,
    };
    res.send(state);
});

var pins = { red: pinRed, yellow: pinYellow, green: pinGreen };
var states = { off: { signal: low, state: false }, on: { signal: high, state: true } };

router.post('/:color/:state', function (req, res) {
    var color = req.params.color
    var pin = pins[color]
    var requiredState = states[req.params.state]
    if (!pin || !requiredState) {
        res.send(404)
        return
    }

    pin.write(requiredState.signal)
    state[color] = requiredState.state;
    res.send(state);
});

module.exports = router;