namespace GameRegistrationNETApp.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Delete(int id);
        void Update(int id, T entity);
    }
}