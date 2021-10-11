using Assignment4.Core;
using System.Collections.Generic;
using System.Linq;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class UserRepository : IUserRepository
    {
        private readonly KanbanContext _context;

        public UserRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            if (user.Email == _context.Users.Where(u => u.Email == user.Email)
                    .Select(u => u).FirstOrDefault()?.Email)
                    return (Conflict, 0);
            
            var userEntity = new User
            {
                Name = user.Name,
                Email = user.Email,
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();
            
            return (Created , userEntity.Id);

        }

        public IReadOnlyCollection<UserDTO> ReadAll() => 
            _context.Users.Select(u => new UserDTO(u.Id, u.Name, u.Email)).ToList().AsReadOnly();

        public UserDTO Read(int userId)
        {
            var users = from u in _context.Users
                where u.Id == userId
                select new UserDTO(u.Id, u.Name, u.Email);

            return users.FirstOrDefault();
        }

        public Response Update(UserUpdateDTO user)
        {
            var entity = _context.Users.Find(user.Id);

            if (entity == null)
            {
                return NotFound;
            }

            entity.Id = user.Id;
            entity.Name = user.Name;
            entity.Email = user.Email;

            _context.SaveChanges();

            return Updated;
        }

        public Response Delete(int userId, bool force = false)
        {
            var entity = _context.Users.Find(userId);

            if (entity == null)
            {
                return NotFound;
            }

            if (force || entity.Tasks.Count == 0)
            {
                _context.Users.Remove(entity);
            } else if (entity.Tasks.Count != 0)
            {
                return Conflict;
            }
            
            _context.SaveChanges();
            return Deleted;
        }
    }
}
