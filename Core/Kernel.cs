using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System.Threading.Tasks;
using Cosmos.HAL;
using System.IO;


namespace MiniOS
{
    public class Kernel : Sys.Kernel
    {


        protected override void BeforeRun()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("MiniOS booted successfully. Type the command 'help' for commands!");
            Console.WriteLine("************************************");
            Console.WriteLine("* __  __ _       _    ____   _____ *");
            Console.WriteLine("*|  \\/  (_)     (_)  / __ \\ / ____|*");
            Console.WriteLine("*| \\  / |_ _ __  _  | |  | | (___  *");
            Console.WriteLine("*| |\\/| | | '_ \\| | | |  | |\\___ \\ *");
            Console.WriteLine("*| |  | | | | | | | | |__| |____) |*");
            Console.WriteLine("*|_|  |_|_|_| |_|_|  \\____/|_____/ *");
            Console.WriteLine("***********Copyright*2020***********");

            var fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
        }

        protected override void Run()
        {
            Console.WriteLine("/>");
            string input = Console.ReadLine();
            Command(input);
        }
        public void Command(string input)

        {
            //File Sys Commands --------------------------------------------------------------------------- File Sys Commands
            if (input == "space")
            {
                long available_space = fs.GetAvailableFreeSpace("0:/");
                Console.WriteLine("Available Free Space: " + available_space);
            }

            if (input == "file type")
            {
                string fs_type = fs.GetFileSystemType("0:/");
                Console.WriteLine("File System Type: " + fs_type);
            }

            if (input == "file list") // List files
            {
                foreach (var directoryEntry in directory_list)
                {
                    Console.WriteLine(directoryEntry.mName);
                }
            }

            var directory_list = fs.GetDirectoryListing("0:/"); // read files

            if (input == "read file")
            {
                try
                {
                    foreach (var directoryEntry in directory_list)
                    {
                        var file_stream = directoryEntry.GetFileStream();
                        var entry_type = directoryEntry.mEntryType;
                        if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                        {
                            byte[] content = new byte[file_stream.Length];
                            file_stream.Read(content, 0, (int)file_stream.Length);
                            Console.WriteLine("File name: " + directoryEntry.mName);
                            Console.WriteLine("File size: " + directoryEntry.mSize);
                            Console.WriteLine("Content: ");
                            foreach (char ch in content)
                            {
                                Console.Write(ch.ToString());
                            }
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            if (input == "write file") // write file
            {
                try
                {
                    var hello_file = fs.GetFile(@"0:\hello_from_elia.txt");
                    var hello_file_stream = hello_file.GetFileStream();

                    if (hello_file_stream.CanWrite)
                    {
                        byte[] text_to_write = Encoding.ASCII.GetBytes("Learning how to use VFS!");
                        hello_file_stream.Write(text_to_write, 0, text_to_write.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            if (input == "create file") // create file
            {
                try
                {
                    fs.CreateFile(@"0:\hello_from_elia.txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }



            //Commands --------------------------------------------------------------------------- Commands
            if (input == "about")
            {
                Console.WriteLine("**************************************************************************************");
                Console.WriteLine("*Mini OS is my personal project, built using Cosmos OS UserKit. Copyright 2020 by EM.*");
                Console.WriteLine("**************************************************************************************");
            }

            if (input == "version")
            {
                Console.WriteLine("***********************");
                Console.WriteLine("*Mini OS Version 1.0.1*");
                Console.WriteLine("***********************");

            }

            if (input == "help")
            {

                Console.WriteLine("********COMMANDS**************************");
                Console.WriteLine("* help- this menu                        *");
                Console.WriteLine("* clear- clears screen                   *");
                Console.WriteLine("* reboot- reboots the system             *");
                Console.WriteLine("* shutdown- shutdowns system             *");
                Console.WriteLine("*                                        *");
                Console.WriteLine("********READ******************************");
                Console.WriteLine("* Cosmos OS- https://github.com/CosmosOS *");
                Console.WriteLine("* about- about this operating system     *");
                Console.WriteLine("******************************************");
                Console.WriteLine("*                                        *");
                Console.WriteLine("********SYSTEM INFO***********************");
                Console.WriteLine("* version- displays version info         *");
                Console.WriteLine("******************************************");


            }

            //echo and sys power --------------------------------------------------------------------------- echo and sys power
            else if (input.StartsWith("echo"))
            {
                try
                {
                    Console.WriteLine(input.Remove(0, 5));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("echo: " + ex.Message);
                }
            }
            else if (input == "reboot")
            {
                Sys.Power.Reboot();
            }
            else if (input == "clear")
            {
                Console.Clear();
            }
            else if (input == "shutdown")
            {
                Cosmos.System.Power.Shutdown();
            }
            else
            {
                Console.WriteLine(input + " Bad Input or Command!");
            }

        }

    }
}