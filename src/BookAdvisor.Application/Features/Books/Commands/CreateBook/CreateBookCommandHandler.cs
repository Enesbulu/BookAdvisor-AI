using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Events;
using BookAdvisor.Domain.Interfaces;
using MassTransit;
using MediatR;


namespace BookAdvisor.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public CreateBookCommandHandler(IBookRepository bookRepository, IPublishEndpoint publishEndpoint)
        {
            _bookRepository = bookRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book(title: request.Title, author: request.Author, isbn: request.ISBN, description: request.Description);

            await _bookRepository.AddAsync(book);

            //Eğer Description boş ise AI servisi ile doldurulması için event yayınla
            if (string.IsNullOrWhiteSpace(request.Description))
                await _publishEndpoint.Publish(new BookCreatedEvent(BookId: book.Id, Title: book.Title, Author: book.Author), cancellationToken);

            return book.Id;
        }
    }
}
