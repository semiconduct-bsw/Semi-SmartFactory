#include <Wire.h>
#include <BH1750.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_WIDTH 128  // OLED 디스플레이 너비
#define SCREEN_HEIGHT 64  // OLED 디스플레이 높이
#define OLED_RESET -1     // Reset 핀이 없는 경우 -1로 설정

BH1750 lightMeter;
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

void setup() {
  Serial.begin(9600);  // 시리얼 모니터 시작
  Wire.begin();

  // BH1750 초기화
  if (!lightMeter.begin()) {
    Serial.println("BH1750 초기화 실패!");
    while (1);
}
Serial.println("BH1750 시작!");


  // SSD1306 OLED 초기화
  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) {  // 디스플레이 I2C 주소가 0x3C일 경우
    Serial.println("SSD1306 할당 실패!");
    while (1);
  }
  display.display();  // 초기 디스플레이 클리어
  delay(2000);  // OLED 초기화 시간

  display.clearDisplay();
  display.setTextSize(1);       // 텍스트 크기 설정
  display.setTextColor(SSD1306_WHITE);  // 텍스트 색상
}

void loop() {
  // 조도 값 읽기
  float lux = lightMeter.readLightLevel();
  Serial.print("Light: ");
  Serial.print(lux);
  Serial.println(" lx");

  // OLED에 출력
  display.clearDisplay();
  display.setCursor(0, 10);
  display.print("Light Level:");
  display.setCursor(0, 30);
  display.print(lux);
  display.print(" lx");
  display.display();

  delay(1000);  // 1초마다 업데이트
}

