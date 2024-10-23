#include <ZumoShield.h>
#include <ZumoMotors.h>

#define NUM_SENSORS 6
#define TIMEOUT 1000 	// 센서 감도 조절 
#define EMITTER_PIN 2

QTRSensorsRC qtrrc( (unsigned char[]) {4, A3, 11, A0, A2, 5}, NUM_SENSORS, TIMEOUT, EMITTER_PIN);
ZumoMotors motors;

unsigned int sensorValues[NUM_SENSORS];

// 이전 모터 속도 저장
int lastLeftSpeed = 0;
int lastRightSpeed = 0;

void setup() {
  Serial.begin(9600);
  delay(1000);
}

void loop() {
  qtrrc.read(sensorValues);
  
  int CenterRight = digitalRead(A0);
  int MiddleRight = digitalRead(A2);
  int FarRight = digitalRead(5);
  int CenterLeft = digitalRead(11);
  int MiddleLeft = digitalRead(A3);
  int FarLeft = digitalRead(4);
  
  // 센서 값 출력
  Serial.print(FarLeft); Serial.print('\t'); 
  Serial.print(MiddleLeft); Serial.print('\t');
  Serial.print(CenterLeft); Serial.print('\t'); 
  Serial.print(CenterRight); Serial.print('\t'); 
  Serial.print(MiddleRight); Serial.print('\t'); 
  Serial.println(FarRight);

  int m1Speed = 0;
  int m2Speed = 0;

  // 센서가 아무것도 감지하지 못한 경우 (모든 센서가 0일 때)
  if (FarLeft == 0 && MiddleLeft == 0 && CenterLeft == 0 && CenterRight == 0 && MiddleRight == 0 && FarRight == 0) {
    // 이전 속도로 계속 진행
    m1Speed = lastLeftSpeed;
    m2Speed = lastRightSpeed;
  } 
  // Center 센서가 선을 감지했을 때 직진
  else if (CenterLeft == 1 || CenterRight == 1) {
    m1Speed = 220;
    m2Speed = 220;
  } 
  // 왼쪽으로 치우쳤을 때
  else if (FarLeft == 1) {  // FarLeft 센서가 선을 감지 -> 급격히 오른쪽으로 회전
    m1Speed = 60;  // 왼쪽 모터를 조금 느리게
    m2Speed = 375;  // 오른쪽 모터를 빠르게
  } 
  else if (MiddleLeft == 1) {  // MiddleLeft 센서가 선을 감지 -> 완만히 오른쪽으로 회전
    m1Speed = 95;  // 왼쪽 모터를 느리게
    m2Speed = 230;  // 오른쪽 모터를 빠르게
  }
  // 오른쪽으로 치우쳤을 때
  else if (FarRight == 1) {  // FarRight 센서가 선을 감지 -> 급격히 왼쪽으로 회전
    m1Speed = 375;  // 왼쪽 모터를 빠르게
    m2Speed = 60;  // 오른쪽 모터를 느리게
  } 
  else if (MiddleRight == 1) {  // MiddleRight 센서가 선을 감지 -> 완만히 왼쪽으로 회전
    m1Speed = 230;  // 왼쪽 모터를 빠르게
    m2Speed = 95;  // 오른쪽 모터를 느리게
  }
  else if (CenterLeft == 1 && CenterRight == 1 && FarLeft == 1 && MiddleLeft == 1 && FarRight == 1 && MiddleRight == 1) {
    m1Speed = 0;
    m2Speed = 0;
  }
  // 모터 속도 설정
  motors.setLeftSpeed(m1Speed);
  motors.setRightSpeed(m2Speed);
  
  // 마지막 모터 속도 저장
  lastLeftSpeed = m1Speed;
  lastRightSpeed = m2Speed;
  
  delay(1);  // 100ms 대기
}
