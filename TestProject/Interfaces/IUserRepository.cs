namespace Work.Interfaces
{
    public interface IUserRepository<T,K>
    {
        void Create(T obj);

        T? Read(K key); 
        
        bool Update(T obj);

        bool Remove(T obj);
    }
}
