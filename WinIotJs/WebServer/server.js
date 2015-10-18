console.log("Starting web server...");
var express = require('express');
var imu3000 = require('./js_server/imu3000');

imu3000.init();

var server = express();
server.use(express.static(__dirname));

server.get('/sensors', function (req, res) {
    var sensorsState = imu3000.readSensorString();
	res.send(sensorsState);
});

var port = 8888;
server.listen(port, function () {
	console.log('Server listening at %s', port);
});