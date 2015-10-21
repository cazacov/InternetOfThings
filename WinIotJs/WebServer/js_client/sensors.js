define(function () {
    var that;
    
    function Sensors(debugWriter, sprintf) {
        that = this;
        this.debugWriter = debugWriter;
        this.sprintf = sprintf;
        
        if (!(this instanceof Sensors)) {
            throw new TypeError("Sensors constructor cannot be called as a function.");
        }
    }
    
    Sensors.prototype = {
        constructor: Sensors,
        normalizeAndMapAxes: function (data) {
            var result = {
                acc: [],
                mag: []
            };
            var cx = -118;
            var cy = -153;
            var cz = -86.5;
            var sx = 0.87;
            var sy = 1;
            var sz = 0.87;
            
            
            result.acc.x = data.acc.y;
            result.acc.y = data.acc.x;
            result.acc.z = data.acc.z;
            
            result.mag.x = (data.mag.x - cx) * sx;
            result.mag.y = (data.mag.z - cz) * sz;
            result.mag.z = (data.mag.y - cy) * sy;
            
            return result;
        },
        
        showData: function (data) {
            that.debugWriter.show("AccX", data.acc.x);
            that.debugWriter.show("AccY", data.acc.y);
            that.debugWriter.show("AccZ", data.acc.z);
            that.debugWriter.show("MagX", data.mag.x);
            that.debugWriter.show("MagY", data.mag.y);
            that.debugWriter.show("MagZ", data.mag.z);
        }
    }
    
    function showCalibrationData(data) {
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
        $('#nx').text(nx);
        $('#ny').text(ny);
        $('#nz').text(nz);
    }

    
    return Sensors;
});