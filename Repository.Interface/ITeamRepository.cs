using System;
using System.Threading.Tasks;
using DomainModel;

namespace Repository.Interface
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> AddMember(Guid teamId, Member member);
        Task<Team> RemoveMember(Guid teamId, Guid memberId);
    }
}