#define  PIR      36
volatile bool motionDetectTag = false;
void IRAM_ATTR isrMotionDetect(void) {
   motionDetectTag = true;
}
void setup() {
   Serial.begin(115200);
   pinMode(PIR, INPUT);
   attachInterrupt(PIR, isrMotionDetect, RISING);
}
void loop() {
   if (motionDetectTag) {
      motionDetectTag = false;
      Serial.println("Motion detected!");
   }
   delay(10);
}
