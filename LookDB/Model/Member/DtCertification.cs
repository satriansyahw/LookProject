using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtCertification", Schema = "mem")]
    public class DtCertification
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2),Required, MaxLength(50)]
        public string CertName { get; set; }
        [Column(Order = 3), MaxLength(100)]
        public string Description { get; set; }
        [Column(Order = 4), MaxLength(6)]
        public string CertStart { get; set; }
        [Column(Order = 5), MaxLength(6)]
        public string CertEnd { get; set; }
        [Column(Order = 6), MaxLength(50)]
        public string FileSupport { get; set; }

        [Column(Order = 7), MaxLength(30)]
        public string InsertBy { get; set; }
        [Column(Order = 8), Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }
        [Column(Order = 9), MaxLength(30)]
        public string UpdateBy { get; set; }
        [Column(Order = 10)]
        public DateTime UpdateDate { get; set; }
        [Column(Order = 11), Required]
        public bool ActiveBool { get; set; }
    }

    public class VW_DtCertification
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public string CertName { get; set; }
        public string Description { get; set; }
        public string CertStart { get; set; }
        public string CertEnd { get; set; }
        public string FileSupport { get; set; }
    }

}
