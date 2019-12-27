using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtEducation", Schema = "mem")]
    public class DtEducation
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string Institution { get; set; }
        [Column(Order = 3), MaxLength(100)]
        public string InstitutionLocation { get; set; }
        [Column(Order = 4), MaxLength(50)]
        public string Major { get; set; }
        [Column(Order = 5), MaxLength(4)]
        public string IPK { get; set; }
        [Column(Order = 6), Required, MaxLength(6)]
        public string StudyStart { get; set; }
        [Column(Order = 7), MaxLength(6)]
        public string StudyEnd { get; set; }
        [Column(Order = 8), Required,Default(DefaultValue =false)]
        public bool OnStudy { get; set; }
        [Column(Order = 9), MaxLength(50)  ,Index(IsClustered = false, IsUnique = false)]
        public string FileSupport { get; set; }


        [Column(Order = 10), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 11), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 12), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 13)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 14), Required]
        public bool ActiveBool { get; set; }
    }
  
}
