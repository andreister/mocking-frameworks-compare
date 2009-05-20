using System;
using System.Runtime.InteropServices;

namespace FailureDiagnostics
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

        static void Main(string[] args)
        {
            var moq = new MoqTests.FailingTests();
            var rhino = new RhinoMocksTests.FailingTests();
            var nmock2 = new NMock2Tests.FailingTests();
            var isolator = new IsolatorTests.FailingTests();
            //var stubs = new StubsTests.ShoppingCartTests();

            Console.WriteLine("Case 1. Some unexpected method gets invoked.");
            Run("Moq      ", () => moq.CallYell_Unexpected());
            Run("Rhino    ", () => rhino.CallYell_Unexpected());
            Run("NMock2   ", () => nmock2.CallYell_Unexpected());
            Run("Isolator ", () => isolator.CallYell_Unexpected());
        }

        private static void Run(string text, Action action)
        {
            const uint STD_OUTPUT_HANDLE = 0xfffffff5;
            const int green = 2;
            const int red = 12;
            const int white = 7;// 15;

            var console = GetStdHandle(STD_OUTPUT_HANDLE);

            try {
                SetConsoleTextAttribute(console, green);
                Console.WriteLine(text);
                action();
            }
            catch(Exception ex) {
                SetConsoleTextAttribute(console, red);
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
            finally {
                SetConsoleTextAttribute(console, white);
            }
        }

    }
}
