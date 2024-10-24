hw_timer_t *timer0 = NULL;
volatile bool has_expired = false;
void IRAM_ATTR isr_timer0Interrupt() {
  has_expired = true;
}
void setup() {
  Serial.begin(115200);
  pinMode(LED_BUILTIN, OUTPUT);
  // Set timer frequency to 1MHz
  timer0 = timerBegin(1000000); 
  // Attach isr_timer0Interrupt to our Timer0. 
  timerAttachInterrupt(timer0, &isr_timer0Interrupt); 
  // Set alarm to call isr_timer0Interrupt every second(value in microseconds)
  // Repeat the alarm (third parameter) with unlimited count = 0(fourth parameter)
  timerAlarm(timer0, 1000000, true, 0);
}
void loop() {
  if (has_expired){
    digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));
    has_expired = false;
  }
}
