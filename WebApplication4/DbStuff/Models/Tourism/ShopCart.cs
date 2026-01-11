namespace WebPortal.DbStuff.Models.Tourism
{
    public class ShopCart : BaseModel
    {
        public int TourInShopId { get; set; }
        public virtual Tours TourInShop { get; set; }
        public int UserId { get; set; }
        public virtual User? UserWhoAddTheTour { get; set; }
    }
}
