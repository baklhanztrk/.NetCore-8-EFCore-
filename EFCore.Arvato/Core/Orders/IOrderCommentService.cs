namespace EFCore.Arvato.Core.Orders
{
    public interface IOrderCommentService
    {
        void InsertComment(OrderComment entity);
        OrderComment GetCommentById(long orderId);
        IQueryable<OrderComment> GetAllComment();

        void BulkInsert(List<OrderComment> entityList);

        void UpdateComment(OrderComment entity);
        void DeleteComment(OrderComment entity);
    }
}
