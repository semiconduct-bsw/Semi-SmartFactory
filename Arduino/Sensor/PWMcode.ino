void setup() {
   analogWrite(17, 127);
}
void loop() {}

#define CH0_LED_PIN1 17
#define CH0_LED_PIN2 16
uint32_t freq = 5000;         // T = 1/f = 1/5 [ms] = 0.2[ms] = 200[us]
uint8_t resolution10 = 10;   // 10 bit
void setup() {  
   ledcAttach(CH0_LED_PIN1, freq, resolution10);
   ledcAttach(CH0_LED_PIN2, freq, resolution10);   
   ledcWrite(CH0_LED_PIN1, 255);    // Resolution 10 bit(0 ~ 1023) : 255는 1/4
   ledcWrite(CH0_LED_PIN2, 511);    // Resolution 10 bit(0 ~ 1023) : 511는 1/2
}
void loop() {
}

#define CH0_LED_PIN1 17
#define CH0_LED_PIN2 16
#define CH1_LED_PIN1 4
#define CH1_LED_PIN2 12
#define CH1_LED_PIN3 36
uint32_t freq = 5000;         
uint8_t resolution10 = 10;  // 10 bit
uint8_t resolution8 = 8;    // 8 bit

void setup() {  
   Serial.begin(115200);
   
   ledcAttach(CH0_LED_PIN1, freq, resolution10);
   ledcAttach(CH0_LED_PIN2, freq, resolution10);
     
   ledcWrite(CH0_LED_PIN1, 255);    
   ledcWrite(CH0_LED_PIN2, 511);    

   ledcAttach(CH1_LED_PIN1, freq, resolution8);
   // Resolution 8 bit (0 ~ 255)    : 127는 ½
   ledcWrite(CH1_LED_PIN1, 127);    
}
void loop() {}
