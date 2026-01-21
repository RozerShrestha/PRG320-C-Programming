using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.AbstractSimple
{
    public abstract class Vehicle
    {
        // Abstract method (no body)
        public abstract void Drive();

        // Normal method (shared logic)
        public void FuelUp(string vehicleType)
        {
            Console.WriteLine($"{vehicleType} is fueled up.");
        }
    }

    // Derived class
    public class Car : Vehicle
    {
        public override void Drive()
        {
            Console.WriteLine("Car is driving on the road.");
        }
    }

    // Derived class
    public class Bike : Vehicle
    {
        public override void Drive()
        {
            Console.WriteLine("Bike is riding on the street.");
        }
    }

}
