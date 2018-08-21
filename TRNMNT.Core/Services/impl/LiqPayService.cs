using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Const;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class LiqPayService : IPaymentService
    {
        #region Dependencies

        // private readonly IPaidServiceFactory _paidServiceFactory;
        private readonly ILogger<LiqPayService> _logger;
        #endregion

        #region Properties

        private string publicKey = "";
        private string privateKey = "";

        #endregion

        #region Consts

        private const string PaymentStatusSuccess = "success";
        private const string PaymentStatusSandbox = "sandbox";
        private const string PaymentStatusFailed = "failure";
        private const string PaymentStatusError = "error";
        private const string PaymentStatusReversed = "reversed";
        private const string LiqPayUrl = "https://www.liqpay.ua/api/request";

        #endregion

        #region .ctor

        public LiqPayService(IConfiguration configuration, ILogger<LiqPayService> logger)
        {
            // _paidServiceFactory = paidServiceFactory;
            _logger = logger;
            publicKey = configuration["LIQPAY_PUBLICKEY"];
            privateKey = configuration["LIQPAY_PRIVATEKEY"];
        }

        #endregion

        #region Public Methods

        public async Task ConfirmPaymentAsync(PaymentDataModel dataModel)
        {
            throw new NotImplementedException();
            // if (dataModel.Signature == GetSignature(dataModel.Data))
            // {

            //     var jsonData = JObject.Parse(DecodeBase64(dataModel.Data));
            //     var paymentReference = jsonData["payment_id"].ToString();
            //     var orderId = jsonData["order_id"].ToString();
            //     var status = jsonData["status"].ToString();
            //     _logger.LogInformation($"refereance{paymentReference} orderid {orderId} status {status}");
            //     if (status == "success" || status == "sandbox")
            //     {
            //         var order = await _orderService.GetOrderAsync(Guid.Parse(orderId));
            //         if (order != null)
            //         {
            //             await _orderService.ApproveOrderAsync(order.OrderId, paymentReference);
            //             var service = _paidServiceFactory.GetService(order.OrderTypeId);
            //             await service.ApproveEntityAsync(Guid.Parse(order.Reference), order.OrderId);
            //         }
            //     }
            // }
        }

        public async Task<(string status, string paymentProviderReference)> GetPaymentStatusAsync(Guid orderId)
        {
            var jsonData = new JObject
            {
                ["version"] = 3, ["public_key"] = publicKey, ["action"] = "status", ["order_id"] = orderId.ToString(),
            };
            var encodedData = GetBase64EncodedData(jsonData.ToString());
            var encodedsignature = GetSignature(encodedData);
            var client = new HttpClient();
            var bodyParams = new Dictionary<string, string>();
            bodyParams.Add("data", encodedData);
            bodyParams.Add("signature", encodedsignature);
            var response = await client.PostAsync(LiqPayUrl, new FormUrlEncodedContent(bodyParams));
            var content = await response.Content.ReadAsStringAsync();
            dynamic jResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            if (jResponse.status == PaymentStatusSandbox || jResponse.status == PaymentStatusSuccess)
            {
                return (OrderStatus.Success, jResponse.liqpay_order_id);
            }
            if (jResponse.status == PaymentStatusReversed)
            {
                return (OrderStatus.Refund, jResponse.liqpay_order_id);
            }
            return (OrderStatus.Failed, jResponse.liqpay_order_id);
        }

        public PaymentDataModel GetPaymentDataModel(Order order, string serverUrl, string redirectUrl, List<(string target, int amount)> splitSettings)
        {
            var encodedData = EncodeData(order, serverUrl, redirectUrl);
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

        private string EncodeData(Order order, string callbackUrl, string redirectUrl)
        {
            var jsonData = new JObject
            {
                ["version"] = 3, ["public_key"] = publicKey, ["action"] = "pay", ["amount"] = order.Amount, ["currency"] = order.Currency, ["description"] = "Sport Event Participation", ["order_id"] = order.OrderId.ToString(), ["sandbox"] = "1", ["server_url"] = callbackUrl, ["result_url"] = redirectUrl
            };
            _logger.LogWarning($"payment {jsonData.ToString()}");
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