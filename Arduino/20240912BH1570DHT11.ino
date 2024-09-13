#include <SPI.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <BH1750.h>
#include <DHT.h>

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

#define OLED_RESET -1
#define SCREEN_ADDRESS 0x3C ///< See datasheet for Address; 0x3D for 128x64, 0x3C for 128x32
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);
BH1750 lightMeter;

#define DHTPIN 4
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(9600);
  
  // OLED 초기화
  if(!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for(;;);
  }
  
  display.clearDisplay();
  display.display();  // 초기화된 빈 화면을 디스플레이에 적용
  
  // 센서 초기화
  Wire.begin();
  lightMeter.begin();
  dht.begin();  // DHT11 센서 초기화
  
  display.setTextColor(WHITE);
}

void loop() {
  display.clearDisplay();
  // put your main code here, to run repeatedly:
  float lux = lightMeter.readLightLevel();
  // DHT11에서 온도와 습도 값 읽기
  float temperature = dht.readTemperature();  // 섭씨 온도 읽기
  float humidity = dht.readHumidity();        // 습도 읽기

  // 조도 값 출력
  display.setCursor(0, 0);
  display.print("Light: ");
  display.print(lux);
  display.println(" lx");
  
  // 온도 값 출력
  display.setCursor(0, 20);
  display.print("Temp: ");
  display.print(temperature);
  display.println(" C");
  
  // 습도 값 출력
  display.setCursor(0, 40);
  display.print("Humidity: ");
  display.print(humidity);
  display.println(" %");

  // 화면에 표시
  display.display();
  
  delay(1000);  // 1초 대기
}
