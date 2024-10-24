from influxdb import InfluxDBClient
client = InfluxDBClient( host='192.168.1.11', port=8086, username=‘influx_phirippa', password=‘1234',
                        database='db_riatech’)
results = client.query('select * from My_office where Humi != 0 and Temp != 0’)

points = results.get_points( tags= {‘Location’ : ‘Indoor’} )

for point in points:
    print('time: %s Location: %s Temp.: %f Humi.: %f' % (point['time'], point['Location'], point['Temp'], point['Humi']) 
