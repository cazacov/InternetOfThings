var sprintf = require(['sprintf']).sprintf;

require([], function () {
    $().ready(function () {
        setTimeout(getSensors(), 200);
    });
    
    function getSensors() {
        $.get("/sensors", function (data, status) {
            console.log(data);
            $('#debugElem').text(data);
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