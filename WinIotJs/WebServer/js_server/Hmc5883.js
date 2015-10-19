//Import WinRT into Node.JS
var uwp = require("uwp");
//uwp.projectNamespace("Windows");

var HMC5883_ADDRESS_MAG = (0x3C >> 1);         // 0011110x
var HMC5883_REGISTER_MAG_CRB_REG_M = 0x01;
var HMC5883_REGISTER_MAG_MR_REG_M = 0x02;
var HMC5883_REGISTER_MAG_OUT_X_H_M = 0x03;

var magI2CDevice;

var isInitialized;

function init(callback) {
    if (isInitialized) {
        return;
    }
    isInitialized = true;
    
    var magSettings = new Windows.Devices.I2c.I2cConnectionSettings(HMC5883_ADDRESS_MAG);
    magSettings.BusSpeed = 0;  // Fast mode
    
    //Find the device: same code in other project, except in JS instead of C#
    var aqs = Windows.Devices.I2c.I2cDevice.getDeviceSelector("I2C1");
    
    Windows.Devices.Enumeration.DeviceInformation.findAllAsync(aqs, null).done(function (dis) {
        Windows.Devices.I2c.I2cDevice.fromIdAsync(dis[0].id, magSettings).done(function (device) {
            magI2CDevice = device;
            initMagnetometer();
            if (callback) {
                callback();
            }
        });
    });
}

function initMagnetometer() {
    // Enable the magnetometer
    writeTo(magI2CDevice, HMC5883_REGISTER_MAG_MR_REG_M, 0x00);

    // Set the gain to +/-1.3 (max sensitivity)  
    writeTo(magI2CDevice, HMC5883_REGISTER_MAG_CRB_REG_M, 0x20);
}

function writeTo(i2cDevice, regAddress, regValue) {
    var tempData = new Array(2);
    tempData[0] = regAddress;
    tempData[1] = regValue;
    i2cDevice.write(tempData);
}

function readMag() {
    // First set the register start address for X
    var addressBuf = new Array(1);
    addressBuf[0] = HMC5883_REGISTER_MAG_OUT_X_H_M;
    
    // Read 6 data bytes
    var buffer = new Array(6);
    
    magI2CDevice.writeRead(addressBuf, buffer);
    
    //Combine bytes into integers
    // MSB first
    mag_x = toInteger(buffer[0], buffer[1]);
    mag_y = toInteger(buffer[2], buffer[3]);
    mag_z = toInteger(buffer[4], buffer[5]);
    
    // map axes
    return {
        x: mag_x,
        y: mag_y,
        z: mag_z
    }
}

function toInteger(msb, lsb) {
    var result = msb << 8 | lsb;
    if (result > 32767) {
        result = result ^ 0xFFFF;
        result = result + 1;
        result = -result;
    }
    return result;
}


function readSensors() {
    if (!magI2CDevice) {
        return;
    }
    if (!isInitialized) {
        init();
        return;
    }
    
    var data = readMag();
    return data;
}

function readSensorString() {
    if (!magI2CDevice) {
        return;
    }
    if (!isInitialized) {
        init();
        return;
    }
    
    var data = readMag();
    return "MX:" + data.x + " MY:" + data.y + " MZ:" + data.z;
}


exports.init = init;
exports.readSensors = readSensors;
exports.readSensorString = readSensorString;