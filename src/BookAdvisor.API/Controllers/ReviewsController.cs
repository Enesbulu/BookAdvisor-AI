using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.Reviews.Commands.AddReview;
using BookAdvisor.Application.Features.Reviews.Queries.GetReviewsByBookId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    [Authorize]
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/reviews
        [HttpPost]
        public async Task<IActionResult> AddReviews([FromBody] AddReviewCommand command)
        {
            var reviewId = await _mediator.Send(command);
            return Ok(new
            {
                Id = reviewId,
                Message = ApplicationMessages.AddingReviewSuccess
            });
        }

        //GET api/reviews/{bookId}
        [HttpGet("{bookId}")]
        [AllowAnonymous]    //anonim kişiler de yorum ve puanlamayı görebilmeli
        public async Task<IActionResult> GetReviews(Guid bookId)
        {

            var query = new GetReviewsByBookIdQuery(bookId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
