namespace BookAdvisor.Domain.Constants
{
    public static class DomainConstants
    {
        public static class Books
        {
            public const int TitleMaxLength = 200;
            public const int AuthorMaxLength = 100;
            public const int IsbnMaxLength = 13;
        }

        public static class ReadingLists
        {
            public const int NameMaxLength = 100;
        }
        public static class Review
        {
            public const string RatingOutOfRange = "Puan 1 ile 5 arasında olmalıdır.";
            public const string UserInformationIsRequired = "Kullanıcı bilgisi zorunludur.";
        }
    }
}
