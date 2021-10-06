using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;
        
        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }
        
        public void Dispose() => _context.Dispose();

        public IReadOnlyCollection<TaskDTO> All()
        {
            var tasks = new List<TaskDTO>();
            foreach (var task in _context.Tasks)
            {
                tasks.Add(new TaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    AssignedToId = task.AssignedTo.Id,
                    Tags = task.Tags.Select(tag => tag.Name).ToImmutableList(),
                    State = task.State
                });
            }

            return tasks.ToImmutableList();
        }

        public int Create(TaskDTO task)
        {
            _context.Tasks.Add(new Task
            {
                Id = task.Id,
                Title = task.Title,
                AssignedTo = _context.Users.FirstOrDefault(user => user.Id == task.AssignedToId),
                Description = task.Description,
                State = task.State,
                Tags = _context.Tags.Where(tag => tag.Name == task.Tags.Select(name => name).FirstOrDefault()).ToList()
            });
            return _context.SaveChanges();
        }

        public void Delete(int taskId)
        {
            var taskToRemove = _context.Tasks.Where(task => task.Id == taskId).Select(task => task).FirstOrDefault();
            if (taskToRemove != null) _context.Tasks.Remove(taskToRemove);
            _context.SaveChanges();
        }

        public TaskDetailsDTO FindById(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            
            return new TaskDetailsDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    AssignedToId = task.AssignedTo.Id,
                    AssignedToName = task.AssignedTo.Name,
                    AssignedToEmail = task.AssignedTo.Email,
                    Tags = task.Tags.Select(tag => tag.Name),
                    State = task.State
                };
            
        }

        public void Update(TaskDTO task)
        {
            _context.Update(task);
            _context.SaveChanges();
        }
    }
}
