import os
import numpy as np

allFiles = os.listdir(os.getcwd())
recur = len(allFiles)

print(recur)

data = np.genfromtxt(allFiles[0], delimiter=',')
