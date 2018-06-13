import os
import matplotlib.pyplot as plt
import pandas

allFiles = os.listdir(os.getcwd())
recur = len(allFiles)

print(os.path.realpath(allFiles[0]))

data = pandas.read_csv(allFiles[0], index_col = 0)

plot = data.plot()

plot.show()
