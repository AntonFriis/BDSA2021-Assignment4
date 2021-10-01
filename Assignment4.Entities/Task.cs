namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLenght(100)]
        public string Title { get; set; }

        //optional
        public User AssignedTo { get; set; }

        [StringLength(max)] //optional
        public string Description { get; set; }

        public enum State { get; set; }

        public Icollection<Tag> Tags { get; set; }

    }
}
