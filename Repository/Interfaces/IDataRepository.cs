using Microsoft.AspNetCore.Mvc;
using PublishAPI.Models;
using System.Threading.Tasks;

namespace PublishAPI.Repository.Interfaces
{
    public interface IDataRepository
    {
        ActionResult GetVoterid(VoterIDRequest voteridRequest);
        Task<bool> IsAliveAsync();
    }
}
