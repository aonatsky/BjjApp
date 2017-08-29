using System;
using System.Collections.Generic;
using System.Text;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public interface IPaymentService
    {
        PaymentDataModel GetPaymentDataModel(int price, string callbackUrl);
    }
}
