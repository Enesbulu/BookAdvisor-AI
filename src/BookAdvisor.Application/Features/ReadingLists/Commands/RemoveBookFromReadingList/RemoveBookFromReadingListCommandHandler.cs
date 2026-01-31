using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.RemoveBookFromReadingList
{
    public class RemoveBookFromReadingListCommandHandler : IRequestHandler<RemoveBookFromReadingListCommand>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public RemoveBookFromReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(RemoveBookFromReadingListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new Exception(ApplicationMessages.LoginRequired);

            //Listeyi ve içindekileri çekiyoruz.
            var readingList = await _readingListRepository.GetByIdAsync(request.ReadingListId);

            if (readingList.UserId != userId)
                throw new UnauthorizedAccessException(ApplicationMessages.UnauthorizedAccess);

            //Domain mantığı, listeden çıkar
            var itemToDelete = readingList.RemoveBookFromReadingList(request.BookId);

            //Eğer listede zaten yoksa null dön işlem yapma
            if (itemToDelete == null) return;

            //repo, db den sil
            await _readingListRepository.RemoveBookFromListItemAsync(itemToDelete);

        }
    }
}
