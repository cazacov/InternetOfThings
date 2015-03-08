import lcddriver
from time import *

lcd = lcddriver.lcd()

lcd.lcd_display_string("Hello world", 1)
lcd.lcd_display_string("Line 2", 2)
lcd.lcd_display_string("Line 3", 3)
lcd.lcd_display_string("Line 4", 4)

