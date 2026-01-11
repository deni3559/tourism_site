namespace WebPortal.DbStuff.Models.Tourism
{
    public class Tours : BaseModel
    {
        public string TourName { get; set; }
        public string TourImgUrl { get; set; }
        public virtual User? Author { get; set; } 
        public double Price {  get; set; }
        public string? Description {  get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual List<ShopCart> ToursInShopCart { get; set; } = new List<ShopCart>();
    }
}
