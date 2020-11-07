using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.UserService
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }
}
