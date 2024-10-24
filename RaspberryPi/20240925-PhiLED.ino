// esp32_subscribe_LED.ino
#include <WiFi.h>
#include <PubSubClient.h>
const char* ssid = "RiatechA2G";
const char* password = "730124go";
const char* userId = “mqtt_boy";
const char* userPw = "1234";
const char* clientId = userId;
char *topic = “MyOffice/Indoor/Lamp";
char* server = "192.168.1.11"; 
char messageBuf[100];

void callback(char* topic, byte* payload, unsigned int length) {  
  Serial.println("Message arrived!\nTtopic: " + String(topic));
  Serial.println("Length: "+ String(length, DEC));
  strncpy(messageBuf, (char*)payload, length);
  messageBuf[length] = '\0';
  String ledState = String(messageBuf);
  Serial.println("Payload: "+ ledState + "\n\n");
  if( ledState == “0"  ){ digitalWrite(LED_BUILTIN, LOW);
  } else if (ledState==“1") { digitalWrite(LED_BUILTIN, HIGH);
  } else { Serial.println("Wrong Message"); }
}


WiFiClient wifiClient; 
PubSubClient client(server, 1883, callback, wifiClient);

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(115200);
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");     delay(1000);
  }
  Serial.println("\nWiFi Connected");
 
  while ( !client.connect(clientId, userId, userPw) ){ 
    Serial.print("*");     delay(1000);
  }
  Serial.println("\nConnected to broker");
  Serial.println(String("Subscribing! topic = ") + topic);
  client.subscribe(topic);
 }

void loop() {   client.loop(); }
