// Main.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <windows.h>
#include <stdio.h>
#include <VersionHelpers.h>

int RS = 4;
int ENABLE = 5;
int D0 = 6;
int D1 = 7;
int D2 = 8;
int D3 = 9;
LiquidCrystal lcd = LiquidCrystal(RS, ENABLE, D0, D1, D2, D3); // define our LCD and which pins to use

int _tmain(int argc, _TCHAR* argv[])
{
	return RunArduinoSketch();
}

void setup()
{
	Log(L"LCD Sample\n");

	lcd.begin(16, 2); // need to specify how many columns and rows are in the LCD unit (it calls clear at the end of begin)

	lcd.setCursor(0, 0);
	lcd.print("Hello Habrahabr!");
}

void loop()
{
}
