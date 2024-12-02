#include <Wire.h>
#include <BH1750.h>
#include <DHT.h>

#define DHTPIN 2       // DHT11 데이터 핀
#define DHTTYPE DHT11  // DHT11 센서 유형

DHT dht(DHTPIN, DHTTYPE);
BH1750 lightMeter;

bool sendData = false;  // ON/OFF 상태를 저장
unsigned long previousMillis = 0;  // 이전 시간 저장용
const long interval = 5000;  // 5초 간격

void setup() {
  Serial.begin(9600);
  Wire.begin(); dht.begin();
  lightMeter.begin();
}

void loop() {
  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;

    if (sendData) {
      // 센서 값 읽기
      float temperature = dht.readTemperature();
      float humidity = dht.readHumidity();
      float lux = lightMeter.readLightLevel();

      // 유효한 범위를 벗어나는 값이 있으면 기본값 0으로 설정
      if (temperature < -40 || temperature > 80) temperature = 0;  // 온도 범위 체크
      if (humidity < 0 || humidity > 100) humidity = 0;            // 습도 범위 체크
      if (lux < 0 || lux > 100000) lux = 0;                        // 조도 범위 체크

      // 온/습/조도 데이터를 아두이노 시리얼로 전송
      Serial.print(temperature);
      Serial.print("/");
      Serial.print(humidity);
      Serial.print("/");
      Serial.println(lux);
    }
  }

  // 명령 확인 (Serial.available() 대체)
  if (Serial.peek() != -1) {  // 시리얼 버퍼에 데이터가 있는지 확인
    String command = Serial.readStringUntil('\n');  // 메시지 읽기
    command.trim();  // 공백 제거

    if (command == "ON") {
      sendData = true;  // 데이터를 전송
    } else if (command == "OFF") {
      sendData = false;  // 데이터 전송 멈춤
    }
  }
}
