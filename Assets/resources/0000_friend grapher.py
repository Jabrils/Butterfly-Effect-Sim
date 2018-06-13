import matplotlib.pyplot as plt
import os
import numpy as np

T=0

# Grab a reference to all files in our working directory
allFiles = os.listdir(os.getcwd())

howMF = len(allFiles)

# initilize a new array that will store the full path to file
#fullFiles = [None]*howMF

# set this new array to store all of the full paths
#for i in range(howMF):
#    fullFiles[i] = os.path.abspath(allFiles[i])

# we now need make an array that stores the data for all files
theFile = [None] * (howMF-1)

# break up each file by line [fileIndex][line]
for i in range(howMF-1):
    theFile[i] = np.loadtxt(allFiles[i+1],delimiter='\n',dtype=np.str)

for t in range(len(theFile)):
    # x timeline = the length of the number of lines
    x1 = [None]*(len(theFile[t])-2)

    # set the x timeline values
    for i in range(len(x1)):
        x1[i] = theFile[t][i+2].split(',')[0]

    #
    yy = [None]*5

    for i in range(len(yy)):
        yy[i] = [None]*(len(theFile[t])-2)

    #
    for j in range(5):
        for i in range(len(yy[j])):
            yy[j][i] = theFile[t][i+2].split(',')[j+1]

    x = [None]*len(x1)
    y = [None]*5

    for i in range(len(y)):
        y[i] = [None]*len(yy[i])

    for j in range(5):
        for i in range(len(x)):
            x[i] = float(x1[i])
            y[j][i] = float(yy[j][i])

    print(x)
    print(y[0])
    print(y[1])
    print(y[2])
    print(y[3])
    print(y[4])

    plt.plot(x,y[0],'blue', label='CS')
    plt.plot(x,y[1],'red', label='Bio')
    plt.plot(x,y[2],'yellow', label='Astro')
    plt.plot(x,y[3],'green', label='VS')
    plt.plot(x,y[4],'purple', label='Marketing')
    plt.grid(True)
    plt.xlabel('Day')
    plt.ylabel('Interest')
    plt.title("Clarissa's Major Declaration of Universe Seed " + str(theFile[t][0]) )
    leg = plt.legend(fancybox=True)
    plt.savefig(str(theFile[t][0]), dpi = 225, transparent=True)
    plt.close()