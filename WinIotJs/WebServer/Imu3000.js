//Import WinRT into Node.JS
var uwp = require("uwp");
uwp.projectNamespace("Windows");

var GYRO = 0x68;              // gyro I2C address
var REG_GYRO_X = 0x1D;        // IMU-3000 Register address for GYRO_XOUT_H
var ACCEL = 0x53;             // Accel I2c Address
var ADXL345_POWER_CTL = 0x2D;

var accI2CDevice;
var gyroI2CDevice;

var isInitialized;

function init() {
    if (isInitialized) {
        return;
    }
    isInitialized = true;
    
    var accSettings = new Windows.Devices.I2c.I2cConnectionSettings(ACCEL);
    accSettings.BusSpeed = 0;  // Fast mode
    
    var gyroSettings = new Windows.Devices.I2c.I2cConnectionSettings(GYRO);
    gyroSettings.BusSpeed = 0;  // Fast mode
    
    //Find the device: same code in other project, except in JS instead of C#
    var aqs = Windows.Devices.I2c.I2cDevice.getDeviceSelector("I2C1");
    
    Windows.Devices.Enumeration.DeviceInformation.findAllAsync(aqs, null).done(function (dis) {
        Windows.Devices.I2c.I2cDevice.fromIdAsync(dis[0].id, accSettings).done(function (device) {
            accI2CDevice = device;
            if (gyroI2CDevice) {
                initImu3000();
            }
        });
        
        Windows.Devices.I2c.I2cDevice.fromIdAsync(dis[0].id, gyroSettings).done(function (device) {
            gyroI2CDevice = device;
            if (accI2CDevice) {
                initImu3000();
            }
        });
    });
}

function initImu3000() {
    // Set Gyro settings
    // Sample Rate 1kHz, Filter Bandwidth 42Hz, Gyro Range 500 d/s 
    writeTo(gyroI2CDevice, 0x16, 0x0B);
    //set accel register data address
    writeTo(gyroI2CDevice, 0x18, 0x32);
    // set accel i2c slave address
    writeTo(gyroI2CDevice, 0x14, ACCEL);
    
    // Set passthrough mode to Accel so we can turn it on
    writeTo(gyroI2CDevice, 0x3D, 0x08);
    // set accel power control to 'measure'
    writeTo(accI2CDevice, ADXL345_POWER_CTL, 8);
    //cancel pass through to accel, gyro will now read accel for us   
    writeTo(gyroI2CDevice, 0x3D, 0x28);
}

function writeTo(i2cDevice, regAddress, regValue) {
    var tempData = new Array(2);
    tempData[0] = regAddress;
    tempData[1] = regValue;
    i2cDevice.write(tempData);
}

function readAccGyro() {
    // First set the register start address for X on Gyro  
    var addressBuf = new Array(1);
    addressBuf[0] = REG_GYRO_X; //Register Address GYRO_XOUT_H
    
    // Read 12 data bytes
    var buffer = new Array(12);
    
    gyroI2CDevice.writeRead(addressBuf, buffer);
    
    //Combine bytes into integers
    // Gyro format is MSB first
    gyro_x = toInteger(buffer[0], buffer[1]);
    gyro_y = toInteger(buffer[2], buffer[3]);
    gyro_z = toInteger(buffer[4], buffer[5]);
    
    // Accel is LSB first. Also because of orientation of chips
    // accel y output is in same orientation as gyro x
    // and accel x is gyro -y
    accel_y = toInteger(buffer[7], buffer[6]);
    accel_x = toInteger(buffer[9], buffer[8]);
    accel_z = toInteger(buffer[11], buffer[10]);
    
    // map axes
    return {
        acc: {
            x: -accel_x,
            y: accel_z,
            z: -accel_y
        },
        gyro: {
            dx: gyro_y,
            dy: gyro_z,
            dz: -gyro_x
        }
    }
}

function toInteger(msb, lsb)
{
    var result = msb << 8 | lsb;
    if (result > 32767) {
        result = result ^ 0xFFFF;
        result = result + 1;
        result = -result;
    }
    return result;
}


function readSensors() {
    if (!(accI2CDevice && gyroI2CDevice)) {
        return;
    }
    if (!isInitialized) {
        init();
        return;
    }
    
    var accData = readAccGyro();
    return accData;
}

function readSensorString() {
    if (!(accI2CDevice && gyroI2CDevice)) {
        return;
    }
    if (!isInitialized) {
        init();
        return;
    }
    
    var accData = readAccGyro();
    return "X:" + accData.acc.x + " Y:" + accData.acc.y + " Z:" + accData.acc.z + " dX:" + accData.gyro.dx + " dY:" + accData.gyro.dy + " dZ:" + accData.gyro.dz;
}


exports.init = init;
exports.readSensors = readSensors;
exports.readSensorString = readSensorString;

