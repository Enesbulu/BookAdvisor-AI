using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.RemoveBookFromReadingList;
using BookAdvisor.Application.Features.ReadingLists.Queries.GetReadingListById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    /// <summary>
    /// Okuma listeleri ile ilgili işlemleri yöneten API uç noktası.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingListsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadingListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni bir okuma listesi oluşturur.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Oluşturulan listenin ID'si.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReadingListCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        /// <summary>
        /// ReadingList içerisine kitap ekleme
        /// </summary>
        /// <param name="readingListId">ReadingList id bilgisi</param>
        /// <param name="command">ReadingList ve Book Id bilgilerini içeren obje alır</param>
        /// <returns></returns>
        [HttpPost("{readingListId}/books")]
        public async Task<IActionResult> AddBook(Guid readingListId, [FromBody] AddBookToReadingListCommand command)
        {
            //gelen istek id ile body gelen id eşleşme kontrolü
            if (readingListId != command.ReadingListId)
                return BadRequest("URL'deki ID ile gönderilen veri uyuşmuyor.");

            await _mediator.Send(command);
            return Ok(ApplicationMessages.ReadingListCreated);
        }


        /// <summary>
        /// ReadingListId ile ReadingList içerisinde bulunan Book nesnelerini döner.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpGet("{listId}")]
        public async Task<IActionResult> GetById(Guid listId)
        {
            var query = new GetReadingListByIdQuery(listId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Listeden Tekil Kitap Silme işlemi
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpDelete("{listId}/books/{bookId}")]
        public async Task<IActionResult> RemoveBookFromReadingList(Guid listId, Guid bookId)
        {
            var command = new RemoveBookFromReadingListCommand(ReadingListId: listId, BookId: bookId);
            await _mediator.Send(command);
            return NoContent(); //Silme başarılı-204

        }
    }
}
