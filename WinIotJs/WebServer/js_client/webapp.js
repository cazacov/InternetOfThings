var sprintf = require(['sprintf']).sprintf;

require([], function () {
    $().ready(function () {
        setTimeout(getSensors(), 200);
    });
    
    var minx = 1000;
    var miny = 1000;
    var minz = 1000;
    var maxx = -1000;
    var maxy = -1000;
    var maxz = -1000;
    
    function getSensors() {
        $.get("/sensors", function (data, status) {
            console.log(data);
            $('#ax').text(data.acc.x);
            $('#ay').text(data.acc.y);
            $('#az').text(data.acc.z);
            $('#mx').text(data.mag.x);
            $('#my').text(data.mag.y);
            $('#mz').text(data.mag.z);

            minx = Math.min(minx, data.mag.x);
            miny = Math.min(miny, data.mag.y);
            minz = Math.min(minz, data.mag.z);
            maxx = Math.max(maxx, data.mag.x);
            maxy = Math.max(maxy, data.mag.y);
            maxz = Math.max(maxz, data.mag.z);
            
            var dx = maxx - minx;
            var dy = maxy - miny;
            var dz = maxz - minz;
            
            var minscale = Math.min(dx, dy, dz);
            var cx = minx + dx / 2;
            var cy = miny + dy / 2;
            var cz = minz + dz / 2;
            
            var sx = minscale / dx;
            var sy = minscale / dy;
            var sz = minscale / dz;
            
            var nx = (data.mag.x - cx) * sx;
            var ny = (data.mag.y - cy) * sy;
            var nz = (data.mag.z - cz) * sz;

            $('#minx').text(minx);
            $('#miny').text(miny);
            $('#minz').text(minz);
            $('#maxx').text(maxx);
            $('#maxy').text(maxy);
            $('#maxz').text(maxz);
            $('#cx').text(cx);
            $('#cy').text(cy);
            $('#cz').text(cz);
            $('#sx').text(sx);
            $('#sy').text(sy);
            $('#sz').text(sz);
            $('#nx').text(nx)           ;
            $('#ny').text(ny);
            $('#nz').text(nz);
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