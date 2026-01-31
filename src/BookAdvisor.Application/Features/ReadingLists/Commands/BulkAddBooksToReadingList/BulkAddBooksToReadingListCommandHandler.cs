using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.BulkAddBooksToReadingList
{
    public class BulkAddBooksToReadingListCommandHandler : IRequestHandler<BulkAddBooksToReadingListCommand>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public BulkAddBooksToReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(BulkAddBooksToReadingListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);

            //Listeyi getir
            var readingList = await _readingListRepository.GetByIdWithBooksAsync(request.ReadingListId);

            if (readingList == null) throw new Exception(ApplicationMessages.ReadingListNotFound);
            if (readingList.UserId != userId) throw new UnauthorizedAccessException(ApplicationMessages.UnauthorizedAccess);

            //Toplu ekleme yap, sadece yeni eklenenleri al
            var newItems = readingList.AddBooksToReadingList(request.BookIds);

            //Eğer eklenecek yeni bir şey yoksa çık.
            if (newItems.Count == 0) return;

            //Toplu kaydet
            await _readingListRepository.AddListItemsRepository(newItems);

        }
    }
}
