using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    class LiqPayService : IPaymentService<LiqPayRequestModel>
    {
        private string publicKey = "i15572856226";
        private string privateKey = "z7TS5ObVdlVvXo7hqlRLQNgLjDHMtuycIndFsxq9";
        private SHA1 sha1Encoder = SHA1.Create();

        public LiqPayService(IConfiguration configuration) 
        {

        }


        public LiqPayRequestModel GetPaymentRequestModel()
        {
            var encodedData = GetBase64EncodedData(GetStringJsonData());
            var stringSignature = String.Concat(privateKey, encodedData, privateKey);
            var encodedsignature = Convert.ToBase64String(sha1Encoder.ComputeHash(Encoding.Unicode.GetBytes(stringSignature)));
            return new LiqPayRequestModel()
            {
                Data = encodedData,
                Signature = encodedsignature
            };
        }


        private string GetBase64EncodedData(string data)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(data));
        }

        private string GetStringJsonData()
        {
            var result = new JObject();
            result["version"] = 3;
            result["public_key"] = publicKey;
            result["action"] = "pay";
            result["amount"] = 10;
            result["currency"] = "UAH";
            result["description"] = "Sport Event Participation";
            result["order_id"] = Guid.NewGuid().ToString();
            result["expired_date"] = "2017-10-24 00:00:00";
            result["sandbox"] = "1";
            return result.ToString();
        }
    }
}
