using Assignment4.Core;
using System.Collections.Generic;
using System;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class UserRepository : IUserRepository
    {
        KanbanContext _context;

        public UserRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            //Check if user exists. @TO-DO

            var userEntity = new User
            {
                Name = user.Name,
                Email = user.Email,
            };

            _context.Users.Add(userEntity);
            return (Created , _context.SaveChanges());

        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public UserDTO Read(int userId)
        {
            throw new NotImplementedException();
        }

        public Response Update(UserUpdateDTO user)
        {
            throw new NotImplementedException();
        }

        public Response Delete(int userId, bool force = false)
        {
            throw new NotImplementedException();
        }
    }
}
