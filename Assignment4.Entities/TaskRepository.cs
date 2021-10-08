using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using Assignment4.Core;
using static Assignment4.Core.Response;
using static Assignment4.Core.State;

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
            var tagsFound = new List<Tag>();
            foreach (var tag in task.Tags)
            {
                tagsFound.Add(_context.Tags.Select(t => t).FirstOrDefault(t => t.Name == tag));
            }

            var taskNew = new Task{
                    Title = task.Title,
                    AssignedTo = _context.Users.Find(task.AssignedToId), // might fail due to nullable need to check this.
                    Description = task.Description,
                    State = New,
                    Tags = tagsFound
            };

            return (Created, _context.SaveChanges());
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
