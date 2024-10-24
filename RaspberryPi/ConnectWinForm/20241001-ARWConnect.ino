#include <WiFi.h>
#include <PubSubClient.h>

#define LED_PIN 15  // LED 핀 정의

const char* ssid = "pcroom";
const char* password = "12345678";
const char* userId = "mqtt_boy";
const char* userPw = "1234";
const char* clientId = userId;
const char* topic = "MyOffice/Indoor/LEDControl";  // 특정 토픽으로 변경
const char* serverIPAddress = "192.168.0.27";
int messageBuf;  // 메시지 버퍼를 int로 정의

// MQTT 메시지 콜백 함수
void callback(char* topic, byte* payload, unsigned int length) {
  // payload를 int로 변환
  messageBuf = atoi((char*)payload);

  Serial.print("Received message: ");
  Serial.println(messageBuf);  // 메시지를 출력하여 확인
  
  // 메시지에 따라 LED 제어
  if (messageBuf == 1) {
    digitalWrite(LED_PIN, HIGH); 
  } else {
    digitalWrite(LED_PIN, LOW);
  }
}

WiFiClient wifiClient;
PubSubClient client(serverIPAddress, 1883, callback, wifiClient);

void setup() {
  Serial.begin(9600);
  pinMode(LED_PIN, OUTPUT);  // LED 핀을 출력으로 설정

  // WiFi 연결
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi Connected!");

  // MQTT 브로커 연결
  while (!client.connect(clientId, userId, userPw)) {
    delay(500);
    Serial.print("*");
  }
  Serial.println("\nConnected to MQTT broker");

  // 토픽 구독
  client.subscribe(topic);
}

void loop() {
  client.loop();  // MQTT 메시지를 계속 받기
}
