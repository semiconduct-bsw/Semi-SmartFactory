#define USER_SW  12
void setup() {
    Serial.begin(9600);
    pinMode(USER_SW, INPUT_PULLUP);
}
void loop() {
    Serial.println( digitalRead(USER_SW) );
    delay(10);
}
