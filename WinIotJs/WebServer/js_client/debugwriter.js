define(function () {
    var me;

    function DebugWriter(targetDiv) {
        me = this;
        if (!(this instanceof DebugWriter)) {
            throw new TypeError("DebugWriter constructor cannot be called as a function.");
        }
        this.targetDiv = targetDiv;
        this.targetDiv.empty();
    }

    DebugWriter.prototype = {
        constructor: DebugWriter,

        accelerometer: function (message) {
            showText('grav.', message);
        },

        compass: function (message) {
            showText('mag.', message);
        },

        show: function (caption, message) {
            showText(caption, message);
        }
    };

    function showText(caption, message) {
        var element = $(document.getElementById(caption));

        if (!element.length)
        {
            var row = $('<tr></tr>');
            var label = $('<td></td>');
            element = $('<td id="' + caption + '"></td>');

            me.targetDiv.append(row);
            row.append(label);
            row.append(element);

            label.text(caption);
        }
        element.text(message);
    }

    return DebugWriter;
});
