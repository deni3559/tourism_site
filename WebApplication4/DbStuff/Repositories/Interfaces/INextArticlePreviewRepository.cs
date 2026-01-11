using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff.Repositories.Interfaces
{
    public interface INextArticlePreviewRepository : IBaseRepository<NextArticlePreview>
    {
        List<NextArticlePreview> GetArticleList();
    }
}