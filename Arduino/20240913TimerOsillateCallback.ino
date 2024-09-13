#include <Timer.h>
#define USER_LED	13
Timer t;

void flip(void) {
  static int ledState = LOW;
  digitalWrite(USER_LED, ledState );
  ledState = !ledState;
}

void setup() {
  pinMode(USER_LED, OUTPUT);
  t.every(1000, flip);
}

void loop() {
  t.update();
}
