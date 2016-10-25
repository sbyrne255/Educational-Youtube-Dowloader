from __future__ import print_function
import pafy, subprocess, os, logging, sys

def downloadAudio(url):
    directory = os.path.expanduser('~\\Documents\\EYS\\')
    if not os.path.exists(directory):
        os.makedirs(directory)
    video = pafy.new(url)
    title = video.title
    f = open(directory+'temp','w')
    f.write(title) # python will convert \n to os.linesep
    f.close() # you can omit in most cases as the destructor will call it
if __name__ == '__main__':
    f = sys.argv[1]
    downloadAudio(f)
    sys.exit(0)
