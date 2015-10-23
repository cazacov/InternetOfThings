console.log("Starting web server...");
var express = require('express');
var imu3000 = require('./js_server/imu3000');
var hmc5883 = require('./js_server/hmc5883');
var filter = require('./js_server/meanfilter');
var lowPassFilter = require('./js_server/lowpassfilter');
var leds = require('./js_server/leds');
filter.init(6);
filter.push([0, 0, 1, 0, 1, -1]); // some reasonable init values
lowPassFilter.init(6);

imu3000.init(function () {
    hmc5883.init(function () {
        readSensors();
    });
});
leds.init();

var server = express();
server.use(express.static(__dirname));

server.get('/sensors', function (req, res) {
    var v = filter.read();
    lowPassFilter.push(v);
    v = lowPassFilter.read();
    res.send({
        acc: {
            x: v[0],
            y: v[1],
            z: v[2]
        },
        mag: {
            x: v[3],
            y: v[4],
            z: v[5]
        }
    });
});

var port = 8888;
server.listen(port, function () {
    console.log('Server listening at %s', port);
    //setTimeout(readSensors, 200);
});

function readSensors()
{
    var imuState = imu3000.readSensors();
    var magState = hmc5883.readSensors();
    filter.push([imuState.acc.x, imuState.acc.y, imuState.acc.z, magState.x, magState.y, magState.z]);
    setTimeout(readSensors, 10);
}