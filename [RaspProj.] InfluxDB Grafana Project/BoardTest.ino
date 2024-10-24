#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <BH1750.h>

// DHT11 설정
#define DHTPIN 4  // DHT11 데이터 핀이 연결된 GPIO 번호
#define DHTTYPE DHT11  // DHT22를 사용할 경우 DHT11 대신 DHT22로 변경
DHT dht(DHTPIN, DHTTYPE);

// OLED 디스플레이 설정
#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
#define OLED_RESET -1
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

// BH1750 조도 센서 설정
BH1750 lightMeter;

void setup() {
  // Serial 통신 시작
  Serial.begin(115200);

  // DHT 센서 시작
  dht.begin();

  // OLED 디스플레이 시작
  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) {
    Serial.println(F("SSD1306 OLED 초기화 실패"));
    while (true);  // 초기화 실패 시 멈춤
  }

  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.println(F("Starting..."));
  display.display();

  // BH1750 조도 센서 시작
  if (!lightMeter.begin()) {
    Serial.println(F("BH1750 초기화 실패"));
    display.clearDisplay();
    display.setCursor(0, 0);
    display.println(F("BH1750 Init Fail"));
    display.display();
    while (true);  // 초기화 실패 시 멈춤
  }
  
  delay(2000);  // 시스템 안정화를 위한 대기 시간
}

void loop() {
  // DHT11 센서 데이터 읽기
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();

  // BH1750 조도 센서 데이터 읽기
  float lux = lightMeter.readLightLevel();

  // OLED에 데이터 출력
  display.clearDisplay();

  display.setCursor(0, 0);
  display.println(F("Cleanroom Env Monitor"));

  // 온도와 습도 출력
  display.setCursor(0, 16);
  if (isnan(temperature) || isnan(humidity)) {
    display.println(F("DHT11: Err reading"));
  } else {
    display.print(F("Temp: "));
    display.print(temperature);
    display.println(F(" C"));

    display.print(F("Humidity: "));
    display.print(humidity);
    display.println(F(" %"));
  }

  // 조도 출력
  display.setCursor(0, 40);
  display.print(F("Light: "));
  display.print(lux);
  display.println(F(" lux"));

  // 디스플레이 업데이트
  display.display();

  // Serial 모니터에도 출력 (디버깅용)
  Serial.print("Temp: ");
  Serial.print(temperature);
  Serial.println(" *C");

  Serial.print("Humidity: ");
  Serial.print(humidity);
  Serial.println(" %");

  Serial.print("Light: ");
  Serial.print(lux);
  Serial.println(" lux");

  delay(2000);  // 2초마다 갱신
}
