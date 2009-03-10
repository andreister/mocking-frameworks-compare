using System;

namespace Performance
{
    class Program
    {
        static int dummy;

        static void Main(string[] args)
        {
            // Measure how long it takes to fetch all the keys exactly once
            var timer = new MultiSampleCodeTimer(10, 100);
            Console.WriteLine("Data units of msec resolution = " + MultiSampleCodeTimer.ResolutionUsec.ToString("f6") + " usec");
            //timer.OnMeasure = MultiSampleCodeTimer.PrintStats; // Uncomment if you want to see detailed stats.

            var moq = new MoqTests.ShoppingCartTests();
            var rhino = new RhinoMocksTests.ShoppingCartTests();
            var nmock2 = new NMock2Tests.ShoppingCartTests();
            var isolator = new IsolatorTests.ShoppingCartTests();
            int result = 0;

            Console.WriteLine("\nMocking methods.");
            timer.Measure("Moq      ", () => { moq.Test1_MockedMethod(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test1_MockedMethod(); result = 2; });
            timer.Measure("NMock2   ", () => { nmock2.Test1_MockedMethod(); result = 3; });
            timer.Measure("Isolator ", () => { isolator.Test1_MockedMethod(); result = 4; });

            Console.WriteLine("\nMocking events.");
            timer.Measure("Moq      ", () => { moq.Test2_MockedEvent(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test2_MockedEvent(); result = 2; });
            timer.Measure("NMock2   ", () => { nmock2.Test2_MockedEvent(); result = 3; });
            timer.Measure("Isolator ", () => { isolator.Test2_MockedEvent(); result = 4; });

            Console.WriteLine("\nMocking properties.");
            timer.Measure("Moq      ", () => { moq.Test3_MockedProperty(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test3_MockedProperty(); result = 2; });
            timer.Measure("NMock2   ", () => { nmock2.Test3_MockedProperty(); result = 3; });
            timer.Measure("Isolator ", () => { isolator.Test3_MockedProperty(); result = 4; });

            Console.WriteLine("\nMocking method arguments.");
            timer.Measure("Moq      ", () => { moq.Test4_MockedArgument(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test4_MockedArgument(); result = 2; });
            timer.Measure("NMock2   ", () => { nmock2.Test4_MockedArgument(); result = 3; });
            timer.Measure("Isolator ", () => { isolator.Test4_MockedArgument(); result = 4; });

            Console.WriteLine("\nPartial mocks.");
            timer.Measure("Moq      ", () => { moq.Test5_PartialMocks(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test5_PartialMocks(); result = 2; });
            timer.Measure("Isolator ", () => { isolator.Test5_PartialMocks(); result = 3; });

            Console.WriteLine("\nRecursive mocks.");
            timer.Measure("Moq      ", () => { moq.Test6_RecursiveMocks(); result = 1; });
            timer.Measure("Rhino    ", () => { rhino.Test6_RecursiveMocks(); result = 2; });
            timer.Measure("Isolator ", () => { isolator.Test6_RecursiveMocks(); result = 3; });

            dummy = result;
        }
    }
}
