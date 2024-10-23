#include <ZumoShield.h>
#include <ZumoMotors.h>
#define NUM_SENSORS 6
#define TIMEOUT 1200 	// 센서 감조 조절 
#define EMITTER_PIN 2
QTRSensorsRC qtrrc( (unsigned char[]) {4, A3, 11, A0, A2, 5}, NUM_SENSORS, TIMEOUT, 			EMITTER_PIN);

ZumoMotors motors;
unsigned int sensorValues[NUM_SENSORS];
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
  Serial.print(FarLeft); Serial.print('\t'); 
  Serial.print(MiddleLeft); Serial.print('\t');
  Serial.print(CenterLeft); Serial.print('\t'); 
  Serial.print(CenterRight); Serial.print('\t'); 
  Serial.print(MiddleRight); Serial.print('\t'); 
  Serial.println(FarRight); 
}
