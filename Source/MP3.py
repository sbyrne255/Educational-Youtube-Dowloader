from __future__ import print_function
import pafy, subprocess, os, logging, sys, uuid

def removeBadChars(x):
    return x.replace('"',"'").replace(":","").replace("?","").replace("*","").replace("/","").replace("\\","").replace("<","").replace(">","").replace("|","")
def downloadAudio(url):
    directory = os.path.expanduser('~\\Documents\\EYS\\MP3\\')
    if not os.path.exists(directory):
        os.makedirs(directory)
    logging.basicConfig(filename=directory+'Log.txt',level=logging.DEBUG)
    try:
        video = pafy.new(url)
        title = video.title
        logging.info("Downloading Video " + title)
        audiostreams = video.audiostreams
        try:
            for a in audiostreams:
                if a.extension.lower() == "m4a":
                    logging.info("Found m4a version...")
                    if os.path.isfile(directory+removeBadChars(title)+".m4a") == True:#Temp file exists (bad)...
                        logging.info("Existing temp file found, deleting...")
                        os.remove(directory+removeBadChars(title)+".m4a")#Delete temp file...
                    if os.path.isfile(directory+removeBadChars(title)+".mp3"):#MP3 file already exists, user probably downloaded it twice...
                        logging.info("Existing MP3 file found, deleting...")
                        os.remove(directory+removeBadChars(title)+".mp3")#Delete existing MP3 file...
                    tempTitle = directory+str(uuid.uuid1())+"."
                    a.download(quiet=True,filepath=tempTitle + a.extension)
                    formattedString = 'ffmpeg -i "{0}m4a" -vn -ar 44100 -ac 2 -ab 192k -f mp3 "{1}mp3"'.format(tempTitle, tempTitle.replace(".m4a",""))
                    logging.info("Using file: " + formattedString)
                    si = subprocess.STARTUPINFO()
                    si.dwFlags |= subprocess.STARTF_USESHOWWINDOW
                    convert = subprocess.call(formattedString, startupinfo=si)
                    logging.info("File cleanup started..")
                    os.rename(tempTitle+"mp3", directory+removeBadChars(title)+".mp3")
                    os.remove(tempTitle+"m4a")
        except Exception as er:
           logging.warning(er)
    except Exception as er:
       logging.warning(er)
if __name__ == '__main__':
    f = sys.argv[1]
    downloadAudio(f)
    sys.exit(0)
