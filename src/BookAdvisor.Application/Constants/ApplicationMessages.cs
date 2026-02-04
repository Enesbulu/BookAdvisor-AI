namespace BookAdvisor.Application.Constants
{
    public static class ApplicationMessages
    {
        // Auth Mesajları
        public const string LoginRequired = "Liste oluşturmak için giriş yapmalısınız.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string InvalidPassword = "Hatalı şifre.";

        // Reading List Mesajları
        public const string ReadingListCreated = "Okuma listesi başarıyla oluşturuldu.";
        public const string ReadingListNotFound = "Belirtilen okuma listesi bulunamadı.";
        public const string BookAddedToList = "Kitap listeye eklendi.";

        //Reviews Mesajları
        public const string AlreadyReviewed= "Bu kitap için zaten bir değerlendirmeniz bulunmaktadır. Puanınızı güncellemek için düzenleme işlemini kullanınız.";
        public const string UseToUpdateRating= "Puanınızı güncellemek için düzenleme işlemini kullanınız. ";
        public const string AddingReviewSuccess= "Değerlendirmeniz başarıyla eklenmiştir. ";



        // Genel Hatalar
        public const string UnauthorizedAccess = "Bu işlem için yetkiniz yok.";

        public const string BookNotFound = "Kitap bulunamadı.";
    }
}
