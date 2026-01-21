using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Week1
{
    public class MethodTest
    {
        //default function implementation test
        public void FunctionTest()
        {
            Console.WriteLine("This is a test function.");
        }

        public void FunctionTest2() { Console.WriteLine("This is a test function 2."); }

        public void FunctionTest3(int a, string b, List<string> l)
        {
            Console.WriteLine($"This is a test function 3 with parameters: {a}, {b}");
        }

        public int FunctionTest4(int a, int b)
        {
            return a + b;
        }

        public List<string> FunctionTest5()
        {
            return new List<string> { "one", "two", "three" };
        }



        // 1. Original method: sum of two integers
        public int Sum(int a, int b)
        {
            return a + b;
        }

        // 2. Overload: sum of three integers
        public int Sum(int a, int b, int c)
        {
            return a + b + c;
        }

        // 3. Overload: sum of two doubles
        public double Sum(double a, double b)
        {
            return a + b;
        }

        // 4. Overload: sum of an array of integers
        public int Sum(int[] numbers)
        {
            int total = 0;
            foreach (int n in numbers)
            {
                total += n;
            }
            return total;
        }

        // 5. Overload: sum of a list of doubles
        public double Sum(List<double> numbers)
        {
            double total = 0;
            foreach (double n in numbers)
            {
                total += n;
            }
            return total;
        }

    }
}
