using System;
using System.Collections.Generic;
using System.Text;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IPaymentService
    {
        PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl);
    }
}
