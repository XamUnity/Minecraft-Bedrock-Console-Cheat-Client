using System;
using Memory;
using System.Threading;
using MemoryEditing;


namespace MemoryEditing
{
    class Hooking
    {
        private string SpeedAddr = "Minecraft.Windows.exe+041942E8,128,240,D8,E8,DBC";
        private string ReachAddr = "Minecraft.Windows.exe+32027E0";

        public void write_speed(Mem mem, string value)
        {
            mem.WriteMemory(SpeedAddr, "float", value);
        }
        public void write_reach(Mem mem, string value)
        {
            mem.WriteMemory(ReachAddr, "float", value);
        }
            mem.WriteMemory(SpeedAddr, "float", "0.1000000015");
            mem.WriteMemory(ReachAddr, "float", "3.0000000");
        }
    }
   
}

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Mem mem = new Mem();
            Console.ForegroundColor = ConsoleColor.Green;

            int PID = mem.GetProcIdFromName("Minecraft.Windows.exe");
            if (PID > 0)
            {
                mem.OpenProcess(PID);
                Console.WriteLine($"PID: {PID}");
                Run(mem);
            }
            else
            {
                Console.WriteLine("You do not have Minecraft opened.");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

        }
        static void Run(Mem mem)
        {
            Hooking hook = new Hooking();

            while (true)
            {
                string output = "cmd>";
                Console.Write(output);
                string input = Console.ReadLine();
                input = input.ToLower();
                if (input == "help")
                {
                    Console.WriteLine("Reach [Value]\nSpeed [Value]");
                }
                else if (input == "clear" || input == "close" || input == "cls")
                {
                    Console.Clear();
                }
                else if (input.Contains("reach "))
                {
                    string value = input.Substring(6);
                    hook.write_reach(mem, value);
                }
                else if (input.Contains("speed "))
                {
                    string value = input.Substring(6);
                    hook.write_speed(mem, value);
                }
                else if (input.Contains("revert"))
                {
                    hook.revert(mem);
                }
                Thread.Sleep(2);
            }
        }
    }
}
