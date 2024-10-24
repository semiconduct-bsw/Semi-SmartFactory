from influxdb import InfluxDBClient
import time
import random

client = InfluxDBClient( host='localhost', port=8086, 
	         username=‘influx_ship', password=‘1234',  database='db_riatech’)

def randomDataPoint():
    json_body=[]
    data_point =   {   'measurement' : ‘sensors',
                       'tags' : { 'Location' : 'Indoor'},  # 'outdoor'
                       'fields' : {'Temp': 0.0, 'Humi' : 0.0 }
                    }  
    
    data_point['fields']['Temp'] = random.random() * 50.0
    data_point['fields']['Humi'] = random.random() * 30.0
    
    if (random.random() > 0.5):
        data_point['tags']['Location'] = 'Indoor'
    else:
        data_point['tags']['Location'] = 'Outdoor'
    
    json_body.append(data_point)
    return json_body

//파일이름 : insertBasicRandom.ipynb

while True:
    json_body = randomDataPoint()
    print(json_body)
    client.write_points( json_body )
    time.sleep(5)
