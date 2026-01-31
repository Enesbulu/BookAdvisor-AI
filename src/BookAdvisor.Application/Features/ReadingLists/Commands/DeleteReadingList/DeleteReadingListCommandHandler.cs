using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.DeleteReadingList
{
    public class DeleteReadingListCommandHandler : IRequestHandler<DeleteReadingListCommand>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteReadingListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);
            }

            var readingList = await _readingListRepository.GetByIdAsync(request.Id);
            if (readingList == null)
                throw new Exception(ApplicationMessages.ReadingListNotFound);

            if (readingList.UserId != userId)
                throw new UnauthorizedAccessException(ApplicationMessages.UnauthorizedAccess);

            await _readingListRepository.DeleteAsync(readingList);
        }
    }
}
