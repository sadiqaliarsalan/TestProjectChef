namespace Work.Interfaces
{
    public interface IUserManager<T,K>
    {
        void Create(T obj);

        T? Read(K key);

        List<T> ReadAll();

        void Update(T obj);

        void Remove(T obj);
    }
}
