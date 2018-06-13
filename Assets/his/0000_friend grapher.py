import matplotlib.pyplot as plt
import os
import numpy as np

L=9

# Grab a reference to all files in our working directory
allFiles = os.listdir(os.getcwd())

howMF = len(allFiles)

# we now need make an array that stores the data for all files
theFile = [None] * (howMF-1)

# break up each file by line [fileIndex][line]
for i in range(howMF-1):
    theFile[i] = np.loadtxt(allFiles[i+1],delimiter='\n',dtype=np.str)

for t in range(len(theFile)):
    # x timeline = the length of the number of lines
    x1 = [None]*(len(theFile[t])-1)

    # set the x timeline values
    for i in range(len(x1)):
        x1[i] = theFile[t][i+1].split(',')[0]

    # 
    yy = [None]*L

    for i in range(len(yy)):
        yy[i] = [None]*(len(theFile[t]))  
        
    # This is where the problem is, looping through the array theFile[NUMBER][ROW] split [Column] doesn't have a long enough length, so I need to extend the length of each array to = the total number of columns, & or add a value in those spots (0 or -1)
    for j in range(len(yy)-1):
        for i in range(len(yy[0])):
            if theFile[t][i].split(',')[j+1] is None:
                yy[j][i] = -5
            else:
                yy[j][i] = theFile[t][i].split(',')[1]
    
    x = [None]*len(x1)
    y = [None]*len(x1)

    # for j in range(L):
    for i in range(len(x)):
        x[i] = float(x1[i])
        y[i] = float(yy[0][i+1])

    #         y[j][i] = float(yy[j][i])

    plt.plot(x,y, label=yy[0][0])
    leg = plt.legend(fancybox=True)
    plt.show()

    # # 
    # for j in range(L):
    #     for i in range(len(yy[j])):
    #         yy[j][i] = theFile[t][i+1].split(',')[j]
    #         print(j,i,theFile[t][i+1])


    # for i in range(len(y)):
    #     y[i] = [None]*len(yy[i])


    # print(x)
    # print(y[0])
    # print(y[1])
    # print(y[2])
    # print(y[3])
    # print(y[4])

    # plt.plot(x,y[0],'blue', label='CS')
    # plt.plot(x,y[1],'red', label='Bio')
    # plt.plot(x,y[2],'yellow', label='Astro')
    # plt.plot(x,y[3],'green', label='VS')
    # plt.plot(x,y[4],'purple', label='Marketing')
    # plt.grid(True)
    # plt.xlabel('Day')
    # plt.ylabel('Interest')
    # plt.title("Clarissa's Major Declaration of Universe Seed " + str(theFile[t][0]) )
    # leg = plt.legend(fancybox=True)
    # plt.savefig(str(theFile[t][0]), dpi = 225, transparent=True)
    # plt.close()
