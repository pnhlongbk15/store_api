using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    [Table("Carts")]
    public class CartModel
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float totalPrice { get; set; }

        public string UserId { get; set; }
        [Required]
        public UserModel User { get; set; }
    }

    [Table("DetailCarts")]
    public class DetailCartModel
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Price { get; set; }

        public string OrderId { get; set; }
        [Required]
        public CartModel Order { get; set; }

        public string ByProductDetailId { get; set; }
        [Required]
        public ByProductDetailModel ByProductDetail { get; set; }
    }
}
