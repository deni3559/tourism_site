using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebPortal.Models.CustomValidationAttributtes;

namespace WebPortal.Models.Tourism
{
    public class ShopCartViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public double SumPriceOfAllItemsInShopCart { get; set; }
        public double SumPriceOfUniqueAddedItemInShopCart { get; set; }
        public List<TourViewModel> Tours { get; set; } = new List<TourViewModel>();
    }
}
