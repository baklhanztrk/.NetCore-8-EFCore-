namespace EFCore.Arvato.Core.Orders
{
    public interface IOrderServices
    {
        void BulkInsertOrder(List<Order> entityList);

        void InsertOrder(Order entity);

        Order GetByIdOrder(long Id);
        IQueryable<OrderDetail> GetAllOrder();      

        void UpdateOrder(Order entity);

        void DeleteOrder(Order entity);

    }
}
