using System.ComponentModel.DataAnnotations;

namespace TaskToDo.DTO
{
    public class TaskDto
    {
        [Required]
        public string TaskName { get; set; }
        public DateTime? TaskDueDate { get; set; }
    }

    public class ListDto
    {
        [Required]
        public string ListName { get; set; }

        [MinLength(1, ErrorMessage = "At least one task is required.")]
        public List<TaskDto> Tasks { get; set; }
    }

}
