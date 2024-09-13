#include <Wire.h>
#include "pcf8591.h"
void setup() {
  Serial.begin(9600);
  Wire.begin();
}
void loop() {
  Wire.beginTransmission(ADDR0_PCF8591);
  Wire.write(ADC_CH0);
  Wire.endTransmission();
  Wire.requestFrom(ADDR0_PCF8591, 1);
  Serial.println( String( Wire.read() * 5.0/256 ) + "[V]");
  delay(100);
}

// pcf8591.h

#define ADDR0_PCF8591 0x48
#define ADDR1_PCF8591 0x49

#define ADC_CH0       0x00
#define ADC_CH1       0x01
