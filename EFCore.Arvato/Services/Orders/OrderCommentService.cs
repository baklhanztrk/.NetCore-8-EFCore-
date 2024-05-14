using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Repository;

namespace EFCore.Arvato.Services.Orders
{
    public class OrderCommentService : IOrderCommentService
    {
        private readonly IRepository<OrderComment> _orderCommentRepository;

        public OrderCommentService(IRepository<OrderComment> orderCommentRepository)
        {
            _orderCommentRepository= orderCommentRepository;
        }

        public void BulkInsert(List<OrderComment> entityList)
        {
            _orderCommentRepository.BulkAddAsync(entityList);
            
        }

        public void DeleteComment(OrderComment entity)
        {           

             _orderCommentRepository.Delete(entity);
        }

        public IQueryable<OrderComment> GetAllComment()
        {
            return _orderCommentRepository.Table;
        }

        public OrderComment GetCommentById(long Id)
        {
            return _orderCommentRepository.Table.Where(op=>op.Order_Id==Id).First();
        }

        public void InsertComment(OrderComment entity)
        {
            _orderCommentRepository.Insert(entity);
        }

        public void UpdateComment(OrderComment entity)
        {
            
            _orderCommentRepository.UpdateAsync(entity);
        }
    }
}
