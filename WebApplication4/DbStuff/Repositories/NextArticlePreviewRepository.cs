using Microsoft.AspNetCore.Mvc;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff.Repositories
{
    public class NextArticlePreviewRepository : BaseRepository<NextArticlePreview>, INextArticlePreviewRepository
    {

        public NextArticlePreviewRepository(TourismPortalContext portalContext) : base(portalContext)
        {
        }

        public List<NextArticlePreview> GetArticleList()
        {
            return _portalContext.
                NextArticlePreview.
                ToList();
        }

    }
}
