# Grafana 설치
sudo apt-get install -y apt-transport-https
sudo apt-get install -y software-properties-common wget
wget -q -O - https://packages.grafana.com/gpg.key | sudo apt-key add -
sudo add-apt-repository "deb https://packages.grafana.com/oss/deb stable main"
sudo apt-get update && sudo apt-get install grafana

# Grafana 시작 및 부팅 시 자동 실행 설정
sudo systemctl start grafana-server
sudo systemctl enable grafana-server.service
