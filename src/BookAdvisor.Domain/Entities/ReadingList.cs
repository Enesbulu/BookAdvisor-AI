using BookAdvisor.Domain.Common;

namespace BookAdvisor.Domain.Entities
{
    public class ReadingList : BaseEntity
    {
        public string Name { get; private set; }
        public string UserId { get; private set; } //Liste sahibi

        private readonly List<ReadingListItem> _items = new();
        public IReadOnlyCollection<ReadingListItem> Items => _items.AsReadOnly();

        private ReadingList() { }

        public ReadingList(string name, string userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Liste Adı Boş Olamaz");
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("Kullanıcı bilgisi zorunludur.");
            Name = name;
            UserId = userId;
        }

        //Listeye kitap ekleme mantığı
        public void AddBook(Guid bookId)
        {
            _items.Add(new ReadingListItem(readingListId: this.Id, bookId: bookId));
        }


        /// <summary>
        /// Listeye Kitap ekleme işlemi, geriye eklenen itemı dönüyor.
        /// </summary>
        /// <param name="bookId">Eklenecek BookId</param>
        /// <returns>ReadingListItem nesnesi döner</returns>
        public ReadingListItem AddBookAtReadingList(Guid bookId)
        {
            if (_items.Any(i => i.BookId == bookId))
            {
                return null;
            }

            var newItem = new ReadingListItem(readingListId: this.Id, bookId: bookId);
            _items.Add(newItem);
            return newItem;
        }

        /// <summary>
        ///  Belirtilen kitabı listeden çıkarır.
        /// </summary>
        /// <param name="bookId">Listeden silinecek bookId</param>
        /// <returns>Silinecek olan item nesnesi (Repository'e vermek için) veya null.</returns>
        public ReadingListItem? RemoveBookFromReadingList(Guid bookId)
        {
            var item = _items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                _items.Remove(item);
                return item;    //Repositori içerisinde silme işlemi yapılacak.
            }
            return null;
        }
    }
}
