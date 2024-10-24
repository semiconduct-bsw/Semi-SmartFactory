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
