using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtLanguage", Schema = "mem")]
    public class DtLanguage
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string LangName { get; set; }
        [Column(Order = 3), MaxLength(6)]
        public Int16 LangLevel { get; set; }
 

        [Column(Order = 4), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 5), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 6), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 7)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 8), Required]
        public bool ActiveBool { get; set; }
    }
  
}
