from __future__ import print_function
import pafy, subprocess, os, logging, sys

def removeBadChars(x):
    return x.replace('"',"'").replace(":","").replace("?","").replace("*","").replace("/","").replace("\\","").replace("<","").replace(">","").replace("|","")

def downloadVideo(url):
    directory = os.path.expanduser('~\\Documents\\EYS\\Videos\\')
    if not os.path.exists(directory):
        os.makedirs(directory)
    logging.basicConfig(filename=directory+'Log.txt',level=logging.DEBUG)
    try:
        video = pafy.new(url)
        title = video.title
        title = removeBadChars(title)
        
        quality = video.getbest(preftype="mp4")
        logging.info("Downloading Video " + title)
        if os.path.isfile(directory+title+".mp4") == True:
            logging.info("Video file already exists, attempting to delete...")
            os.remove(directory+title+".mp4")
        quality.download(quiet=True,filepath=directory+removeBadChars(title)+".mp4")
    except Exception as er:
        logging.warning(er)
if __name__ == '__main__':
    f = sys.argv[1]
    downloadVideo(f)
    sys.exit(0)
