using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    [Table("Users")]
    public class UserModel : IdentityUser
    {
        [StringLength(255)]
        public string? Sex { get; set; }


        [DataType(DataType.Date)]
        public DateTime? Birth { get; set; }


        [Column(TypeName = "text")]
        public string? Image { get; set; }

        public void PrintInfo() { Console.WriteLine($"Ketqua: {base.UserName}-{base.Email}"); }
    }


    [Table("Address")]
    public class AddressModel
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }


        [Required]
        [StringLength(255)]
        public string Fullname { get; set; }


        [Required]
        [StringLength(255)]
        public string PhoneNumber { get; set; }


        [Required]
        [Column(TypeName = "text")]
        public string Address { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public string UserId { get; set; }
        [Required]
        public UserModel User { get; set; }

        public void SetValues(dynamic options)
        {
            Console.WriteLine("set Address: {0}",options);
            Id = options.Id;
            Fullname = options.Fullname;
            PhoneNumber = options.PhoneNumber;
            Address = options.Address;
            IsDefault = options.IsDefault;
            UserId = options.UserId;
        }
    }
}
