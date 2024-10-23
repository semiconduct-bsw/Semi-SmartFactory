#include <ZumoMotors.h>
ZumoMotors motors;
void setup() {
  int m1Speed = 200;
  int m2Speed = 200;
  motors.setSpeeds(m1Speed, m2Speed);
}
void loop() {
}
