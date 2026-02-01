using BookAdvisor.Application.Features.Books.Commands.CreateBook;
using BookAdvisor.Application.Features.Books.Queries.GetAllBooks;
using BookAdvisor.Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    [Authorize]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //GET api/books?pageNumber=1&pageSize=10&searchKeyword=Dune
        /// <summary>
        /// Sistemdeki bütün kitapları sayfalama ve arama filtresiyle getirir
        /// </summary>
        /// <param name="query">Arama parametresi</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] GetAllBooksQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //GET api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetBookByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, id);
        }


    }
}
