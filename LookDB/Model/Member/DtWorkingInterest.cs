using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static LookDB.LookDBContext;

namespace LookDB.Model.Member
{
    [Table("DtWorkingInterest", Schema = "mem")]
    public class DtWorkingInterest
    {
        [Column(Order = 0), Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 1), Required]
        public int MemberID { get; set; }

        [Column(Order = 2), MaxLength(30)]
        public string Posisi { get; set; }

        [Column(Order = 3), MaxLength(50)]
        public string Dept { get; set; }

        [Column(Order = 4), MaxLength(50)]
        public string Description { get; set; }

        [Column(Order = 5), MaxLength(50)]
        public string Location { get; set; }

        [Column(Order = 6), Required, MaxLength(50)]
        public int Salary { get; set; }

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

    public class VW_DtWorkingInterest
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public string Posisi { get; set; }
        public string Dept { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Salary { get; set; }
        public string InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool ActiveBool { get; set; }
    }
}
