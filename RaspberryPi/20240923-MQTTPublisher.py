import paho.mqtt.client as mqtt
mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
mqttc.connect(“localhost”, 1883, 60)
mqttc.publish(‘temp’, 25.1)

--------------

#file name : pubBasic2.py
import paho.mqtt.publish as publish
publish.single(“temp”, 21.1, hostname=“localhost”)
