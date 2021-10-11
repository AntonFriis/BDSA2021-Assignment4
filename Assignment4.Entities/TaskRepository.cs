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
                tagsFound.Add(_context.Tags.Select(t => t).Where(t => t.Name == tag).FirstOrDefault());
            }

            var taskNew = new Task{
                    Title = task.Title,
                    AssignedTo = _context.Users.Find(task.AssignedToId), // might fail due to nullable need to check this.
                    Description = task.Description,
                    State = New,
                    Tags = tagsFound
            };

            _context.Tasks.Add(taskNew);

            return (Created, _context.SaveChanges());
        }

        public IReadOnlyCollection<TaskDTO> ReadAll() => _context.Tasks.Select(
            t => new TaskDTO (
                t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.Name).ToList(), t.State
            )
        ).ToList().AsReadOnly();

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {

            var tasks = from t in _context.Tasks // This will Return a Type of IQuriable not List
                             where t.State == Removed
                             select new TaskDTO(
                                 t.Id,
                                 t.Title,
                                 t.AssignedTo.Name,
                                 t.Tags.Select(ta => ta.Name).ToList(),
                                 t.State
                             );

            return tasks.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            //Find the tag in DB
            var foundtag = from ta in _context.Tags
                            where ta.Name == tag
                            select ta;

            // Find the Object in the DBSet. Returns the object
            var ftag = _context.Tags.Find(foundtag.FirstOrDefault().Id);  // How to oneline this shit..

            var tasks = from t in _context.Tasks
                            where t.Tags.Contains(ftag) // Match with object.
                            select new TaskDTO(
                                t.Id,
                                t.Title,
                                t.AssignedTo.Name,
                                t.Tags.Select(ta => ta.Name).ToList(),
                                t.State
                            );

            return tasks.ToList().AsReadOnly();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            var tasks = from t in _context.Tasks
                            where t.AssignedTo.Id == userId
                            select new TaskDTO(
                                t.Id,
                                t.Title,
                                t.AssignedTo.Name,
                                t.Tags.Select(ta => ta.Name).ToList(),
                                t.State
                            );

            return tasks.ToList().AsReadOnly();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
             var tasks = from t in _context.Tasks
                            where t.State == state
                            select new TaskDTO(
                                t.Id,
                                t.Title,
                                t.AssignedTo.Name,
                                t.Tags.Select(ta => ta.Name).ToList(),
                                t.State
                            );

            return tasks.ToList().AsReadOnly();
        }
        public TaskDetailsDTO Read(int taskId)
        {
            var tasks = from t in _context.Tasks
                where t.Id == taskId
                select new TaskDetailsDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Created,
                    t.AssignedTo.Name,
                    t.Tags.Select(ta => ta.Name).ToList(),
                    t.State,
                    t.StateUpdated
                );
            return tasks.FirstOrDefault();
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
