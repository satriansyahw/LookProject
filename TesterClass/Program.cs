using System;

namespace TesterClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestDtMember testDt = new TestDtMember();
            testDt.TestSave().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
