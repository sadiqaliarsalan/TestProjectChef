using Work.Models;

namespace Work.Interfaces
{
    public interface IUserData
    {
        Dictionary<Guid, User> Users { get; }
    }
}
