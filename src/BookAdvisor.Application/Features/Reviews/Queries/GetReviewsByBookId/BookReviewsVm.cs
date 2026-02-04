namespace BookAdvisor.Application.Features.Reviews.Queries.GetReviewsByBookId;

public class BookReviewsVm
{
    public List<ReviewDto> Reviews { get; set; }
    public int TotalCount { get; set; }
    public double AvarageRating { get; set; }
}
