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

var pins = { red: pinRed, yellow: pinYellow, green: pinGreen };

var state = {
    red: false,
    yellow: false,
    green: false,
}

pinRed.write(low);
pinYellow.write(low);
pinGreen.write(low);


router.get('/:color/:state', function (req, res) {
    
    // Turn on the LED on GPIO pin 5     
    
    res.send(state);
});


/* GET home page. */
router.get('/', function (req, res) {
    
    // Turn on the LED on GPIO pin 5     

    res.send(state);
});

/* GET home page. */
router.get('/reset', function (req, res) {
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



//router.get('/red/on', function (req, res) {
//    pinRed.write(high);
//    state.red = true;
//    res.send(state);
//});

//router.get('/red/off', function (req, res) {
//    pinRed.write(low);
//    state.red = false;
//    res.send(state);
//});

//router.get('/green/on', function (req, res) {
//    pinGreen.write(high);
//    state.green = true;
//    res.send(state);
//});

//router.get('/green/off', function (req, res) {
//    pinGreen.write(low);
//    state.green = false;
//    res.send(state);
//});

//router.get('/yellow/on', function (req, res) {
//    pinYellow.write(high);
//    state.yellow = true;
//    res.send(state);
//});

//router.get('/yellow/off', function (req, res) {
//    pinYellow.write(low);
//    state.yellow = false;
//    res.send(state);
//});



module.exports = router;