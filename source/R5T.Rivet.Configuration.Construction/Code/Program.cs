using System;

namespace R5T.Rivet.Configuration.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Construction.SubMain();

            //Program.SubMain();
        }

        private static void SubMain()
        {
            Program.HelloWorld();
        }

        private static void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
