/* 
	Editor: http://www.visualmicro.com
	        visual micro and the arduino ide ignore this code during compilation. this code is automatically maintained by visualmicro, manual changes to this file will be overwritten
	        the contents of the Visual Micro sketch sub folder can be deleted prior to publishing a project
	        all non-arduino files created by visual micro and all visual studio project or solution files can be freely deleted and are not required to compile a sketch (do not delete your own code!).
	        note: debugger breakpoints are stored in '.sln' or '.asln' files, knowledge of last uploaded breakpoints is stored in the upload.vmps.xml file. Both files are required to continue a previous debug session without needing to compile and upload again
	
	Hardware: Arduino Due (Programming Port), Platform=sam, Package=arduino
*/

#ifndef _VSARDUINO_H_
#define _VSARDUINO_H_
#define __SAM3X8E__
#define USB_VID 0x2341
#define USB_PID 0x003e
#define USBCON
#define USB_MANUFACTURER "\"Unknown\""
#define USB_PRODUCT "\"Arduino Due\""
#define ARDUINO 156
#define ARDUINO_MAIN
#define printf iprintf
#define __SAM__
#define __sam__
#define F_CPU 84000000L
#define __cplusplus
#define __inline__
#define __asm__(x)
#define __extension__
#define __ATTR_PURE__
#define __ATTR_CONST__
#define __inline__
#define __asm__ 
#define __volatile__

#define __ICCARM__
#define __ASM
#define __INLINE
#define __GNUC__ 0
#define __ICCARM__
#define __ARMCC_VERSION 400678
#define __attribute__(noinline)

#define prog_void
#define PGM_VOID_P int
            
typedef unsigned char byte;
extern "C" void __cxa_pure_virtual() {;}




void readAndReportData(byte address, int theRegister, byte numBytes);
void outputPort(byte portNumber, byte portValue, byte forceSend);
void checkDigitalInputs(void);
void setPinModeCallback(byte pin, int mode);
void analogWriteCallback(byte pin, int value);
void digitalWriteCallback(byte port, int value);
void reportAnalogCallback(byte analogPin, int value);
void reportDigitalCallback(byte port, int value);
void sysexCallback(byte command, byte argc, byte *argv);
void enableI2CPins();
void disableI2CPins();
void systemResetCallback();
//
//

#include "C:\programme\Arduino\hardware\arduino\sam\cores\arduino\arduino.h"
#include "C:\programme\Arduino\hardware\arduino\sam\variants\arduino_due_x\pins_arduino.h" 
#include "C:\programme\Arduino\hardware\arduino\sam\variants\arduino_due_x\variant.h" 
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\BleFirmataSketch.ino"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\BLEFirmata.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\BLEFirmata.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\Boards.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\RBL_nRF8001.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\RBL_nRF8001.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\RBL_services.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_cmds.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_evts.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_protocol_defines.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_queue.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_queue.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_setup.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\aci_setup.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\acilib.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\acilib.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\acilib_defs.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\acilib_if.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\acilib_types.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\ble_assert.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\hal_aci_tl.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\hal_aci_tl.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\hal_platform.h"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\lib_aci.cpp"
#include "E:\Git\InternetOfThings\BluetoothCommunication\BleFirmataSketch\lib_aci.h"
#endif
