namespace BookAdvisor.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }
    }
}
