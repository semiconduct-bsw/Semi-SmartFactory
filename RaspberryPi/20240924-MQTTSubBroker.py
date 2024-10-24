import paho.mqtt.client as mqtt

def on_connect( client, userdata, flags, reason_code, properties ):
   print(f“Connect with result code:{reason_code}“)
   client.subscribe(“temp”)

def on_message( client, userdata, msg ):
   print( msg.topic +” “+str(msg.payload) )

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
mqttc.username_pw_set(username=“mqtt_boy”, password=“1234”)
mqttc.on_connect = on_connect
mqttc.on_message = on_message
mqttc.connect(“broker_IP_Address”, 1883, 60)
mqttc.loop_forever( )
