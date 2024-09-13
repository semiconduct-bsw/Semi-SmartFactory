#include "Timer.h"
#define USER_LED	12
Timer t;

void setup() {
  pinMode(USER_LED, OUTPUT);
  // oscillate = 진동, High부터 Low까지 진동(1000ms)
  t.oscillate(USER_LED, 1000, LOW);
}

void loop() {
	// t 객체가 시간을 보는 것 = 폴링
  t.update();
}
