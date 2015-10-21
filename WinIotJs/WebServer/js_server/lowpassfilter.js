var oldValues = [];

var filterAlpha = 0.1;


function init(vectorLen) {
    oldValues = Array.apply(null, Array(vectorLen)).map(function (x, i) { return 0; });
}

function push(valueVector) {
    oldValues = oldValues.map(function (curValue, index) {
        return curValue + filterAlpha * (valueVector[index] - curValue);
    });
}

function read() {
    return oldValues;
}

exports.init = init;
exports.read = read;
exports.push = push;
