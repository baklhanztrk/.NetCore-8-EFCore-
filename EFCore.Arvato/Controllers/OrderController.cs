using Azure.Core;
using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Core.RabbitMq;
using EFCore.Arvato.Dtos;
using EFCore.Arvato.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Xml.Linq;

namespace EFCore.Arvato.Controllers
{
    [Route("api/[controller]/[action]")]    
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IOrderCommentService _orderCommentService;
        private readonly IRabbitMQServices _rabbitMqService;
       
        
        public OrderController(
            IOrderServices orderServices,
            IOrderCommentService orderCommentService,
            IRabbitMQServices rabbitMqService
            )
        {
            _orderServices= orderServices;
            _orderCommentService = orderCommentService;
            _rabbitMqService = rabbitMqService;


        }


        [HttpGet]

        public async Task<IActionResult> InsertRabbitMqOrder()
        {
            await _rabbitMqService.ListenMessageQueue("ActivationQueue", "");
            return Ok(new { Message = "Başarılı" });

        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAll()
        {
            var record = _orderServices.GetAllOrder();



            return Ok(new { record });

        }

        #region BulkInsert
        [HttpPost]
        public async Task<IActionResult> BulkCreateOrder(List<ViewOrder> request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });

            if (request.Count > 100) return BadRequest(new { Message = "En fazla 100 sipariş toplu kaydedilebilir." });

            var listOrder = new List<Order>();
            foreach (var order in request)
            {
                Order vOrder = new Order
                {
                    OrderDate = order.OrderDate,
                    OrderId = order.OrderId,
                    OrderNumber = order.OrderNumber,
                    AccountId = order.AccountId,
                    District = order.District,
                    Carrier = order.Carrier,
                    City = order.City,
                    SalesChannel = order.SalesChannel,
                    OrderType = order.OrderType,
                    Status = order.Status,
                    UserId = order.UserId,

                };


                listOrder.Add(vOrder);
            }

            _orderServices.BulkInsertOrder(listOrder);
            //_orderCommentService.BulkInsert(listOrder.Select(op=>op.Comment).ToList());

            return Ok(new { Message = "Başarılı" });
        }

        #endregion




        [HttpPost]
        public async Task<IActionResult> CreateOrder(ViewOrder request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });
            var recordOrder = _orderServices.GetByIdOrder(request.OrderId);

            if (recordOrder is not null) return BadRequest(new { Message = "Daha önce böyle bir kayıt oluşturulmuş." });

            Order vOrder = new Order
                {
                    OrderDate = request.OrderDate,
                    OrderId = request.OrderId,
                    OrderNumber = request.OrderNumber,
                    AccountId = request.AccountId,
                    District = request.District,
                    Carrier = request.Carrier,
                    City = request.City,
                    SalesChannel = request.SalesChannel,
                    OrderType = request.OrderType,
                    Status = request.Status,
                    UserId = request.UserId,
                    
                };
            
            _orderServices.InsertOrder(vOrder);            

            return Ok(new { Message = "Başarılı" });
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrderComment(ViewOrderComment request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });
            if (request.OrderId<=0) return BadRequest(new { Message = "OrderId 0 veya 0 dan küçük olmaz" });

            var recordCommnet = _orderCommentService.GetCommentById(request.OrderId);
            if(recordCommnet is not null) return BadRequest(new { Message = "Bu siparişe Comment Daha Önce Eklenmiş." });
            OrderComment vOrderCommnet = new OrderComment
            {                
                Order_Id = request.OrderId,            
                UserId = request.UserId,
                CreatedAt=request.UserId,
                Comment = request.Comment
            };

            _orderCommentService.InsertComment(vOrderCommnet);

            return Ok(new { Message = "Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(ViewOrder request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });

            var recordOrder = _orderServices.GetByIdOrder(request.OrderId);

            //var recordCommnet = _orderCommentService.GetCommentById(recordOrder.OrderId);

            if (recordOrder is null) return BadRequest(new { Message = "Sipariş Bulunamadı." });

            #region EntityOrder
            recordOrder.Carrier = request.Carrier == null ? recordOrder.Carrier : request.Carrier;
            recordOrder.City = request.City == null ? recordOrder.City : request.City;
            recordOrder.AccountId = request.AccountId;
            recordOrder.Status = request.Status == null ? recordOrder.Status : request.Status;
            recordOrder.OrderId = request.OrderId;
            recordOrder.OrderType = request.OrderType == null ? recordOrder.OrderType : request.OrderType;
            recordOrder.OrderDate = request.OrderDate == null ? recordOrder.OrderDate : request.OrderDate;
            recordOrder.SalesChannel = request.SalesChannel == null ? recordOrder.SalesChannel : request.SalesChannel;
            recordOrder.District = request.District == null ? recordOrder.District : request.District;
            recordOrder.UpdatedAt = request.UpdatedAt == null ? recordOrder.UpdatedAt : request.UpdatedAt;
            recordOrder.UserId = request.UserId;
            #endregion


            _orderServices.UpdateOrder(recordOrder);


            return Ok(new { Message = "Sipariş Başarıyla Güncellendi." });
        }


        [HttpPut]
        public async Task<IActionResult> UpdateOrderComment(ViewOrderComment request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });

            var recordCommnet = _orderCommentService.GetCommentById(request.OrderId);

            if (recordCommnet is null) return BadRequest(new { Message = "Siparişe Ait Yorum Bulunamadı." });



            #region EntityOrder
            recordCommnet.Comment = request.Comment == null ? recordCommnet.Comment : request.Comment;
            recordCommnet.CreatedAt = request.CreatedAt == null ? recordCommnet.CreatedAt : request.CreatedAt;
            recordCommnet.UserId = request.UserId == null ? recordCommnet.UserId : request.UserId;
            _orderCommentService.UpdateComment(recordCommnet);
            #endregion


            return Ok(new { Message = "Sipariş Yorumu Güncellendi." });


        }


           

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(ViewOrder request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });

            var recordOrder = _orderServices.GetByIdOrder(request.OrderId);

            var recordCommnet = _orderCommentService.GetCommentById(request.OrderId);

            if (recordOrder is null) return BadRequest(new { Message = "Sipariş Bulunamadı." });

            _orderServices.DeleteOrder(recordOrder);
            if(recordCommnet is not null) _orderCommentService.DeleteComment(recordCommnet);


            return Ok(new { Message = "Sipariş Başarıyla Silindi." });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderComment(ViewOrderComment request)
        {
            if (request is null) return BadRequest(new { Message = "İstek boş atılamaz." });

            var recordCommnet = _orderCommentService.GetCommentById(request.OrderId);

            

            if (recordCommnet is null) return BadRequest(new { Message = "Siparişe Ait Yorum Bulunamadı." });
           
            _orderCommentService.DeleteComment(recordCommnet);


            return Ok(new { Message = "Sipariş Yorumu Başarıyla Silindi." });
        }



        







    }
}
