using Bogus;
using BookAdvisor.Application.Features.Books.Commands.CreateBook;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Events;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using MassTransit;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
            _handler = new CreateBookCommandHandler(_mockBookRepository.Object, _mockPublishEndpoint.Object);
        }


        [Fact]
        public async Task Handle_Should_CreateBook_And_Add_To_Repository()
        {
            //Arrange-Hazırlık
            var command = new CreateBookCommand(
                Title: "Test Book",
                Author: "Test Author",
                ISBN: "1354645",
                Description: "Test Açıklaması"
                );

            //Act - Eylem
            var result = await _handler.Handle(command, cancellationToken: CancellationToken.None);

            //Assert    - varsayım test
            result.Should().NotBeEmpty();
            _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);
            _mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BookCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task Handle_Should_Publish_Event_When_Description_Is_Empty()
        {
            //Arrange
            var command = new CreateBookCommand(
                Title: "AI Gereken Kitap",
                Author: "Yazar",
                ISBN: "1321354",
                Description: ""
                );

            //Act
            await _handler.Handle(command, cancellationToken: CancellationToken.None);

            //Assert
            _mockPublishEndpoint.Verify(x => x.Publish(It.IsAny<BookCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handle_Should_CreateBook_With_Random_Data()
        {
            //
            var faker = new Faker<CreateBookCommand>()
                .CustomInstantiator(f => new CreateBookCommand(
                    Title: f.Lorem.Sentence(3),         // Rastgele 3 kelimelik kitap adı
                    Author: f.Name.FullName(),          // Rastgele yazar adı (John Doe vb.)
                    Description: f.Lorem.Paragraph(),   // Rastgele dolu bir paragraf
                    ISBN: f.Commerce.Ean13()            // Rastgele barkod
                    ));
            var command = faker.Generate(); //veriyi üretir.

            //act
            var result = await _handler.Handle(command, CancellationToken.None);

            //assert
            result.Should().NotBeEmpty();

            //repositoriye eklenen kitap bilgilerini kontrol etme
            _mockBookRepository.Verify(
                x => x.AddAsync(
                    It.Is<Book>(
                        b => b.Title == command.Title && b.Author == command.Author)), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Create_Book_Successfully()
        {
            // ARRANGE
            var command = new CreateBookCommand("1984", "George Orwell", "Distopya","123-123-123");

            // ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            // 1. Repository'nin AddAsync metodu 1 kere çağrıldı mı?
            _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);

            // 2. Çağrılan metodun içine giden veriler doğru mu?
            _mockBookRepository.Verify(x => x.AddAsync(It.Is<Book>(b =>
                b.Title == "1984" &&
                b.Author == "George Orwell"
            )), Times.Once);
        }

        //Çoklu veri testi (Data Driven Test)
        [Theory]    //Fact yerine theory kullanıyorum
        [InlineData("", "Yazar 1")]
        [InlineData("Kitap 1", "")]
        [InlineData(null, null)]
        public async Task Handle_Should_Throw_Or_Behave_Specific_On_Invalid_Data(string title, string author)
        {
            //VAlidasyon testi yazılabilecek alan..
        }
    }
}
