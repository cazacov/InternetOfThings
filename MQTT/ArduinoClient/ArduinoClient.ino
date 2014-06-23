/*
 Basic MQTT example 
*/

#define TOPIC_STATUS "home/status"
#define TOPIC_TEMPERATURE "home/temperature"
#define MY_ID "ArduinoClient"
#define MQTT_LOGIN  NULL
#define MQTT_PASSWORD  NULL

#include <SPI.h>
#include <Ethernet.h>
#include <PubSubClient.h>

bool connectToTheServer(void);

// Update these with values suitable for your network.
byte mac[]    = {  0xDE, 0xED, 0xBA, 0xFE, 0xFE, 0xED };
byte server[] = { 192, 168, 178, 48 };
byte ip[]     = { 192, 168, 178, 100 };

char payloadBuf[200];

void callback(char* topic, byte* payload, unsigned int length) {

  strncpy(payloadBuf, (char*)payload, 128);
  payloadBuf[128] = 0;
  
  // handle message arrived
  if (strcmp(topic, TOPIC_TEMPERATURE))
  {
    Serial.print("Callback ");
    Serial.print(topic);
    Serial.print(length);
    Serial.println(payloadBuf);
  }
}

EthernetClient ethClient;
PubSubClient client(server, 1883, callback, ethClient);

void setup()
{
  char msg[20];
  sprintf(msg, "online again and again");
  
  Serial.begin(57600);
  Serial.println("Starting Ethernet...");
  Ethernet.begin(mac, ip);
  if (connectToTheServer())
  {
    client.subscribe(TOPIC_TEMPERATURE);    
    client.subscribe(TOPIC_STATUS);    
  }
}

long nextPublish = millis() + 20;

void loop()
{
  client.loop();
  
  if (!client.connected())
  {
    connectToTheServer();
  }
  
  if (millis() > nextPublish)
  {
  char msg[20];
  int val = analogRead(0);
  int temp10 = val;
  sprintf(msg, "%d", temp10);
  client.publish(TOPIC_TEMPERATURE, (byte*)msg, strlen(msg)+1, 1);
    nextPublish = millis()+10;
  }
}

bool connectToTheServer(void)
{
    
  Serial.println("Connecting to the MQTT server...");
  if (client.connect(MY_ID, TOPIC_STATUS, 1, 1, "offline")) {
    client.publish(TOPIC_STATUS, (byte*)"online", strlen("online")+1, 1);    
    Serial.println("Ok");    
    return true;
  }
  else
  {
    Serial.println("Connection failed");
    return false;
  }
}
