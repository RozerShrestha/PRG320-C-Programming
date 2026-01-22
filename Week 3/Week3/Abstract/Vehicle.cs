using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.AbstractSimple
{
    //Abstract class cannot be instantiated directly.
    //It can have abstract methods (without body) and normal methods (with body).
    //Abstract methods must be implemented by derived classes.
    //Normal methods can provide shared logic for derived classes.
    //Abstract class serve as a bluprint for other classes.
    //It can contain:
    //Abstract methods(no implementation, must be overridden in derived classes)
    //Concrete methods(with implementation, can be reuse by derived classes)
    //

    public abstract class Vehicle
    {
        // Abstract method (no body)
        public abstract void Drive();

        // virtual method can be overridden in derived classes if needed
        public virtual void FuelUp(string vehicleType)
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

        public override void FuelUp(string vehicleType)
        {
            Console.WriteLine($"{vehicleType} is being refueled with a bike pump.");
        }
    }

}
