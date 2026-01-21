using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstract
{
    public interface IQRPaymentProcessor:IPaymentProcessor
    {
        void QRDecrypt();
    }
}
