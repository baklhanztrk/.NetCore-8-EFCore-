namespace EFCore.Arvato.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long Id);
        T GetById(object id);



        void BulkAddAsync(IEnumerable<T> entity);
        void AddAsync(T entity);
        void AddListAdd(List<T> entity);
        T InsertAndGetSet(T entity);
        void Insert(T entity);





        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        Task DeleteAsync(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Update(IEnumerable<T> entities);
        void UpdateAsync(T entity);
    }
}
