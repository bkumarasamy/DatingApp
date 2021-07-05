using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            return await _context.User
                .Where(x => x.Username == username)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            // throw new System.NotImplementedException();
        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query= _context.User.AsQueryable();

            query=query.Where(u => u.Username != userParams.CurrentUsername);
            query=query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query=query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query=userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                   _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber,userParams.PageSize);
               
            // return await _context.User
            //     .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            //     .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.User
            .Include(p => p.Photos)
            .ToListAsync();
            //throw new System.NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
            // return await _context.User.FirstOrDefaultAsync(id);
            // throw new System.NotImplementedException();
        }

        public async Task<AppUser> GetUserByNameAsync(string username)
        {
            return await _context.User
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.Username == username);
            //throw new System.NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
            //throw new System.NotImplementedException();
        }

        public void update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            
            // throw new System.NotImplementedException();
        }
    }
}