using System;
using System.Collections.Generic;
using System.Text;

namespace TRNMNT.Core.Services
{
    interface IPaymentService<T> where T: class
    {
        T GetPaymentRequestModel();
    }
}
