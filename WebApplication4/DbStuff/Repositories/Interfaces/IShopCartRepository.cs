using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff.Repositories.Interfaces
{
    public interface IShopCartRepository: IBaseRepository<ShopCart>
    {
        int GetCountOfUniqueTourAddedInShopCart(int tourId, int userId);
        List<ShopCart> GetToursAddedInAllShopCart();
        List<int> GetListAllTorsShopCart(int userId);
        List<ShopCart> GetUserShopCart(int userId);
    }
}