using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using Assignment4.Core;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            return (BadRequest, 0);
        }
        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }
        public TaskDetailsDTO Read(int taskId)
        {
            throw new NotImplementedException();
        }
        public Response Update(TaskUpdateDTO task)
        {
            throw new NotImplementedException();
        }
        public Response Delete(int taskId)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => _context.Dispose();
    }
}
