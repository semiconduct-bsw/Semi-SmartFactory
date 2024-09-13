void setup() {
  Serial.begin(9600);
}

// the loop function runs over and over again forever
void loop() {
  int adcValue = analogRead(A0);
  Serial.println(adcValue);
  delay(100);
}

void loop() {
  int sensorValue = analogRead(A0);
  float voltage = sensorValue * 5.0/1024.0;
  Serial.println(voltage, 4);
  delay(50);
}
