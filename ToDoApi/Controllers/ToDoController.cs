using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _context;
        public ToDoController(ToDoContext context)
        {
            _context = context;

            if (_context.ToDoModel.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.ToDoModel.Add(new ToDoModel
                { Name = "Item1" });
                _context.SaveChanges();
            }

        }

        #region Methodes

        /// <summary>
        /// Get/Api/ToDo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ToDoModel>> PostTodoItem(ToDoModel item)
        {
            _context.ToDoModel.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        /// <summary>
        /// Get/Api/ToDo/"{id}"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoModel>> GetTodoItem(long id)
        {
            var todoItem = await _context.ToDoModel.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// Post/Api/ToDo/Create
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>a newly create to do item</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> OnPostAsync(ToDoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("sorry something went wrong");
            }

            await _context.ToDoModel.AddAsync(model);
            await _context.SaveChangesAsync();
            return Ok("your item has been Added");
        }

        /// <summary>
        /// Put/Api/ToDo/edit/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, ToDoModel item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete/Api/ToDo/delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.ToDoModel.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.ToDoModel.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

    }
}