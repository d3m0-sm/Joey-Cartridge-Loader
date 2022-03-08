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
            string[] config = { };
            string root_dir = "";
            string[] fs = {};
            bool check = false;
            string[] debug = { }; 

            if (!File.Exists("loader.conf"))
            {
                File.WriteAllText("loader.conf", "letter=D\nignore_checksum=false");
            }

            config = File.ReadAllLines("loader.conf");

            foreach (string line in config)
            {
                if (line.StartsWith("letter="))
                {
                    letter = line.Replace("letter=", "");
                }
                else if (line.StartsWith("ignore_checksum="))
                {
                    ignore_checksum = line.Replace("ignore_checksum=", "");
                }
            }

            if (letter == "" || ignore_checksum == "")
            {
                Console.WriteLine("Error: Corrupted config file, please correct or delete loader.conf to restore defaults");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (Directory.Exists($"{letter}:\\"))
            {
                root_dir = $"{letter}:\\";
                fs = Directory.GetFiles(root_dir, "*.GB?");
                if (File.Exists(root_dir + "FIRMWARE.JR") && fs[0].StartsWith(root_dir + "ROM") == false)
                {
                    check = true;
                    if (ignore_checksum == "false" && File.Exists(root_dir + "DEBUG.TXT"))
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
                                    Console.ReadKey();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Something went wrong. Couldn't find DEBUG.TXT on Joey Jr.");
                        Console.WriteLine("\nNote: you can ignore the checksum by setting the option in loader.conf to true");
                        Console.ReadKey();
                    }
                }
                else if (File.Exists(root_dir + "FIRMWARE.JR") == false)
                {
                    Console.WriteLine($"Error: The device with the drive letter {letter} isn't a Joey jr.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Error: Couldn't load ROM");
                    Console.WriteLine("Please cleanup your cartridge a nd reinsert it");
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
