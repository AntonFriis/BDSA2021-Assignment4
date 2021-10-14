using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(int.MaxValue)] //optional
        public string Description { get; set; }

        public DateTime Created { get; set; }

        //optional
        public User AssignedTo { get; set; }

        public ICollection<Tag> Tags { get; set; }

        [Required]
        [StringLength(50)]
        public State State { get; set; }

        public DateTime StateUpdated { get; set; }

    }
}
