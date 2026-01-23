using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class InvalidItemDataException:Exception
    {
        public string PropertyName { get; set; }
        public object InvalidValue { get; set; }

        //default constructor
        public InvalidItemDataException()
        {
            
        }
        //parameterized constructor
        public InvalidItemDataException(string message):base(message)
        {
            
        }
        //constructor with message and inner exception
        public InvalidItemDataException(string message, Exception innerException):base(message, innerException)
        {
            
        }

        // Constructor with message, property name, and invalid value
        public InvalidItemDataException(string message, string propertyName, object invalidValue): base(message)
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }


    }
}
