using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;
using TRNMNT.Core.Helpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TRNMNT.Core.Services
{
    public class LiqPayService : IPaymentService
    {
        private string publicKey = "i39927249084";
        private string privateKey = "LRGmWbN29X05TT0M8j0sSbWgw2UoY1K9uWnp4Nqy";
        private IConfiguration configuration;
        private readonly IPaidServiceFactory paidServiceFactory;
        private IParticipantService participantService;
        private ITeamService teamService;
        private IOrderService orderService;
        private readonly ILogger logger;

        public LiqPayService(IConfiguration configuration, IPaidServiceFactory paidServiceFactory, IOrderService orderService, ILogger logger)
        {
            this.configuration = configuration;
            this.paidServiceFactory = paidServiceFactory;
            this.orderService = orderService;
            this.logger = logger;
        }

        public async Task ConfirmPaymentAsync(PaymentDataModel dataModel)
        {
            if (dataModel.Signature == GetSignature(dataModel.Data))
            {
                try
                {
                    JObject jsonData = JObject.Parse(DecodeBase64(dataModel.Data));
                    var paymentReference = jsonData["payment_id"].ToString();
                    var orderId = jsonData["order_id"].ToString();
                    var status = jsonData["status"].ToString();
                    logger.LogDebug($"paymentId:{paymentReference}, orderId:{orderId}, status:{status}");

                    if (status == "success" || status == "sandbox")
                    {
                        var order = await orderService.GetOrder(Guid.Parse(orderId));
                        if (order != null)
                        {
                            await orderService.ApproveOrderAsync(order.OrderId, paymentReference);
                            var service = paidServiceFactory.GetService(order.OrderTypeId);
                            await service.ApproveEntityAsync(Guid.Parse(order.Reference), order.OrderId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }

            }
            logger.LogError("Signature declined");
        }

        public PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl)
        {
            var encodedData = EncodeData(order, callbackUrl);
            var encodedsignature = GetSignature(encodedData);
            return new PaymentDataModel()
            {
                Data = encodedData,
                Signature = encodedsignature
            };
        }


        private string GetSignature(string encodedData)
        {
            var stringSignature = String.Concat(privateKey, encodedData, privateKey);
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(stringSignature)));
        }

        private string EncodeData(Order order, string callbackUrl)
        {
            var jsonData = new JObject();
            jsonData["version"] = 3;
            jsonData["public_key"] = publicKey;
            jsonData["action"] = "pay";
            jsonData["amount"] = order.Amount;
            jsonData["currency"] = order.Currency;
            jsonData["description"] = "Sport Event Participation";
            jsonData["order_id"] = order.OrderId.ToString();
            jsonData["expired_date"] = "2017-10-24 00:00:00";
            jsonData["sandbox"] = "1";
            jsonData["result_url"] = callbackUrl;
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

    }
}
