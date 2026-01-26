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

        // Genel Hatalar
        public const string UnauthorizedAccess = "Bu işlem için yetkiniz yok.";
    }
}
