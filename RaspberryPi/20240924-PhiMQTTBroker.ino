#include <WiFi.h>
#include <PubSubClient.h>
const char* ssid = "RiatechA2G";
const char* password = "730124go";

const char* userId = "mqtt_boy";
const char* userPw = "1234";
const char* clientId = userId;
char* server = "192.168.1.11"; 

WiFiClient wifiClient; 
PubSubClient client(server, 1883, wifiClient);

void setup() {
  Serial.begin(115200);
 
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }
  Serial.println("\nWiFi Connected");
  Serial.print("Local IP address : ");
  Serial.println(WiFi.localIP());  
  Serial.println("Connecting to broker");
  while ( !client.connect(clientId, userId, userPw) ){ 
    Serial.print("*");
    delay(500);
  }
  Serial.println("\nConnected to broker");
}

void loop() {}
