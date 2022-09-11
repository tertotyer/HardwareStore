using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Areas
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = $"Email '{email}' уже занят.";
            return error;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "";
            return error;
        }        
    }
}
