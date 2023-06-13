namespace Student.DataAccess
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        void DeleteAll();
        void DeleteAll(string table);
        List<T> GetAll();
        T GetById(int id);
        void DeleteById(int id);
       
        void Editing(T entity);
    }
}
