namespace BookAdvisor.Domain.Events
{
    public record BookCreatedEvent(Guid BookId, string Title, string Author);


}
