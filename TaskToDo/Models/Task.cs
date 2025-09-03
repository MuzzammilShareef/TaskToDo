using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskToDo.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }

        [MaxLength(20)]
        public string TaskName { get; set; }

        public bool IsComplete { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? TaskDueDate { get; set; }

        [ForeignKey("List")]
        public int ListID { get; set; }

        public List List { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsModified { get; set; } = false;

        public DateTime? ModifiedOn { get; set; }
    }
}
