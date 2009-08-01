using System;
using System.Runtime.InteropServices;

namespace FailureDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            var moq = new MoqTests.FailingTests();
            var rhino = new RhinoMocksTests.FailingTests();
            var nmock2 = new NMock2Tests.FailingTests();
            var isolator = new IsolatorTests.FailingTests();
            var stubs = new StubsTests.FailingTests();

            Console.WriteLine("Case 1. Some unexpected method gets invoked.");
            Run("Moq      ", moq.CallOnceExpectNever);
            Run("Rhino    ", rhino.CallOnceExpectNever);
            Run("NMock2   ", nmock2.CallOnceExpectNever);
            Run("Isolator ", isolator.CallOnceExpectNever);
            Run("Stubs    ", stubs.CallOnceExpectNever);
            Console.WriteLine("Press any key to continue..."); Console.ReadLine();

            Console.WriteLine("Case 2. Some expected method is not invoked.");
            Run("Moq      ", moq.CallNeverExpectOnce);
            Run("Rhino    ", rhino.CallNeverExpectOnce);
            Run("NMock2   ", nmock2.CallNeverExpectOnce);
            Run("Isolator ", isolator.CallNeverExpectOnce);
            Run("Stubs    ", stubs.CallNeverExpectOnce);
            Console.WriteLine("Press any key to continue..."); Console.ReadLine();

            Console.WriteLine("Case 3. Some expected method is not invoked - custom message.");
            Run("Moq      ", moq.FailWithCustomMessage);
            Run("Rhino    ", rhino.FailWithCustomMessage);
            Run("NMock2   ", nmock2.FailWithCustomMessage);
            Run("Isolator ", isolator.FailWithCustomMessage);
            Run("Stubs    ", stubs.FailWithCustomMessage);
            Console.WriteLine("Press any key to continue..."); Console.ReadLine();

            Console.WriteLine("Case 4. Expected method is invoked but parameters were incorrect.");
            Run("Moq      ", moq.CallExpectedWithWrongParameters);
            Run("Rhino    ", rhino.CallExpectedWithWrongParameters);
            Run("NMock2   ", nmock2.CallExpectedWithWrongParameters);
            Run("Isolator ", isolator.CallExpectedWithWrongParameters);
            Run("Stubs    ", stubs.CallExpectedWithWrongParameters);
        }

        #region Console colored output

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetStdHandle(uint nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetConsoleScreenBufferInfo(int hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);
        
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTextAttribute", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleTextAttribute(int hConsoleOutput, int wAttributes);
        
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD { short X; short Y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT { short Left; short Top; short Right; short Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        private static void Run(string text, Action action)
        {
            const uint stdOutputHandle = 0xfffffff5;
            const int green = 2;
            const int red = 12;

            var console = GetStdHandle(stdOutputHandle);
            int originalColor = GetOriginalColor(console);
            
            try {
                SetConsoleTextAttribute(console, green);
                Console.WriteLine(text);
                action();
            }
            catch (Exception ex) {
                SetConsoleTextAttribute(console, red);
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
            finally {
                SetConsoleTextAttribute(console, originalColor);
            }
        }
 
        private static int GetOriginalColor(int hConsole)
        {
            var consoleInfo = new CONSOLE_SCREEN_BUFFER_INFO();
            GetConsoleScreenBufferInfo(hConsole, ref consoleInfo);
            return consoleInfo.wAttributes;
        }

        #endregion
    }
}
