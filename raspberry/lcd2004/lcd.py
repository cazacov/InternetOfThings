#!/usr/bin/python

import lcddriver
import re
from time import *
from subprocess import check_output

lcd = lcddriver.lcd()

ip = check_output(['hostname', '-I'])
m = re.search(r'\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b', ip)
if m is not None:
    ip = m.group(0)
else:
    ip = 'Not connected'

starttime = strftime("%H:%M:%S", localtime())

lcd.lcd_display_string("Raspberry PI 2", 1)
lcd.lcd_display_string('IP: ' + ip, 2)
lcd.lcd_display_string('Started at: ' + starttime, 3)
lcd.lcd_display_string("", 4)

