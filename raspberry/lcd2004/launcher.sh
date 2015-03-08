#!/bin/sh
# launcher.sh
# navigate to home directory, then to this directory, then execute python script, then back home

sleep 10
cd /
cd home/pi/git/InternetOfThings/raspberry/lcd2004
sudo python lcd.py
cd /
