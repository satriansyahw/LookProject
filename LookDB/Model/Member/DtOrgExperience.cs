using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtOrgExperience", Schema = "mem")]
    public class DtOrgExperience
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string OrgName { get; set; }
        [Column(Order = 3),  MaxLength(30)]
        public string Position { get; set; }
        [Column(Order = 4), MaxLength(100)]
        public string Description { get; set; }
        [Column(Order = 5), MaxLength(6)]
        public string OrgStart { get; set; }
        [Column(Order = 6), MaxLength(6)]
        public string OrgEnd { get; set; }
        [Column(Order = 7), Required,Default(DefaultValue =false)]
        public bool OnOrg { get; set; }
        [Column(Order = 8), MaxLength(50)]
        public string FileSupport { get; set; }


        [Column(Order = 9), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 10), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 11), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 12)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 13), Required]
        public bool ActiveBool { get; set; }
    }
  
}
