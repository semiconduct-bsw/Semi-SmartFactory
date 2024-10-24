// AnalogRead()
void setup() {
   Serial.begin(115200);
}
void loop() {
   Serial.println( analogRead(A4) );
   delay(500);
}

// AnalogReadMilliVolts()
void setup() {
   Serial.begin(115200);
}
void loop() {
   Serial.println( analogRead(A4) );
   Serial.println( analogReadMilliVolts(A4) );
   delay(500);
}

// AnalogReadSolution()
void setup() {
   Serial.begin(115200);
   analogReadResolution(10);
}
void loop() {
   Serial.println( analogRead(A4) );
   Serial.println( analogReadMilliVolts(A4) );
   delay(500);
}
