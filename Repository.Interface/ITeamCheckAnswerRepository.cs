using System.Linq;
using System.Threading.Tasks;
using DomainModel;

namespace Repository.Interface
{
    public interface ITeamCheckAnswerRepository
    {
        IQueryable<TeamCheckAnswer> GetAll();
        Task SaveAnswer(TeamCheckAnswer answer);
    }
}