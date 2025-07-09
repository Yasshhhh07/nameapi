using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models;
using EmployeeApi.Data;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            try
            {
                return Ok(await _context.Employees.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            try
            {
                var emp = await _context.Employees.FindAsync(id);
                if (emp == null) return NotFound();
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

         
        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: ID already exist"); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            if (id != updated.Id) return BadRequest("ID mismatch");

            try
            {
                var emp = await _context.Employees.FindAsync(id);
                if (emp == null) return NotFound();

                emp.Name = updated.Name;
                emp.Age = updated.Age;
                emp.Department = updated.Department;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error:{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var emp = await _context.Employees.FindAsync(id);
                if (emp == null) return NotFound();

                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
