void setup() {
   Serial.begin(115200);
   unsigned long int startTime = micros();
   dacWrite(25, 64);
   Serial.println(micros() - startTime);
}
void loop() {}
}
