void setup() {
  Serial.begin(9600);                         // UART0
  Serial1.begin(9600, SERIAL_8N1, 25, 26);    // UART1의 Rx, Tx 핀을 25, 26으로 변경
  Serial2.begin(9600, SERIAL_8N1, 16, 17);    // UART2의 RX, Tx핀은 16, 17
}
