using BookAdvisor.Domain.Common;

namespace BookAdvisor.Domain.Entities
{
    public class ReadingListItem : BaseEntity
    {
        public Guid ReadingListId { get; private set; }
        public Guid BookId { get; private set; }
        public virtual Book Book { get; private set; }

        private ReadingListItem() { }

        public ReadingListItem(Guid readingListId, Guid bookId)
        {
            ReadingListId = readingListId;
            BookId = bookId;
        }

    }
}
