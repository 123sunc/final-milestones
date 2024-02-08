using System.ComponentModel.DataAnnotations;

namespace SKYResturant.Models
{
    public class BasketItem
    {
        [Required]
        public int StockID { get; set; }
        [Required]
        public int BasketID { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
