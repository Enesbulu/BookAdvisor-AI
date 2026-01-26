using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList
{
    public class AddBookToReadingListCommandHandler : IRequestHandler<AddBookToReadingListCommand>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;
        public AddBookToReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }


        public async Task Handle(AddBookToReadingListCommand request, CancellationToken cancellationToken)
        {
            ///Kullanıcı giriş kontrolü
            var userId = _currentUserService.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);

            ///Liste getirme
            var readingList = await _readingListRepository.GetByIdAsync(request.ReadingListId);

            ///Liste var mı kontrolü
            if (readingList == null)
            {
                throw new Exception(ApplicationMessages.ReadingListNotFound);
            }

            ///Yetki kontrolü -Kullanıcının listesi mi?
            if (readingList.UserId != userId)
            {
                throw new UnauthorizedAccessException(ApplicationMessages.UnauthorizedAccess);
            }

            ///Domain mantığı Kitabı listeye ekleme
            //readingList.AddBook(request.BookId);
            var newItem = readingList.AddBookAtReadingList(request.BookId);

            if (newItem == null) { return; }

            ///Kaydetme
            await _readingListRepository.AddListItemAsync(newItem);
        }
    }
}
