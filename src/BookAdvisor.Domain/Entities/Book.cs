using BookAdvisor.Domain.Common;

namespace BookAdvisor.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public string ISBN { get; private set; }

        private Book() { }

        public Book(string title, string author, string description, string isbn)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Title cannot be empty.[Kitap başlığı boş olamaz.]");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentNullException(nameof(author), "Author cannot be empty.[Yazar adı boş olamaz.]");


            Title = title;
            Author = author;
            Description = description;
            ISBN = isbn;
        }

        // Domain Behavior: Kitap bilgilerini güncellemek için  kullanılır.
        public void UpdateDetails(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Title cannot be empty. [Başlık boş olamaz.]");
            Title = title;
            Description = description;
            UpdateDate = DateTime.UtcNow;
        }

    }
}
