using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskToDo.Data;
using TaskToDo.DTO;

namespace TaskToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ToDoContext _context;

        public TaskController(ToDoContext context)
        {
            _context = context;
        }

        // FETCHING TASK BY ID
        [HttpGet("GetTaskByID/{id}")]
        public async Task<ActionResult<TaskToDo.Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            return task;
        }

        // DELETING TASK BY ID
        [HttpDelete("DeleteTaskByID/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // ADD TASK BY TASK ID       
        [HttpPost("AddTask/{listId}")]
        public async Task<IActionResult> AddTask(int listId, [FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new TaskToDo.Models.Task
            {
                TaskName = taskDto.TaskName,
                TaskDueDate = taskDto.TaskDueDate,
                ListID = listId,
                CreatedOn = DateTime.Now,
                IsComplete = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task added successfully.", TaskID = task.TaskID });
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.TaskName = updatedTask.TaskName ?? task.TaskName;
            task.TaskDueDate = updatedTask.TaskDueDate ?? task.TaskDueDate;
            task.IsModified = true;
            task.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Task updated successfully." });
        }

        [HttpPut("ToggleCompletion/{id}")]
        public async Task<IActionResult> ToggleCompletion(int id, [FromBody] CompletionDto completionDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.IsComplete = completionDto.IsComplete;
            task.IsModified = true;
            task.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Task completion status updated." });
        }
    }
}
