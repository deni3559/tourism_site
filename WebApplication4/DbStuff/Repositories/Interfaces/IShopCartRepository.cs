using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff.Repositories.Interfaces
{
    public interface IShopCartRepository: IBaseRepository<ShopCart>
    {
        int GetCountOfUniqueTourAddedInShopCart(int tourId, int userId);
        double GetSumPriceOfUniqueAddedItem(int id);
        List<ShopCart> GetToursAddedInShopCart();
        List<ShopCart> GetUniqueTourAddedInShopCart();
        List<ShopCart> GetUserShopCart(int userId);
        public double SumPriceInShopCart();
    }
}