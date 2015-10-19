var sprintf = require(['sprintf']).sprintf;

require([], function () {
    $().ready(function () {
        setTimeout(getSensors(), 200);
    });
    
    function getSensors() {
        $.get("/sensors", function (data, status) {
            console.log(data);
            $('#ax').text(data.acc.x);
            $('#ay').text(data.acc.y);
            $('#az').text(data.acc.z);
            $('#mx').text(data.mag.x);
            $('#my').text(data.mag.y);
            $('#mz').text(data.mag.z);
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