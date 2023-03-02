using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    [Table("Products")]
    public class ProductModel
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Category { get; set; }

        [Required]
        [StringLength(255)]
        public string Brand { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Price { get; set; }

        [Required]
        public int AllInStock { get; set; }

        [Column(TypeName = "text")]
        public string? Image { get; set; }

        public int? NumberOfReviews { get; set; }

        [Column(TypeName = "float")]
        public float? Rating { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public void SetValues(dynamic options)
        {
            Console.WriteLine("set kobe: {0}", options);
            Id = options.Id ?? Id;
            Title = options.Title ?? Title;
            Category = options.Category ?? Category;
            Brand = options.Brand ?? Brand;
            Price = options.Price ?? Price;
            AllInStock = options.AllInStock ?? AllInStock;
        }
    }


    [Table("ByProductDetails")]
    public class ByProductDetailModel
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Category { get; set; }

        [Required]
        [StringLength(50)]
        public string Size { get; set; }

        [Required]
        [StringLength(50)]
        public string Color { get; set; }

        [Required]
        public int InStock { get; set; }

        public string ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
    }
}
