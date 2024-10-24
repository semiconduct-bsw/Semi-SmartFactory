smart123@Rasb1234:~ $ influx
Connected to http://localhost:8086 version 1.6.7~rc0
InfluxDB shell version: 1.8.10

> create database db_clean
> use db_clean

> create user influx_phirippa with password '1234' with all privileges
> grant all privileges on db_clean to influx_phirippa
> show users

> create user influx_ship with password '1234' with all privileges
> grant all privileges on db_clean to influx_ship
> show users
