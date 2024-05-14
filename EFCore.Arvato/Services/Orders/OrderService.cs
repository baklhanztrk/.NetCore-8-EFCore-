using EFCore.Arvato.Context;
using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Repository;
using System.Linq;
using Order = EFCore.Arvato.Core.Orders.Order;


namespace EFCore.Arvato.Services.Orders
{
    public class OrderService : IOrderServices
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderComment> _orderCommentRepository;

        public OrderService(IRepository<Order> orderRepository, IRepository<OrderComment> orderCommentRepository)
        {
            _orderRepository = orderRepository;

            _orderCommentRepository = orderCommentRepository;

        }
        public void BulkInsertOrder(List<Order> orderList)
        {
            
            _orderRepository.BulkAddAsync(orderList);   
            
           
        }


        public void InsertOrder(Order entity)
        {
            _orderRepository.AddAsync(entity);

          
        }

        public Order GetByIdOrder(long Id)
        {
            var record = _orderRepository.Table.Where(op => op.OrderId == Id).FirstOrDefault();

            if(record is not null) return record;

            return record;
        }


        public IQueryable<OrderDetail> GetAllOrder()
        {
            var query = from o in _orderRepository.TableNoTracking
                        from oc in _orderCommentRepository.TableNoTracking
                        where o.OrderId == oc.Order_Id
                        select new OrderDetail
                        {
                            Detail = new Order
                            {
                                AccountId = o.AccountId,
                                UserId = o.UserId,
                                OrderId = o.OrderId,
                                OrderNumber = o.OrderNumber,
                                OrderType = o.OrderType,
                                OrderDate = o.OrderDate,
                                Status = o.Status,
                                Carrier = o.Carrier,
                                City = o.City,
                                SalesChannel = o.SalesChannel,


                            },
                            OrderCommnetDetail = new OrderComment
                            {
                                Comment = oc.Comment,
                                UserId = oc.UserId,
                                CreatedAt = oc.CreatedAt,
                                Order_Id = oc.Order_Id,
                            }

                        };

            return query.Distinct().OrderBy(k=>k.Detail.Id);

            
        }

        public void DeleteOrder(Order order)
        {

            
            var result = _orderRepository.Table.Where(op=>op.OrderId==order.OrderId);

            if (result is not null) _orderRepository.Delete(order);
        }

        public Object FindOrder(Order order)
        {
            var result = _orderRepository.GetByIdAsync(order.OrderId).Result;

            if (result is not null) return result;

            return  new { Message = "Sipariş Bulunamadı" };
        }

        public  void UpdateOrder(Order entity)
        {

       
            _orderRepository.UpdateAsync(entity);
            
        }


        
    }
}
