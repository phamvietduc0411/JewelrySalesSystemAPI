namespace JewelrySalesSystem.DAL.Infrastructures
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetEntityByIdAsync(int id);

        TEntity AddEntity(TEntity entity);

        void UpdateEntity(TEntity entity);

        Task<ICollection<TEntity>> GetAllEntitiesAsync();

        Task AddEntities(ICollection<TEntity> entities);
    }
}
