#define USER_SW  12
#define BUZZER   3void setup() {
    pinMode(USER_SW, INPUT_PULLUP);
    pinMode(BUZZER, OUTPUT);
    while(digitalRead(USER_SW)) { ; }
}
void loop() {
    tone(BUZZER, 1000); delay(500);
    noTone(BUZZER); delay(500);
}
