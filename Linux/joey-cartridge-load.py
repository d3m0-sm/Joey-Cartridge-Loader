import os
from os import popen as sh
from glob import glob


config = {
        
        "letter": "auto",
        "ignore_checksum": "false",
        "ignore_rom_name": "false"
        }

path = ""

def getJoeyPath(grep: str):
    cm  = sh(f"cat /proc/mounts | grep -i {grep}")
    resp = cm.read()
    if resp == "":
        return
    mount = resp.split(',')[0].split(' ')
    return mount[1]

def getGui():
    gui = ["gnome", "kde"]
    for g in gui:
        cm = sh(f"w -sh | grep -i {g} | wc -l")
        resp = int(cm.read().replace("\n", ""))-1
        if resp == 1:
            return g
    return None


if os.path.exists("loader.conf") == False:
    cfg = open("loader.conf", "w")
    for k in config.keys():
        cfg.write(f"{k}={config[k]}")
        cfg.write("\n")
    cfg.close()

cfg = open("loader.conf", "r")
line = cfg.readlines()
cfg.close()
for l in line:
    s = l.split('=')
    if s[0] == "\n":
        continue
    if s[0] not in config.keys() or (s[1] == "\n" or s[1] == ""):
        print("Error: Corrupted config file, please correct or delete loader.conf to restore defaults")
        exit()
    config[s[0]] = s[1].replace("\n", "")

if config["letter"] == "auto":
    path = getJoeyPath("bennvenn")
else:
    part = f"/dev/{config['letter']}"
    path = getJoeyPath(part)
if path == None:
    print("Couldn't find target device!")
    exit()
if os.path.exists(f"{path}/FIRMWARE.JR") == False or os.path.exists(f"{path}/DEBUG.TXT") == False:
    print("The target devise ins't a Joey Jr.")
    exit()
else:
    romList = glob(f'{path}/*.GB?')
    if len(romList) >= 1:
        if config["ignore_rom_name"] == "false":
            if romList[0].startswith(f"{path}/ROM.GB"):
                print("Error: Couldn't load ROM")
                print("Please cleanup your cartridge and reinsert it")
                exit()
        if config["ignore_checksum"] == "false":
            debug = open(f"{path}/DEBUG.TXT", "r")
            line = debug.readlines()
            debug.close()
            for l in line:
                if "Checksum" in l:
                    checksum = l.replace("Checksum ", "").replace("\n", "")
                    if checksum != "Correct":
                        print("Error: Checksum is incorrect!")
                        print("Please cleanup your cartridge and reinsert it")
                        exit()
        gui = getGui()
        if gui == "kde":
            sh(f"kde-open5 {romList[0]}")
        elif gui == "gnome":
            sh(f"kde-gnome {romList[0]}")
    else:
        print("Error: Couldn't find rom")

            





