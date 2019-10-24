# encoding = utf8
from PIL import Image
import array as arr

im = Image.open('QCImage3.png', 'r')
pix_val = list(im.getdata())

r, c = 8, 8
pix_arr = [[0 for x in range(r)] for y in range(c)]

file_write  = open('..\PQC\pixels.txt', 'w')
file_write.write('8')

col = 0
row = -1

for i, (a, b, c) in enumerate(pix_val):
	if i % 8 == 0:
		col = 0
		row = row+1
		file_write.write("\n")

	if a == 0 and b == 0 and c ==0:
		val = -1
	else:
		val = 1
	pix_arr[row][col] = val
	if row == 7 and col ==7:
		file_write.write(str(pix_arr[row][col]))
	else:
		file_write.write(str(pix_arr[row][col]).join(' ,'))

	col = col+1