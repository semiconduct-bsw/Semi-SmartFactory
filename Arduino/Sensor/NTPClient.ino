#include <NTPClient.h>
#include <WiFi.h>
#include <WiFiUdp.h>
const char *ssid     = "RiatechA2G";
const char *password = "730124go";

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP);
void setup(){
  Serial.begin(115200);  WiFi.begin(ssid, password);
  while ( WiFi.status() != WL_CONNECTED ) {
    delay ( 500 );
    Serial.print ( "." );
  }  timeClient.begin();
}
void loop() {
  timeClient.update();
  Serial.println(timeClient.getFormattedTime());
  delay(1000);
}
