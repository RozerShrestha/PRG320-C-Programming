using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public interface IQRPaymentProcessor:IPaymentProcessor
    {
        void QRDecrypt();
    }
}
