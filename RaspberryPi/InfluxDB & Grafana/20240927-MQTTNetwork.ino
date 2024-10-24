// esp32_publish_DHT11.ino
#include <WiFi.h>
#include <PubSubClient.h>
#define PAYLOAD_MAXSIZE 64
// ----- DHT11 ------------------------
#include "DHT.h"
#define DHTPIN 4     
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);
// -------------------------------------
const char* ssid = "RiatechA2G";
const char* password = "730124go";
const char* userId = "mqtt_boy";
const char* userPw = "1234";
const char* clientId = userId;
char *topic = "MyOffice/Indoor/SensorValue";
char* server = "192.168.1.11"; 
WiFiClient wifiClient; 
PubSubClient client(server, 1883, wifiClient);

void setup() {
  Serial.begin(115200);
 
  Serial.print("\nConnecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(1000);
  }
  Serial.println("\nWiFi Connected");
  
  Serial.println("Connecting to broker");
  while ( !client.connect(clientId, userId, userPw) ){ 
    Serial.print("*");
    delay(1000);
  }
  Serial.println("\nConnected to broker");
  dht.begin();
}
void loop() {
   char payload[PAYLOAD_MAXSIZE];
   float h = dht.readHumidity();
   float t = dht.readTemperature();
   if (isnan(h) || isnan(t) ) {
      Serial.println("Failed to read from DHT sensor!");
   return;
   }
   String sPayload = "{'Temp':"
               + String(t, 1)
               + ",'Humi':"
               + String(h, 1)
               + "}";
   sPayload.toCharArray(payload, PAYLOAD_MAXSIZE);
   client.publish(topic, payload);
   Serial.print(String(topic) + " ");
   Serial.println(payload);
   delay(2000);
}
