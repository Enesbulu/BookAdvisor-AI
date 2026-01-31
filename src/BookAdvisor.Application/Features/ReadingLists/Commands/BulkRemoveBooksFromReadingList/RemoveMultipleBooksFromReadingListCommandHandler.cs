using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.BulkRemoveBooksFromReadingList
{
    public class RemoveMultipleBooksFromReadingListCommandHandler : IRequestHandler<BulkRemoveBooksFromReadingListCommand>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public RemoveMultipleBooksFromReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(BulkRemoveBooksFromReadingListCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            //Doğru kullanıcı giriş kontrolü
            if (string.IsNullOrEmpty(currentUserId))
                throw new Exception(ApplicationMessages.LoginRequired);
            //Silinecek listeyi alma
            var targetReadingList = await _readingListRepository.GetByIdAsync(request.TargetReadingListId);

            //Liste var mı?
            if (targetReadingList == null)
                throw new Exception(ApplicationMessages.ReadingListNotFound);

            //Listenin sahibi doğru mu?
            if (targetReadingList.UserId != currentUserId)
                throw new UnauthorizedAccessException(ApplicationMessages.UnauthorizedAccess);

            //kitapları hafızadan çıkar ve silinen nesneleri al
            var itemsToDelete = targetReadingList.RemoveMultipleBooksFromThisReadingList(request.BookIdsToRemove);

            //Eğer silinecek bir şey yoksa işlemden çık
            if (itemsToDelete.Count == 0) return;

            //Db den kalıcı olarak sil
            await _readingListRepository.RemoveBooksFromListItemsAsync(itemsToDelete);
        }
    }
}
