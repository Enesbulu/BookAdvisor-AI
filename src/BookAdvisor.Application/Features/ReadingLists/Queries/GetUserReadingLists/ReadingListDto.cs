using System;
using System.Collections.Generic;
using System.Text;

namespace BookAdvisor.Application.Features.ReadingLists.Queries.GetUserReadingLists
{
    public record ReadingListDto(Guid id, string Name, int BookCount);
}
