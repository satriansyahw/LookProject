using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtCompany", Schema = "mem")]
    public class DtCompany
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1), Required,MaxLength(13), Index(IsClustered = true, IsUnique = true)]
        public string CompanyNoReg { get; set; }
        [Column(Order = 2), Required,MaxLength(30)]
        public string CompName { get; set; }
        [Column(Order = 3), MaxLength(30)]
        public string CompField { get; set; }
        [Column(Order = 4), MaxLength(200)]
        public string ShortProfile { get; set; }
        [Column(Order = 5), MaxLength(100)]
        public string CompanyAddr { get; set; }
        [Column(Order = 6),  MaxLength(30)]
        public string CompCity { get; set; }
        [Column(Order = 7),  MaxLength(30)]
        public string CompProv { get; set; }
        [Column(Order = 8), MaxLength(20)]
        public string NPWP { get; set; }
        [Column(Order = 9), MaxLength(30)]
        public string ContactName1 { get; set; }
        [Column(Order = 10),  MaxLength(15)]
        public string ContactHP1 { get; set; }
        [Column(Order = 11), MaxLength(60)]
        public bool ContactEmail1 { get; set; }
        [Column(Order = 12), MaxLength(30)]
        public string ContactName2 { get; set; }
        [Column(Order = 13), MaxLength(15)]
        public string ContactHP2 { get; set; }
        [Column(Order = 14), MaxLength(60)]
        public string ContactEmail2 { get; set; }
        [Column(Order = 15), MaxLength(100)]
        public string CompPortal { get; set; }
        [Column(Order = 16), MaxLength(1)]
        public string MemberType { get; set; }
        [Column(Order = 17), MaxLength(10)]
        public string SubscribeStart { get; set; }
        [Column(Order = 18), MaxLength(10)]
        public string SubscribeEnd { get; set; }
        [Column(Order = 19),Default(DefaultValue =true)]
        public bool EffectiveBool { get; set; }

        [Column(Order = 20), MaxLength(18)]
        public string InsertBy { get; set; }
        [Column(Order = 21), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 22), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 23)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 24), Required]
        public bool ActiveBool { get; set; }
    }
  
}
