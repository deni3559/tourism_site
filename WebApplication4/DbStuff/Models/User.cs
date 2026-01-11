using WebPortal.Enum;
using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Money { get; set; }
        public Role Role { get; set; }
        public Language Language { get; set; }
        public virtual List<Tours> CreatedTours { get; set; } = new List<Tours>();
        public virtual List<ShopCart>? AddedToursInShopCart { get; set; } = new List<ShopCart>();
    }
}
