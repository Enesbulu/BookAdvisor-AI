using BookAdvisor.Domain.Common;
using BookAdvisor.Domain.Constants;

namespace BookAdvisor.Domain.Entities
{
    public class Review : BaseEntity
    {
        //ilişkiler
        public Guid BookId { get; private set; }
        public string UserId { get; private set; }

        //Değerlendirme verileri
        public int Rating { get; private set; } //1 ila 5 arası puanlama
        public string? Comment { get; private set; }    //yorum

        //Navigation properties
        public virtual Book Book { get; private set; }

        private Review() { }

        public Review(Guid bookId, string userId, int rating, string? comment)
        {
            //Aralık dışı puanlama
            if (rating < 1 || rating > 5)
                throw new ArgumentException(DomainConstants.Review.RatingOutOfRange);
            //login zorunluluğu
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), DomainConstants.Review.UserInformationIsRequired);

            BookId = bookId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
        }

        //Yorum güncelleme
        public void Update(int rating, string? comment)
        {
            if (rating < 1 || rating >5)
            {
                throw new ArgumentException(DomainConstants.Review.RatingOutOfRange);
            }

            Rating = rating;
            Comment = comment;
        }
    }
}
