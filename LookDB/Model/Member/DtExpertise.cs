using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtExpertise", Schema = "mem")]
    public class DtExpertise
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string ExpertName { get; set; }
        [Column(Order = 3), MaxLength(100)]
        public string Description { get; set; }
        [Column(Order = 4), MaxLength(6)]
        public Int16 ExpertLevel { get; set; }
        [Column(Order = 5), MaxLength(50),  Index(IsClustered = false, IsUnique = false)]
        public string FileSupport { get; set; }


        [Column(Order = 6), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 7), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 8), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 9)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 10), Required]
        public bool ActiveBool { get; set; }
    }
    public class VW_DtExpertise
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public string ExpertName { get; set; }
        public string Description { get; set; }
        public Int16 ExpertLevel { get; set; }
        public string FileSupport { get; set; }
    }
}
