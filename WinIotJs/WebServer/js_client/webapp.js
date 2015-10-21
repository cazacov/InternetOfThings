var sprintf = require(['sprintf']).sprintf;

require(['debugwriter', 'sensors', 'controller3d'], function (DebugWriter, Sensors, Controller3D) {
    
    var debugWriter = new DebugWriter($(document.getElementById('debugBox')));
    var sensors = new Sensors(debugWriter, sprintf);
    controller = new Controller3D(debugWriter, sprintf);
    controller.start();
    

    $().ready(function () {
        setTimeout(getSensors(), 200);
    });
    
 
    function getSensors() {
        $.get("/sensors?id=" + (new Date()).getTime(), function (rawData, status) {
            var data = sensors.normalizeAndMapAxes(rawData);
            sensors.showData(data);
            controller.useSensorData(data);
        })
        .fail(function (err) {
            $('#debugElem').text("AJAX ", "Error: ");
            console.log(err);
        })
        .always(function () {
            setTimeout(getSensors, 5);
        });
    }
    
});

