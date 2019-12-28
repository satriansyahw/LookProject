using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtMember", Schema = "mem")]
    public class DtMember
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1), Required,MaxLength(13), Index(IsClustered = true, IsUnique = true)]
        public string MemberNoReg { get; set; }
        [Column(Order = 2), Required,MaxLength(30)]
        public string FrontName { get; set; }
        [Column(Order = 3), MaxLength(30)]
        public string BackName { get; set; }
        [Column(Order = 4), MaxLength(60)]
        public string FullName { get; set; }
        [Column(Order = 5), Required, MaxLength(20)]
        public string IDCardNo { get; set; }
        [Column(Order = 6), Required, MaxLength(15)]
        public string HP { get; set; }
        [Column(Order = 7), Required, MaxLength(60)]
        public string Email { get; set; }
        [Column(Order = 8), Required, MaxLength(10)]
        public string DateBirth { get; set; }
        [Column(Order = 9), MaxLength(30)]
        public string PlaceBirth { get; set; }
        [Column(Order = 10), Required, MaxLength(1)]
        public string Sex { get; set; }
        [Column(Order = 11), Required]
        public bool Marital { get; set; }
        [Column(Order = 12), MaxLength(100)]
        public string Address { get; set; }
        [Column(Order = 13), MaxLength(30)]
        public string AddressCity { get; set; }
        [Column(Order = 14), MaxLength(30)]
        public string AddressProv { get; set; }
        [Column(Order = 15), MaxLength(50)]
        public string Photo { get; set; }

        [Column(Order = 16), MaxLength(18)]
        public string InsertBy { get; set; }
        [Column(Order = 17), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 18), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 19)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 20), Required]
        public bool ActiveBool { get; set; }
    }
  
        public string BackName { get; set; }
        public string FullName { get; set; }
        public string HP { get; set; }
        public string Email { get; set; }
        public string DateBirth { get; set; }
        public string PlaceBirth { get; set; }
        public string Sex { get; set; }
        public string Photo { get; set; }
    }
}
