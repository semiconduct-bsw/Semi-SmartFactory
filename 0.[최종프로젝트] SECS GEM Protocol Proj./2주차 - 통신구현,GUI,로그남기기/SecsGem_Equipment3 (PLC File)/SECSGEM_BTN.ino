const int buttonPin = 2;          // 스위치가 연결된 핀
String input_signal;               // 시리얼 입력 문자열 변수
bool reportID_flag = false;        // 데이터 출력 여부 플래그
bool buttonState = false;          // 현재 스위치 상태
bool lastButtonState = false;      // 마지막 스위치 상태

void setup() {
  pinMode(buttonPin, INPUT_PULLUP);   // 스위치 핀을 풀업 입력 모드로 설정
  Serial.begin(9600);                 // 시리얼 통신 시작
  Serial.println("장비를 시동하기 위해서는 on 신호를 입력하세요:");
}

void loop() {
  // 시리얼로 입력 받기
  if (Serial.available() > 0) { 
    input_signal = Serial.readStringUntil('\n'); // 줄바꿈까지 문자열 읽기
    Serial.print("입력한 문자열: ");
    Serial.println(input_signal); // 입력된 문자열 출력
    
    // 입력 신호에 따라 플래그 변경
    if (input_signal == "ON" || input_signal == "on") {
      reportID_flag = true;
      Serial.println("데이터 출력 시작");
    } 
    else if (input_signal == "OFF" || input_signal == "off") {
      reportID_flag = false;
      Serial.println("데이터 출력 중단");
    } 
    else {
      Serial.println("on/off signal 입력 오류");
    }
  }

  // 데이터 출력 조건: reportID_flag가 true이고, 스위치가 눌렸을 때
  if (reportID_flag) {
    buttonState = !digitalRead(buttonPin); // 스위치가 눌렸는지 확인 (풀업 모드이므로 반전)
    
    if (buttonState && !lastButtonState) { // 스위치가 눌렸을 때만 한 번 출력
      Serial.println("10701");
    }

    lastButtonState = buttonState; // 스위치 상태 업데이트
  }

  delay(50);  // 디바운싱을 위한 짧은 지연 시간
}
