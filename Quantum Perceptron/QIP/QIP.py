# encoding = utf8
from PIL import Image
import array as arr

im = Image.open('QCImage2.png', 'r')
width, height = im.size
print(width)
print(height)
pix_val = list(im.getdata())
r, c = height, width
pix_arr = [[0 for x in range(r)] for y in range(c)]

file_write  = open('..\PQC\pixels.txt', 'w')
file_write.write(str(height))

col = 0
row = -1

for i, (a, b, c) in enumerate(pix_val):
	if i % width == 0:
		col = 0
		row = row+1
		file_write.write("\n")
	
	# Default binarized value
	val = -1

	if a == 255 and b == 255 and c == 255:
		val = 1

	pix_arr[row][col] = val

	if col == width-1:
		file_write.write(str(pix_arr[row][col]))
	else:
		file_write.write(str(pix_arr[row][col]).join(' ,'))

	col = col+1