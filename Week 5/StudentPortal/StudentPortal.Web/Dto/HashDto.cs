using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortal.Web.Services.Dto
{
    //These are the properties that will be returned to the client when the user logs in successfully.
    //The hash and salt will be used to verify the user's password when they log in again.
    public class HashDto
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
