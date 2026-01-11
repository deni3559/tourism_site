using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.Services.Permissions
{
    public interface IArticlePermission
    {
        bool CanDelete(NextArticlePreview dbData);
    }
}