using System.IO;
using System.Security.AccessControl;
using System;
using SystemAcl.Collections.Generic;
using FileSystemAclExtensions.ComponentModel.DataAnnotations;

namespace Assignment4.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
