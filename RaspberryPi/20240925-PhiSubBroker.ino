// esp32_subscribe_DHT11.ino
#include <WiFi.h>
#include <PubSubClient.h>
const char* ssid = "RiatechA2G";
const char* password = "730124go";
const char* userId = "mqtt_boy";
const char* userPw = "1234";
const char* clientId = userId;
const char *topic = "MyOffice/Indoor/#";
const char* serverIPAddress = "192.168.1.11";
char messageBuf[100];

void callback(char* topic, byte* payload, unsigned int length) { 
  Serial.println("Message arrived!\nTopic: " + String(topic));
  Serial.println("Length: "+ String(length, DEC));
 
  strncpy(messageBuf, (char*)payload, length);
  messageBuf[length] = '\0';
  Serial.println("Payload: "+ String(messageBuf) + "\n\n");
}

WiFiClient wifiClient; 

PubSubClient client(serverIPAddress, 1883, callback, wifiClient);
void setup() {
   Serial.begin(115200);
   Serial.print("\nConnecting to "); Serial.println(ssid);
   WiFi.begin(ssid, password);
   while (WiFi.status() != WL_CONNECTED) {
      Serial.print("."); delay(500);
   }
   Serial.println("\nWiFi Connected\nConnecting to broker");
   while ( !client.connect(clientId, userId, userPw) ){ 
      Serial.print("*"); delay(500);
   }
   Serial.println("\nConnected to broker");
   client.subscribe(topic);
}
void loop() {
   client.loop();
}
