#include <Arduino.h>
#include <Wire.h>

void setup()
{
	pinMode(2, OUTPUT);
	pinMode(3, OUTPUT);
	digitalWrite(2, HIGH);
	digitalWrite(3, HIGH);
	Serial.begin(57600);
	Wire.begin();
	delay(10);
	Serial.println("Initialized");
	delay(100);
}

void loop()
{
	// Wake up device
	digitalWrite(2, LOW);
	Wire.beginTransmission(0);
	Wire.endTransmission();
	delay(3);
	digitalWrite(2, HIGH);

	uint8_t readCommand[] = { 0x03, 0x07, 0x02, 0x00, 0x00, 0x00, 0x1E, 0x2D };
	Wire.beginTransmission(100);
	Wire.write(readCommand, 8);
	Wire.endTransmission();

	delay(4);

	uint8_t readBuf[8] = {0, 0, 0, 0, 0, 0, 0, 0};
	Wire.requestFrom(100, 7);

	for (int i = 0; i < 7; i++)
	{
		if (Wire.available())
		{
			readBuf[i] = Wire.read();
		}
		else
		{
			Serial.println("Wire not available");
			Serial.println(i);
		}
	}

	for (int i = 0; i < 7; i++)
	{
		Serial.println(readBuf[i], HEX);
	}

	Serial.println("Done.");

	while (1)
	{
		;
	}
}
