using System;
using System.Collections.Generic;
using System.Text;

namespace HRManagement.Application.Common.Exceptions
{
    public sealed class UnauthorizedException(string message):Exception(message)
    {
    }
}
