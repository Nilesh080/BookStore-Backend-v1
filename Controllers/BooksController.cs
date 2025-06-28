using BookStore.API.DTOs;
using BookStore.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllAsync();
            if (books == null) {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] BookRequest request)
        {
            var id = await _bookRepository.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = id, controller = "Books" }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BookRequest request)
        {
            await _bookRepository.Update(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result  = await _bookRepository.Delete(id);
            if (!result) return NotFound();
            return Ok();

        }
    }
}
