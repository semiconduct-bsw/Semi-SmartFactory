### Installing InfluxDB

단계 1. Raspberrypi(buster)의 소프트웨어를 최신 버전 업그레이드

- pi@raspberrypi:~ $ sudo apt update && sudo apt upgrade -y

단계 2. Installing InfluxDB to the Raspberrypi(buster)

- pi@raspberrypi:~ $ curl https://repos.influxdata.com/influxdata-archive.key | gpg --dearmor | sudo tee /usr/share/keyrings/influxdbarchive-keyring.gpg >/dev/null
- pi@raspberrypi:~ $ echo "deb [signed-by=/usr/share/keyrings/influxdbarchive-keyring.gpg] https://repos.influxdata.com/debian stable main" | sudo tee /etc/apt/sources.list.d/influxdb.list
- pi@raspberrypi:~ $ sudo apt update
- pi@raspberrypi:~ $ sudo apt install influxdb

단계 3. 부팅될 때 InfluxDB 서버 활성화

- pi@raspberrypi:~ $ sudo systemctl unmask influxdb
- pi@raspberrypi:~ $ sudo systemctl enable influxdb

단계 4. InfluxDB 서버 시작

- pi@raspberrypi:~ $ sudo systemctl start influxdb


### Grafana 설치

단계 1

- $ sudo mkdir -p /etc/apt/keyrings/
- $ wget q -O - https://apt.grafana.com/gpg.key | gpg --dearmor | sudo tee /etc/apt/keyrings/grafana.gpg > /dev/null

단계 2

- $ echo "deb [signed-by=/etc/apt/keyrings/grafana.gpg] https://apt.grafana.com stable main" | sudo tee /etc/apt/sources.list.d/grafana.list

단계 3

- $ sudo apt-get update
- $ sudo apt-get install -y grafana

단계 4

- $ sudo /bin/systemctl enable grafanaserver

단계 5

- $ sudo /bin/systemctl start grafanaserver

단계 6

- 브라우저를 열어서 http://<서버 IP 주소>:3000에 접속

### 데이터베이스 설정

1. URL – http://localhost:8086 or http://라즈베리 파이 IP address:8086

- Grafana 서버 입장에서 접속 해야 하는 InfluxDB 서버는 어디에 있는가? localhost

2. InfluxDB 설정

- Database – db_riatech
    - 이전 단계에서 생성한 데이터베이스 이름
- User/Password
    - InfluxDB의 db_riatech에 접속할 수 있는 사용자 정보

3. Save & Test

- Save & Test 버튼 클릭 후 아래와 같은 녹색창(Data source is working)이 나오면 정상
