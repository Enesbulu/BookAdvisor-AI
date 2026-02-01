namespace BookAdvisor.Application.Comman.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }  //Dönen veriler
        public int PageNumber { get; set; } //Şu anki sayfa
        public int PageSize { get; set; }   //Sayfa boyutu
        public int TotalCount { get; set; } //Db de olan Toplam kayıt sayısı

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(int pageNumber, int pageSize, int totalCount, List<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
    }
}
