using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, List<string> roles, List<string> permissions);
        string GenerateRefreshToken();
    }
}
