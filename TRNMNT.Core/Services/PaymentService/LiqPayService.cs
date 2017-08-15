using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public class LiqPayService : IPaymentService
    {
        private string publicKey = "i15572856226";
        private string privateKey = "z7TS5ObVdlVvXo7hqlRLQNgLjDHMtuycIndFsxq9";


        public LiqPayService(IConfiguration configuration)
        {

        }


        public PaymentDataModel GetPaymentDataModel(int price)
        {
            var encodedData = GetBase64EncodedData(GetStringJsonData(price));
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

        private string GetStringJsonData(int price)
        {
            var result = new JObject();
            result["version"] = 3;
            result["public_key"] = publicKey;
            result["action"] = "pay";
            result["amount"] = price;
            result["currency"] = "UAH";
            result["description"] = "Sport Event Participation";
            result["order_id"] = Guid.NewGuid().ToString();
            result["expired_date"] = "2017-10-24 00:00:00";
            result["sandbox"] = "1";
            return result.ToString();
        }
    }
}
