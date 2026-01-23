using BookAdvisor.Application.Features.Books.Commands.CreateBook;
using BookAdvisor.Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetBookByIdQuery(id);
            var book = await _mediator.Send(query);
            if (book == null)
                return NotFound($"ID'si {id} olan kitap bulunamadı.");
            return Ok(book);
        }
    }
}
