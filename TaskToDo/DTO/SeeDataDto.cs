namespace TaskToDo.DTO
{
    public class SeeDataDto
    {
        public int ListID { get; set; }
        public string ListName { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Status { get; set; }      // Completed / Not Completed
        public string Change { get; set; }      // Modified / Not Modified
        public string Present { get; set; }     // Deleted / Not Deleted
    }
}
