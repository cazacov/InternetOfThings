var windowSize = 3;

var filterValues = [];
var valIndex = 0;

function init(vectorLen) {
    for (var i = 0; i < windowSize; i++) {
        filterValues[i] = Array.apply(null, Array(vectorLen)).map(function (x, i) { return 0; })
    }
}

function push(valueVector)
{
    filterValues[valIndex] = valueVector;
    valIndex = (valIndex + 1) % windowSize;
}

function read()
{
    var vectorLen = filterValues[0].length;
    var result = Array.apply(null, Array(vectorLen)).map(function (x, i) { return 0; })

    for (var i = 0; i < windowSize; i++) {
        for (var j = 0; j < vectorLen; j++) {
            result[j] += filterValues[i][j];
        }
    }

    for (var j = 0; j < vectorLen; j++) {
        result[j] /= windowSize;
    }

    return result;
}

exports.init = init;
exports.read = read;
exports.push = push;
