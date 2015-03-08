// Main.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "arduino.h"
#include <exception>
#include <Wire.h>

int _tmain(int argc, _TCHAR* argv[])
{
    return RunArduinoSketch();
}

wchar_t buf[20];

TwoWire twoWire;

void setup()
{
    // TODO: Add your code here
	twoWire.begin();
	twoWire.getI2cHasBeenEnabled() ? Log(L"I2C is enabled") : Log(L"I2C is disabled");

	for (int pin = 2; pin <= 13; pin++)
	{
		pinMode(pin, OUTPUT);
	}
	for (int pin = 2; pin < 13; pin++)
	{
		if (pin == 2 || pin == 4 || pin == 12)
		{
			continue;
		}
		try {
			analogWrite(pin, 128);
		}
		catch (...)
		{
			swprintf(buf, 20, L"%d", pin);
			Log(buf);
		}

	}
}

// the loop routine runs over and over again forever:
void loop()
{
	
}