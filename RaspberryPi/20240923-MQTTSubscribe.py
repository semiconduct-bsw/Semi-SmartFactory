import paho.mqtt.client as mqtt

def on_connect( client, userdata, flags, rc ):
   print(“Connect with result code “ + str(rc) )
   client.subscribe(“temp”)

def on_message( client, userdata, msg ):
   print( msg.topic +” “+str(msg.payload) )

mqttc = mqtt.Client( )
mqttc.on_connect = on_connect
mqttc.on_message = on_message
mqttc.connect(“localhost”, 1883, 60)
mqttc.loop_forever( )

--------------------------

import paho.mqtt.client as mqtt

def on_connect( client, userdata, flags, reason_code, properties ):
   print(f“Connect with result code:{reason_code}“)
   client.subscribe(“temp”)

def on_message( client, userdata, msg ):
   print( msg.topic +” “+str(msg.payload) )

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
mqttc.on_connect = on_connect
mqttc.on_message = on_message
mqttc.connect(“localhost”, 1883, 60)
mqttc.loop_forever( )
