#include "BluetoothSerial.h"
String device_name = "ESP32-BT-Slave XXX";BluetoothSerial SerialBT;
void setup() {
  Serial.begin(115200);
  SerialBT.begin(device_name); 
  Serial.printf("The device with name \"%s\" is started.");
  Serial.print("\nNow you can pair it with Bluetooth!\n", device_name.c_str());
}
void loop() {
  if (Serial.available()) {
    SerialBT.write(Serial.read());
  }
  if (SerialBT.available()) {
    Serial.write(SerialBT.read());
  }
  delay(20);
}
