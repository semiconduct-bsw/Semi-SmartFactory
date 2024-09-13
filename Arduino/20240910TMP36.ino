void setup() {
  Serial.begin(9600);  
}
 
void loop() {
  int sensorValue = analogRead(A0);  
  float V = sensorValue * 5.0/1024.0; 
  float temp = 100.0*V – 50;  
 
  Serial.print(temp); 
  Serial.println(“[℃]”);
 
  delay(1000);
}
