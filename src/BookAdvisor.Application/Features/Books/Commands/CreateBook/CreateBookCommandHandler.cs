using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAiService _aiService;
        public CreateBookCommandHandler(IBookRepository bookRepository, IAiService aiService)
        {
            _bookRepository = bookRepository;
            _aiService = aiService;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            string description = request.Description;

            //Eğer Kullanıcı açıklama girmemişse, AI servisi ile açıklama oluştur
            if (string.IsNullOrWhiteSpace(description))
            {
                description = await _aiService.GenerateBookSummaryAsync(title: request.Title, author: request.Author);
            }


            var book = new Book(title: request.Title, author: request.Author, isbn: request.ISBN, description: description);

            await _bookRepository.AddAsync(book);

            return book.Id;
        }
    }
}
