unsigned long int etime = 0;
void setup() {
  Serial.begin(9600);
}

void loop() {
  etime = micros( );
  analogRead(A0);
  Serial.println( micros( ) – etime );
}
