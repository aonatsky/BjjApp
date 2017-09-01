using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public class LiqPayService : IPaymentService
    {
        private string publicKey = "i15572856226";
        private string privateKey = "z7TS5ObVdlVvXo7hqlRLQNgLjDHMtuycIndFsxq9";
        private IConfiguration configuration;

        public LiqPayService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl)
        {
            var encodedData = GetBase64EncodedData(GetStringJsonData(order, callbackUrl));
            var stringSignature = String.Concat(privateKey, encodedData, privateKey);
            var encodedsignature = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(stringSignature)));
            return new PaymentDataModel()
            {
                Data = encodedData,
                Signature = encodedsignature
            };
        }


        private string GetBase64EncodedData(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        private string GetStringJsonData(Order order, string callbackUrl)
        {
            var result = new JObject();
            result["version"] = 3;
            result["public_key"] = publicKey;
            result["action"] = "pay";
            result["amount"] = order.Amount;
            result["currency"] = order.Currency;
            result["description"] = "Sport Event Participation";
            result["order_id"] = order.OrderId.ToString();
            result["expired_date"] = "2017-10-24 00:00:00";
            result["sandbox"] = "1";
            result["result_url"] = callbackUrl;
            return result.ToString();
        }
    }
}
