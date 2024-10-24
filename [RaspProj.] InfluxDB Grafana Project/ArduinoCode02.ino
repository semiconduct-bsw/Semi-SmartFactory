#include <WiFi.h>
#include <HTTPClient.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <BH1750.h>

// WiFi 연결 정보
const char* ssid = "RiatechA2G";      // Wi-Fi 네트워크 이름
const char* password = "730124go";    // Wi-Fi 비밀번호

// InfluxDB 서버 정보
const char* serverName = "http://192.168.1.11:8086/write?db=db_clean";  // InfluxDB URL

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

unsigned long previousMillis = 0;  // 이전 시간 저장용
const long interval = 5000;        // 5초마다 갱신

void setup() {
  // Serial 통신 시작
  Serial.begin(115200);

  // WiFi 연결 설정
  connectToWiFi();

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

// Wi-Fi 연결 함수
void connectToWiFi() {
  WiFi.begin(ssid, password);
  int attempt = 0;

  while (WiFi.status() != WL_CONNECTED && attempt < 10) {  // 10번까지만 연결 시도
    delay(1000);
    Serial.println("Connecting to WiFi...");
    attempt++;
  }

  if (WiFi.status() == WL_CONNECTED) {
    Serial.println("Connected to WiFi");
  } else {
    Serial.println("Failed to connect to WiFi");
    while (true);  // WiFi 연결 실패 시 멈춤
  }
}

// InfluxDB로 데이터 전송 함수
void sendToInfluxDB(float temperature, float humidity, float lux) {
  if (WiFi.status() == WL_CONNECTED) {  // WiFi 연결 확인
    HTTPClient http;

    // 라인 프로토콜 형식의 데이터 생성
    String postData = "environment,location=cleanroom ";
    postData += "temperature=" + String(temperature) + ",";
    postData += "humidity=" + String(humidity) + ",";
    postData += "light=" + String(lux);

    http.begin(serverName);  // InfluxDB 서버 주소 설정
    http.addHeader("Content-Type", "text/plain");  // HTTP 헤더 설정

    // HTTP POST 요청 비동기 처리 (바로 반환)
    int httpResponseCode = http.POST(postData);  // HTTP POST 요청 전송

    // HTTP 요청을 블로킹하지 않기 위해서 응답 처리는 필요시만 확인
    if (httpResponseCode > 0) {
      Serial.println(httpResponseCode);  // 응답 코드 출력
    } else {
      Serial.print("Error on sending POST: ");
      Serial.println(httpResponseCode);
    }

    http.end();  // HTTP 연결 종료
  } else {
    Serial.println("WiFi Disconnected");
  }
}

void loop() {
  // 현재 시간을 저장
  unsigned long currentMillis = millis();

  // 5초마다 갱신
  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;  // 마지막 갱신 시간을 저장

    // DHT11 센서 데이터 읽기
    float temperature = dht.readTemperature();
    float humidity = dht.readHumidity();

    // BH1750 조도 센서 데이터 읽기
    float lux = lightMeter.readLightLevel();

    // OLED에 데이터 출력
    display.clearDisplay();  // OLED 화면 초기화

    display.setCursor(0, 0);
    display.println(F("Cleanroom Env Monitor"));

    // 온도와 습도 출력
    display.setCursor(0, 16);
    if (isnan(temperature) || isnan(humidity)) {
      display.println(F("DHT11: Error reading"));
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

    // OLED 화면 업데이트
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

    // InfluxDB에 데이터 전송 (OLED 갱신 후)
    sendToInfluxDB(temperature, humidity, lux);
  }
}
