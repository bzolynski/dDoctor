using Domain.Entities;
using Persistance.Services.Common;
using System.Threading.Tasks;

namespace Persistance.Services.UserDataServices
{
    public interface IAccountDataService : IDataService<Account>
    {
        //Task<IEnumerable<User>> GetManyByType(int id, UserTypes userType);

        Task<Account> GetByUsername(string username);
        Task<Account> GetByEmail(string email);



    }
}
