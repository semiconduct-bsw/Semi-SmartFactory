#include <Wire.h>
#include <BH1750.h>
#include <DHT.h>

#define DHTPIN 2       // DHT11 데이터 핀 (D2)
#define DHTTYPE DHT11  // DHT11 센서 유형

DHT dht(DHTPIN, DHTTYPE);  // DHT 객체 생성
BH1750 lightMeter;         // BH1750 객체 생성

void setup() {
  Serial.begin(9600);  // 시리얼 통신 시작
  Wire.begin();

  // DHT 센서 초기화
  dht.begin();

  // BH1750 조도 센서 초기화
  if (!lightMeter.begin()) {
    Serial.println("BH1750 초기화 실패!");
    while (1);
  }
}

void loop() {
  // 온도와 습도 값 읽기
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();

  // 조도 값 읽기
  float lux = lightMeter.readLightLevel();

  // 데이터 유효성 검사 (센서 오류 시 NaN값 방지)
  if (isnan(temperature) || isnan(humidity) || lux < 0) {
    Serial.println("센서 오류!");
  } else {
    // 온/습/조도 데이터를 "온/습/조도" 형식으로 출력
    Serial.print(temperature);
    Serial.print("/");
    Serial.print(humidity);
    Serial.print("/");
    Serial.println(lux);
  }

  delay(5000);  // 5초마다 업데이트
}
