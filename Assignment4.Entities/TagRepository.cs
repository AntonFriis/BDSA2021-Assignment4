using System.Collections.Generic;
using System.Linq;
using Assignment4.Core;
using static Assignment4.Core.Response;
using static Assignment4.Core.State;

namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly KanbanContext _context;

        public TagRepository(KanbanContext context)
        {
            _context = context;
        }
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            if (tag.Name == _context.Tags.Where(t => t.Name == tag.Name)
                                        .Select(t => t).FirstOrDefault()?.Name)
                return (Conflict, 0);
            var newTag = new Tag{Name = tag.Name};
            _context.Tags.Add(newTag);

            _context.SaveChanges();

            return (Created, newTag.Id);
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public TagDTO Read(int tagId)
        {
            var tags = from t in _context.Tags
                where t.Id == tagId
                select new TagDTO(t.Id, t.Name);

            return tags.FirstOrDefault();
        }

        public Response Update(TagUpdateDTO tag)
        {
            throw new System.NotImplementedException();
        }

        public Response Delete(int tagId, bool force = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
