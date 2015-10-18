//Import WinRT into Node.JS
var uwp = require("uwp");
uwp.projectNamespace("Windows");

var i2cDevice;

var settings = I2cConnectionSettings

//Find the device: same code in other project, except in JS instead of C#
var aqs = Windows.Devices.I2c.I2cDevice.getDeviceSelector("I2C1");

Windows.Devices.Enumeration.DeviceInformation.findAllAsync(aqs, null).done(function (dis) {
	Windows.Devices.I2c.I2cDevice.fromIdAsync(dis[0].id, new Windows.Devices.I2c.I2cConnectionSettings(0x40)).done(function (device) {
		i2cDevice = device;
	});
});

var isInitialized;

var GYRO = 0x68;      	   		// gyro I2C address
var REG_GYRO_X = 0x1D;   		// IMU-3000 Register address for GYRO_XOUT_H
var ACCEL = 0x53;        		// Accel I2c Address
var ADXL345_POWER_CTL = 0x2D;

var HMC5883_ADDRESS_MAG = (0x3C >> 1);         // 0011110x
var HMC5883_REGISTER_MAG_CRB_REG_M = 0x01;
var HMC5883_REGISTER_MAG_MR_REG_M = 0x02;
var HMC5883_REGISTER_MAG_OUT_X_H_M = 0x03;


function init() {
	if (isInitialized)
	{
		return;
	}
	isInitialized = true;
	
	initAccelerometer();
  	initMagnetometer();
}

function initAccelerometer()
{
	// Set Gyro settings
  // Sample Rate 1kHz, Filter Bandwidth 42Hz, Gyro Range 500 d/s 
  writeTo(GYRO, 0x16, 0x0B);       
  //set accel register data address
  writeTo(GYRO, 0x18, 0x32);     
  // set accel i2c slave address
  writeTo(GYRO, 0x14, ACCEL);     
    
  // Set passthrough mode to Accel so we can turn it on
  writeTo(GYRO, 0x3D, 0x08);     
  // set accel power control to 'measure'
  writeTo(ACCEL, ADXL345_POWER_CTL, 8);     
  //cancel pass through to accel, gyro will now read accel for us   
  writeTo(GYRO, 0x3D, 0x28);   
}

function initMagnetometer()
{
  // Enable the magnetometer
  writeTo(HMC5883_ADDRESS_MAG, HMC5883_REGISTER_MAG_MR_REG_M, 0x00);
  
  // Set the gain to +/-1.3 (max sensitivity)  
  writeTo(HMC5883_ADDRESS_MAG, HMC5883_REGISTER_MAG_CRB_REG_M, 0x20);
}  

function writeTo(deviceAddress, regAddress, regValue)
{

}

function writeSensor(regAddr, data) {
  Wire.beginTransmission(MPU9150_I2C_ADDRESS);
  Wire.write(addr);
  Wire.write(data);
  Wire.endTransmission(true);

  return 1;
}

function readSensorString() {
	if (!i2cDevice) {
		return;
	}
	if (!isInitialized) {
		init();
	}
}


exports.init = init;
exports.readSensors = readSensors;

