using WebPortal.DbStuff.Models.Tourism;
using WebPortal.DbStuff.Repositories;
using WebPortal.Enum;

namespace WebPortal.Services.Permissions
{
    public class ArticlePermission :IArticlePermission
    {
        private IAuthService _authService;

        public ArticlePermission(IAuthService authService)
        {
            _authService = authService;
        }

        public bool CanDelete(NextArticlePreview article)
        {
            if (!_authService.IsAuthenticated())
            {
                return false;
            }

            var user = _authService.GetUser();
            var userRightForDeletion = user.Role == Role.Admin;
            return userRightForDeletion;
        }
    }
}
