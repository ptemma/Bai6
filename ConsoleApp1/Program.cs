using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string PATH = @"D:\b1909971";

            Console.WriteLine(Path.Combine(PATH, "kakak"+".txt"));
            Console.ReadLine();
        }
    }
}
