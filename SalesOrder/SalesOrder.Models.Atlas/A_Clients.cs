namespace SalesOrder.Models.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class A_Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public A_Clients()
        {
            ST_CDocs = new HashSet<ST_CDocs>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short FirmID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientID { get; set; }

        public int? ClientTypeID { get; set; }

        public short? BranchID { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(15)]
        public string CliGrp { get; set; }

        [StringLength(200)]
        public string ClientName { get; set; }

        [StringLength(200)]
        public string ClientNameEng { get; set; }

        public int? CountryID { get; set; }

        public int? RegID { get; set; }

        public int? GradID { get; set; }

        public int? ObshtinaID { get; set; }

        [StringLength(15)]
        public string ECode { get; set; }

        public int? RegDDS { get; set; }

        [StringLength(15)]
        public string DanNomer { get; set; }

        [StringLength(15)]
        public string Bulstat { get; set; }

        [StringLength(20)]
        public string Grad { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string AddressEng { get; set; }

        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(25)]
        public string Fax { get; set; }

        [StringLength(25)]
        public string EMail { get; set; }

        [StringLength(30)]
        public string BankName { get; set; }

        [StringLength(20)]
        public string BankCode { get; set; }

        [StringLength(20)]
        public string BankAcc { get; set; }

        [StringLength(30)]
        public string MOLName { get; set; }

        [StringLength(50)]
        public string MOLData { get; set; }

        [StringLength(3)]
        public string AEU { get; set; }

        public bool? ViewInNoms { get; set; }

        public int? DelivererID { get; set; }

        [StringLength(20)]
        public string S1 { get; set; }

        [StringLength(20)]
        public string S2 { get; set; }

        [StringLength(20)]
        public string S3 { get; set; }

        [StringLength(20)]
        public string S4 { get; set; }

        [StringLength(20)]
        public string S5 { get; set; }

        [StringLength(30)]
        public string S6 { get; set; }

        [StringLength(30)]
        public string S7 { get; set; }

        [StringLength(30)]
        public string S8 { get; set; }

        public bool? B1 { get; set; }

        public bool? B2 { get; set; }

        public int? I1 { get; set; }

        public int? I2 { get; set; }

        public int? I3 { get; set; }

        public int? I4 { get; set; }

        public int? I5 { get; set; }

        public int? I6 { get; set; }

        public int? I7 { get; set; }

        public int? I8 { get; set; }

        public int? V3_I1 { get; set; }

        public int? V3_I2 { get; set; }

        public int? V3_I3 { get; set; }

        public int? V3_I4 { get; set; }

        public int? V3_I5 { get; set; }

        public int? V3_I6 { get; set; }

        public int? V3_I7 { get; set; }

        public int? V3_I8 { get; set; }

        public int? V3_I9 { get; set; }

        public int? V3_I10 { get; set; }

        public DateTime? V3_DT1 { get; set; }

        public DateTime? V3_DT2 { get; set; }

        public DateTime? V3_DT3 { get; set; }

        public DateTime? V3_DT4 { get; set; }

        public DateTime? V3_DT5 { get; set; }

        public double? V3_F1 { get; set; }

        public double? V3_F2 { get; set; }

        public double? V3_F3 { get; set; }

        [StringLength(50)]
        public string V3_S1 { get; set; }

        [StringLength(50)]
        public string V3_S2 { get; set; }

        [StringLength(50)]
        public string V3_S3 { get; set; }

        [StringLength(50)]
        public string V3_S4 { get; set; }

        [StringLength(50)]
        public string V3_S5 { get; set; }

        [StringLength(50)]
        public string V3_S6 { get; set; }

        [StringLength(50)]
        public string V3_S7 { get; set; }

        public int? PayType1ID { get; set; }

        public int? PayNote1ID { get; set; }

        public int? RabType1 { get; set; }

        public int? PayType2ID { get; set; }

        public int? PayNote2ID { get; set; }

        public int? RabType2 { get; set; }

        public int? PayTypeIDNZOK { get; set; }

        public int? PayNoteIDNZOK { get; set; }

        public int? RabTypeNZOK { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ST_CDocs> ST_CDocs { get; set; }
    }
}
