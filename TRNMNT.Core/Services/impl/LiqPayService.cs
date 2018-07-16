using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class LiqPayService : IPaymentService
    {
        #region Dependencies

        private readonly IPaidServiceFactory _paidServiceFactory;
        private readonly IOrderService _orderService;
        #endregion

        #region Properties

        private string publicKey = "";
        private string privateKey = "";

        #endregion

        #region .ctor

        public LiqPayService(IPaidServiceFactory paidServiceFactory, IOrderService orderService, IConfiguration configuration)
        {
            _paidServiceFactory = paidServiceFactory;
            _orderService = orderService;
            publicKey = configuration["LiqPay:PublicKey"];
            privateKey = configuration["LiqPay:PrivateKey"];
        }

        #endregion

        #region Public Methods

        public async Task ConfirmPaymentAsync(PaymentDataModel dataModel)
        {
            if (dataModel.Signature == GetSignature(dataModel.Data))
            {

                var jsonData = JObject.Parse(DecodeBase64(dataModel.Data));
                var paymentReference = jsonData["payment_id"].ToString();
                var orderId = jsonData["order_id"].ToString();
                var status = jsonData["status"].ToString();
                //_logger.LogDebug($"paymentId:{paymentReference}, orderId:{orderId}, status:{status}");

                if (status == "success" || status == "sandbox")
                {
                    var order = await _orderService.GetOrderAsync(Guid.Parse(orderId));
                    if (order != null)
                    {
                        await _orderService.ApproveOrderAsync(order.OrderId, paymentReference);
                        var service = _paidServiceFactory.GetService(order.OrderTypeId);
                        await service.ApproveEntityAsync(Guid.Parse(order.Reference), order.OrderId);
                    }
                }
            }
        }

        public PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl)
        {
            var encodedData = EncodeData(order, callbackUrl);
            var encodedsignature = GetSignature(encodedData);
            return new PaymentDataModel
            {
                Data = encodedData,
                    Signature = encodedsignature
            };
        }

        #endregion

        #region Private Methods

        private string GetSignature(string encodedData)
        {
            var stringSignature = String.Concat(privateKey, encodedData, privateKey);
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(stringSignature)));
        }

        private string EncodeData(Order order, string callbackUrl)
        {
            var jsonData = new JObject
            {
                ["version"] = 3, ["public_key"] = publicKey, ["action"] = "pay", ["amount"] = order.Amount, ["currency"] = order.Currency, ["description"] = "Sport Event Participation", ["order_id"] = order.OrderId.ToString(), ["expired_date"] = "2017-10-24 00:00:00", ["sandbox"] = "1", ["result_url"] = callbackUrl
            };
            return GetBase64EncodedData(jsonData.ToString());
        }

        private string GetBase64EncodedData(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        private string DecodeBase64(string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        #endregion
    }
}