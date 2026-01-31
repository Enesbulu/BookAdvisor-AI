using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Queries.GetUserReadingLists
{
    public class GetUserReadingListsQueryHandler : IRequestHandler<GetUserReadingListQuery, List<ReadingListDto>>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetUserReadingListsQueryHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<ReadingListDto>> Handle(GetUserReadingListQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);
            }

            //repository den lsiteyi çekme
            var lists = await _readingListRepository.GetUserListsAsync(userId);
            //entity dto dönüşümü
            return lists.Select(x => new ReadingListDto(
                id: x.Id,
                Name: x.Name,
                BookCount: x.Items.Count)).ToList();
        }
    }
}
