from influxdb import InfluxDBClient	#file name : SubHumiTempInsetMod.py
import paho.mqtt.client as mqtt
dbClient = InfluxDBClient(host=‘localhost’, port=8086, username=‘influx_ship’, password=‘1234’, database=‘db_riatech’)

def on_connect( client, userdata, flags, reason_code, properties):
   print(“Connect with result code “ + str(reason_code) )
   client.subscribe(“MyOffice/Indoor/#”)

def on_message( client, userdata, msg ):
   print( msg.topic +” “+str(msg.payload) )
   # msg.topic  ‘MyOffice/Indoor/SensorValue’
   topic = msg.topic.split(‘/’)
   loc = topic[1]
   subloc = topic[2]
   # msg.payload  “{‘Temp’ : 23.1, ‘Humi’: 33.3}”
   payload = eval(msg.payload)

   json_body = [ ]
   data_point = {
	‘measurement’: ‘sensors’,
	‘tags’: {‘Location’: ‘ ‘, ‘SubLocation’:’ ‘},
	‘fields’: {‘Temp’: 0.0, ‘Humi’:0.0}
	}
   data_point[‘tags’][‘Location’] = loc
   data_point[‘tags’][‘SubLocation’] = subloc
   data_point[‘fields’][‘Temp’] = payload[‘Temp’]
   data_point[‘fields’][‘Humi’] = payload[‘Humi’]
   json_body.append(data_point)
   dbClient.write_points( json_body )

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
mqttc.username_pw_set(username=“mqtt_boy”, password=“1234”)
mqttc.on_connect = on_connect
mqttc.on_message = on_message
mqttc.connect(“localhost”, 1883, 60)
mqttc.loop_forever( )
