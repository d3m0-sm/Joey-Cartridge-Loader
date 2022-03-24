using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Joey_Cartridge_loader
{
    class Program
    {
        static void Main(string[] args)
        {
            string letter = "";
            string ignore_checksum = "";
            string ignore_rom_name = "";
            string[] config = { };
            string root_dir = "";
            string[] fs = {};
            bool check = false;
            string[] debug = { }; 

            if (!File.Exists("loader.conf"))
            {
                File.WriteAllText("loader.conf", "letter=D\nignore_checksum=false\nignore_rom_name=false");
            }

            config = File.ReadAllLines("loader.conf");

            foreach (string line in config)
            {
                string[] cfg = line.Split('=');
                string arg = cfg[0];
                string param = cfg[1];
                if (arg == "letter")
                {
                    letter = param;
                }
                else if (arg == "ignore_checksum")
                {
                    ignore_checksum = param;
                }
                else if (arg == "ignore_rom_name")
                {
                    ignore_rom_name = param;
                }
            }

            if (letter == "" || ignore_checksum == "")
            {
                Console.WriteLine("Error: Corrupted config file, please correct or delete loader.conf to restore defaults");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (letter == "auto")
            {
                Console.WriteLine("Error: letter type auto is not supported on Windows");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (Directory.Exists($"{letter}:\\"))
            {
                root_dir = $"{letter}:\\";
                fs = Directory.GetFiles(root_dir, "*.GB?");
                if (ignore_rom_name == "false")
                {
                    if (fs[0].StartsWith(root_dir + "ROM"))
                    {
                        Console.WriteLine("Error: Couldn't load ROM");
                        Console.WriteLine("Please cleanup your cartridge and reinsert it");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                if (File.Exists(root_dir + "FIRMWARE.JR") && File.Exists(root_dir + "DEBUG.TXT"))
                {
                    check = true;
                    if (ignore_checksum == "false")
                    {
                        debug = File.ReadAllLines(root_dir + "DEBUG.TXT");
                        foreach (string line in debug)
                        {
                            if (line.StartsWith("Checksum"))
                            {
                                string checksum = line.Replace("Checksum ", "");
                                if (checksum != "Correct" )
                                {
                                    Console.WriteLine("Error: Checksum is incorrect!");
                                    Console.WriteLine("Please cleanup your cartridge and reinsert it");
                                    Console.ReadKey();
                                    Environment.Exit(0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error: The device with the drive letter {letter} isn't a Joey jr.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"Error: No device connected with the accociated drive letter: {letter}");
                Console.ReadKey();
            }

            if (check == true)
            {
                Process.Start(fs[0]);
            }
        }
    }
}
