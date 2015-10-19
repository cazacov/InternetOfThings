console.log("Starting web server...");
var express = require('express');
var imu3000 = require('./js_server/imu3000');
var hmc5883 = require('./js_server/hmc5883');
var leds = require('./js_server/leds');

imu3000.init();
hmc5883.init();
leds.init();

var server = express();
server.use(express.static(__dirname));

server.get('/sensors', function (req, res) {
    var imuState = imu3000.readSensors();
    var magState = hmc5883.readSensors();
    res.send({
        acc: imuState.acc,
        mag: magState
    });
});

var port = 8888;
server.listen(port, function () {
	console.log('Server listening at %s', port);
});