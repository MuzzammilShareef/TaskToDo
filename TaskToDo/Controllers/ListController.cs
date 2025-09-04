using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskToDo.Data;
using TaskToDo.DTO;
using TaskToDo.Models;

namespace TaskToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ListController : ControllerBase
    {

        private readonly ToDoContext _context;

        public ListController(ToDoContext context)
        {
            _context = context;
        }

        //SEE DATA PROCEDURE
        [HttpGet("SeeData")]
        public async Task<ActionResult<List<SeeDataDto>>> GetSeeData()
        {
            var result = await _context.SeeDataDtos
                .FromSqlRaw("EXEC SeeData")
                .ToListAsync();

            return Ok(result);
        }

        //INSERT LIST PROCEDURE
        [HttpPost("CreateList")]
        public async Task<IActionResult> InsertList([FromBody] ListDto listDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listNameParam = new SqlParameter("@ListName", listDto.ListName ?? "");

            var taskDataJson = JsonSerializer.Serialize(listDto.Tasks ?? new List<TaskDto>()); //use the expression listDto.Tasks if it is not null or use the experssion on the right new List<TaskDto>() which will be a empty list of TaskDto objects
            var taskDataParam = new SqlParameter("@TaskData", taskDataJson); //turning the above into param

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC InsertList @ListName, @TaskData", listNameParam, taskDataParam);

            return Ok(new { message = "List and Tasks inserted successfully." });
        }

        //COMPARE PROCEDURE
        //[HttpPost("CompareUpdate")]
        //public async Task<IActionResult> Compare([FromBody] CompareDto compareDto)
        //{
        //    var listIdParam = new SqlParameter("@ListID", compareDto.ListID);
        //    var taskDataJson = JsonSerializer.Serialize(compareDto.Tasks ?? new List<TaskDto>());
        //    var taskDataParam = new SqlParameter("@TaskData", taskDataJson);

        //    await _context.Database.ExecuteSqlRawAsync(
        //        "EXEC Compare @ListID, @TaskData", listIdParam, taskDataParam);

        //    return Ok(new { message = "Tasks compared and updated successfully." });
        //}

        //COMPARE PROCEDURE
        [HttpPut("CompareUpdate/{id}")]
        public async Task<IActionResult> Compare(int id, [FromBody] List<TaskDto> tasks)
        {
            if (tasks == null)
                return BadRequest("Tasks cannot be empty.");

            var listIdParam = new SqlParameter("@ListID", id);
            var taskDataJson = JsonSerializer.Serialize(tasks);
            var taskDataParam = new SqlParameter("@TaskData", taskDataJson);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Compare @ListID, @TaskData", listIdParam, taskDataParam);
            return Ok(new { message = "Tasks compared and updated successfully." });

        }

        // CLEAR ALL DATA PROCEDURE
        [HttpPost("ClearAllData")]
        public async Task<IActionResult> ClearAllData()
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC ClearAllData");
            return Ok(new { message = "All data cleared and identity reseeded successfully." });
        }

        // FETCHING LIST BY ID
        [HttpGet("GetListByID/{id}")]
        public async Task<ActionResult<List>> GetList(int id)
        {
            var list = await _context.Lists.Include(l => l.Tasks)
                .FirstOrDefaultAsync(l => l.ListID == id);

            if (list == null)
                return NotFound();

            return list;
        }

        // DELETING LIST BY ID
        [HttpDelete("DeleteListByID/{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null) return NotFound();

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
