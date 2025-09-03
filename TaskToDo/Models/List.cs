using System.ComponentModel.DataAnnotations;

namespace TaskToDo.Models
{
    public class List
    {
        [Key]
        public int ListID { get; set; }

        [Required]
        [MaxLength(40)]
        public string ListName { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsModified { get; set; } = false;

        public ICollection<Task> Tasks { get; set; }
    }
}
