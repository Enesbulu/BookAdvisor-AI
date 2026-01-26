using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList
{
    public class CreateReadingListCommandHandler : IRequestHandler<CreateReadingListCommand, Guid>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateReadingListCommandHandler(IReadingListRepository readingListRepository, ICurrentUserService currentUserService)
        {
            _readingListRepository = readingListRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateReadingListCommand request, CancellationToken cancellationToken)
        {
            //  Güvenlik Kontrolü: Kullanıcı kim?
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);
            }

            //Domain varlığının oluşturulması
            var readinglist = new ReadingList(request.Name, userId);

            //Veritabanı kayıt işlemleri
            await _readingListRepository.AddAsync(readinglist);

            //Sonuç dönüşü
            return readinglist.Id;

        }
    }
}
