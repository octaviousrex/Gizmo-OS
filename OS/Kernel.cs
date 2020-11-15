using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;

namespace OS
{
    public class Kernel : Sys.Kernel
    {
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = "0:\\";

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

        E:
            Console.WriteLine("MiniOS booted successfully. Enter password: ");
            string password = "1234";
            string input = Console.ReadLine();
            if (input == password)
            {
                Console.Clear();
                Run();
            }
            else
            {
                Console.WriteLine("WRONG PASSWORD. TRY AGAIN!");
                goto E;
            }
        }

        protected override void Run()
        {
            Console.WriteLine("/>");
            string input = Console.ReadLine();
            Command(input);

            Console.WriteLine("Welcome to Mini OS! Type the command 'help' for commands!");
            Console.WriteLine("************************************");
            Console.WriteLine("* __  __ _       _    ____   _____ *");
            Console.WriteLine("*|  \\/  (_)     (_)  / __ \\ / ____|*");
            Console.WriteLine("*| \\  / |_ _ __  _  | |  | | (___  *");
            Console.WriteLine("*| |\\/| | | '_ \\| | | |  | |\\___ \\ *");
            Console.WriteLine("*| |  | | | | | | | | |__| |____) |*");
            Console.WriteLine("*|_|  |_|_|_| |_|_|  \\____/|_____/ *");
            Console.WriteLine("***********Copyright*2020***********");
        }
        public void Command(string input)

        {


            if (input == "version")
            {
                Console.WriteLine("***********************");
                Console.WriteLine("*Mini OS Version 0.0.1*");
                Console.WriteLine("***********************");

                if (input == "about")
                {
                    Console.WriteLine("**************************************************************************************");
                    Console.WriteLine("*Mini OS is my personal project, built using Cosmos OS UserKit. Copyright 2020 by EM.*");
                    Console.WriteLine("**************************************************************************************");
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
                else
                {
                    Console.WriteLine(input + " Bad Input or Command! try 'help' for a list of commands");
                }

            }

        }
    }

}
