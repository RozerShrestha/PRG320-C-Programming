using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1
{
    public class Fruit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        //default constructor
        public Fruit()
        {
            
        }

        //parameterized constructor
        public Fruit(int id, string name,double Price)
        {
            Id = id;
            Name = name; 
            this.Price = Price;
        }

        public Fruit(string name, double Price)
        {
            Name = name;
            this.Price = Price;
        }

    }

}
