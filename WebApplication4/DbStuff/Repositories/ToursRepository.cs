using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.DbStuff.DataModels;
using WebPortal.DbStuff.Models.Tourism;


namespace WebPortal.DbStuff.Repositories
{
    public class ToursRepository : BaseRepository<Tours>, IToursRepository
    {
        public ToursRepository(TourismPortalContext portalContext) : base(portalContext)
        {

        }
        public List<Tours> GetShopItemWithAuthor()
        {
            
            return _portalContext
                .Tours
                .Include(x => x.Author)
                .ToList();
        }
        public bool IsUniqName(string? name)
        {
            return !_dbSet.Any(x => x.TourName == name);
        }
        public List<ToursAutorStatiscticModel> GetTourAutor()
        {
            FormattableString fs = $@"
                Select  TourName, UserName as AutorName, CreatedDate
                From Tours T
                Left Join Users U ON T.AuthorId = U.Id
                Order By TourName";
            var response = _portalContext.Database
                .SqlQuery<ToursAutorStatiscticModel>(fs)
                .ToList();
            return response;
        }
    }
}
