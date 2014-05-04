/*
 Connects Arduino to Raspberry Pi
 Arduino Mega: Serial 1 on digital pins 18 and 19
 Raspberry Pi: GPIO UART connected to Arduino over 3.3<->5V level shifter
 
 This is just a simple passthrough, based on Arduino SoftSerial example
*/


void setup()  
{
 // Open serial communications to PC and wait for port to open:
  Serial.begin(57600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }

  Serial.println("Connected to PC");

  // set the data rate for the port to Raspberry Pi
  Serial1.begin(9600);
}

void loop() // run over and over
{
  int byteCount;

  // If data is available on Raspberry Pi, print it to PC
  if (byteCount = Serial1.available())
  {
    while(byteCount--) 
    {
      Serial.write(Serial1.read());
    }
  }

    // If data is available on PC, print it to Raspberry Pi
    if (byteCount = Serial.available())
    {
    while (byteCount--)
    {
      Serial1.write(Serial.read());
    }
  }
}
