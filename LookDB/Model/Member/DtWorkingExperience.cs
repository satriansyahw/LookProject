using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtWorkingExperience", Schema = "mem")]
    public class DtWorkingExperience
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string CompName { get; set; }
        [Column(Order = 3),  MaxLength(100)]
        public string CompField { get; set; }
        [Column(Order = 4), MaxLength(50)]
        public string Dept { get; set; }
        [Column(Order = 5),  MaxLength(30)]
        public string Position { get; set; }
        [Column(Order = 6), MaxLength(30)]
        public string Specialization { get; set; }
        [Column(Order = 7), MaxLength(100)]
        public string Description { get; set; }
        [Column(Order = 8), MaxLength(6)]
        public string WorkStart { get; set; }
        [Column(Order = 9), MaxLength(6)]
        public string WorkEnd { get; set; }
        [Column(Order = 10), Required,Default(DefaultValue =false)]
        public bool OnWork { get; set; }
        [Column(Order = 11), MaxLength(50)]
        public string FileSupport { get; set; }


        [Column(Order = 12), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 13), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 14), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 15)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 16), Required]
        public bool ActiveBool { get; set; }
    }
  
}
