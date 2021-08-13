using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void update(AppUser user);

         Task<IEnumerable<AppUser>> GetUserAsync();

         Task<AppUser> GetUserByIdAsync(int id);

         Task<AppUser> GetUserByNameAsync(string username);

         Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userparams);

         Task<MemberDTO> GetMemberAsync(string username);

         Task<string> GetUserGender(string username);
    }
}