// 동작 1,2 같이 사용하는 코드
int rLedPin = 13;
int gLedPin = 8;
int buttonPin = 2;

void setup() {
  pinMode(rLedPin, OUTPUT);
  pinMode(gLedPin, OUTPUT);
  pinMode(buttonPin, INPUT);
}

void loop() {
   digitalWrite(rLedPin, HIGH);
   delay(1000);
   digitalWrite(rLedPin, LOW);
   delay(1000);
   int buttonState = digitalRead(buttonPin);
   digitalWrite(gLedPin, buttonState);
}
