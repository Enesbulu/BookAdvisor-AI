using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList;
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
        /// ReadinList içerisine kitap ekleme
        /// </summary>
        /// <param name="id">ReadingList id bilgisi</param>
        /// <param name="command">ReadingList ve Book Id bilgilerini içeren obje alır</param>
        /// <returns></returns>
        [HttpPost("{id}/books")]
        public async Task<IActionResult> AddBook(Guid id, [FromBody] AddBookToReadingListCommand command)
        {
            //gelen istek id ile body gelen id eşleşme kontrolü
            if (id != command.ReadingListId)
                return BadRequest("URL'deki ID ile gönderilen veri uyuşmuyor.");

            await _mediator.Send(command);
            return Ok(ApplicationMessages.ReadingListCreated);
        }


        /// <summary>
        /// ReadingList Id ile ReadinList içerisinde bulunan Book nesnelerini döner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetReadingListByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
