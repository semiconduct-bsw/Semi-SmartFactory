int ledPin = 9;

void setup() {
  pinMode(ledPin, OUTPUT);
}

void loop() {
  int sensorValue = analogRead(A0);
  int brightness = sensorValue / 1023.0 * 255;
  analogWrite(ledPin,  brightness);
  delay(20);
}
