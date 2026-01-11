using Microsoft.EntityFrameworkCore;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;
using WebPortal.DbStuff.Repositories.Interfaces;

namespace WebPortal.DbStuff.Repositories
{
    public class ShopCartRepository : BaseRepository<ShopCart>, IShopCartRepository
    {
        public ShopCartRepository(TourismPortalContext portalContext) : base(portalContext)
        {
        }

        public List<ShopCart> GetToursAddedInShopCart()
        {
            return _portalContext
                .ShopCart
                .Include(x => x.TourInShop)
                .ThenInclude(x => x.Author)
                .ToList();
        }


        public List<ShopCart> GetUniqueTourAddedInShopCart()
        {
            return _portalContext
                .ShopCart
                .Include(x => x.TourInShop)
                .ThenInclude(x => x.Author)
                .GroupBy(x => x.TourInShopId)
                .Select(x => x.First())
                .ToList();
        }

        public int GetCountOfUniqueTourAddedInShopCart(int tourId, int userId)
        {
            return _portalContext
                .ShopCart
                 .Include(x => x.TourInShop)
                .Where(x => x.UserId == userId)
                .Count(x => x.TourInShopId == tourId);
        }
        public List<ShopCart> GetUserShopCart(int userId)
        {
            return _portalContext.ShopCart
                .Include(x => x.TourInShop)
                    .ThenInclude(t => t.Author)
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.TourInShopId)
                .Select(x => x.First())
                .ToList();
        }

        public double GetSumPriceOfUniqueAddedItem(int tourId)
        {
            return _portalContext.ShopCart
            .Where(x => x.TourInShopId == tourId)
            .Sum(x => x.TourInShop.Price);
        }

        public double SumPriceInShopCart()
        {
            return _portalContext
                .ShopCart
                .Include(x => x.TourInShop)
                .Sum(x => x.TourInShop.Price);
        }
    }
}
