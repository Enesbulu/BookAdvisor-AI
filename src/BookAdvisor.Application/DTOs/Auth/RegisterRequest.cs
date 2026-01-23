using System;
using System.Collections.Generic;
using System.Text;

namespace BookAdvisor.Application.DTOs.Auth
{
    public record RegisterRequest(
            string FirstName,
            string LastName,
            string Email,
            string Password
        );
}
