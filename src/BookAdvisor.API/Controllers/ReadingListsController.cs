using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.BulkAddBooksToReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.BulkRemoveBooksFromReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.DeleteReadingList;
using BookAdvisor.Application.Features.ReadingLists.Commands.RemoveBookFromReadingList;
using BookAdvisor.Application.Features.ReadingLists.Queries.GetReadingListById;
using BookAdvisor.Application.Features.ReadingLists.Queries.GetUserReadingLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    /// <summary>
    /// Okuma listeleri ile ilgili işlemleri yöneten API uç noktası.
    /// </summary>
    [Authorize]
    [Route("api/reading-lists")]
    [ApiController]
    public class ReadingListsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadingListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //GET api-/reading-lists
        [HttpGet]
        public async Task<IActionResult> GetALl()
        {
            var query = new GetUserReadingListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //GET api/reading-lists/{id}
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

        //POST api/reading-lists
        /// <summary>
        /// Yeni bir okuma listesi oluşturur.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Oluşturulan listenin ID'si.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReadingListCommand command)
        {
            var id = await _mediator.Send(command);
            //return CreatedAtAction(nameof(GetById), new { id = id }, id);   // 201 Created dönmek header'da yeni kaynağın adresini verir.
            return Ok(); //şimdilik servis çalıştığında hata vermesin. Önyüz yok henüz.
        }


        //DELETE api/reading-lists/{id}
        [HttpDelete("{readingListId}")]
        public async Task<IActionResult> Delete(Guid readingListId)
        {
            await _mediator.Send(new DeleteReadingListCommand(readingListId));
            return NoContent();
        }

        /// ---Diğer Kaynak işlemleri --

        //POST api/reading-lists/{readingListId}/books
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
                return BadRequest("URL'deki ID ile Body gönderilen veri uyuşmuyor.");

            await _mediator.Send(command);
            return Ok(ApplicationMessages.BookAddedToList);
        }

        // DELETE api/reading-lists/{id}/books/{bookId}
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

        //POST api/reading-lists/{id}/books/bulk
        /// <summary>
        /// Listeye Toplu kitap ekleme
        /// </summary>
        /// <param name="id">Liste id</param>
        /// <param name="bookIds">Eklenecek Kitapların id listesi</param>
        /// <returns></returns>
        [HttpPost("{listId}/books/bulk")]
        public async Task<IActionResult> BulkAddBooks(Guid listId, [FromBody] List<Guid> bookIds)
        {
            //Command oluştur
            var command = new BulkAddBooksToReadingListCommand(listId, bookIds);
            await _mediator.Send(command);
            return Ok("Kitap başarıyla eklendi.");
        }

        // DELETE api/reading-lists/{readingListId}/books/bulk
        /// <summary>
        /// Belirtilen okuma listesinden, gönderilen ID listesindeki kitapları topluca siler.
        /// </summary>
        /// <param name="readingListId">Hedef listenin ID'si</param>
        /// <param name="bookIdsToRemove">Silinecek kitapların ID listesi (Body'den gelir)</param>
        [HttpDelete("{readingListId}/books/bulk")]
        public async Task<IActionResult> RemoveMultipleBooksFromReadingList(Guid readingListId, [FromBody] List<Guid> bookIdsToRemove)
        {
            var command = new BulkRemoveBooksFromReadingListCommand(TargetReadingListId: readingListId, BookIdsToRemove: bookIdsToRemove);
            await _mediator.Send(command);
            return NoContent();
        }



    }
}
