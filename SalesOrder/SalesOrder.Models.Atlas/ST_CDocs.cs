namespace SalesOrder.Models.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ST_CDocs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ST_CDocs()
        {
            ST_CDocsPays = new HashSet<ST_CDocsPays>();
            ST_CDocsPos = new HashSet<ST_CDocsPos>();
            ST_CDocsUslPos = new HashSet<ST_CDocsUslPos>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short FirmID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short BranchID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocID { get; set; }

        public short DocTypeID { get; set; }

        public short DocNoID { get; set; }

        [StringLength(30)]
        public string DocNo { get; set; }

        public short? DocNo1ID { get; set; }

        [StringLength(30)]
        public string DocNo1 { get; set; }

        [StringLength(30)]
        public string IDEad { get; set; }

        public DateTime? EadDate { get; set; }

        [StringLength(30)]
        public string InvNo { get; set; }

        public DateTime InputDate { get; set; }

        public DateTime DocDate { get; set; }

        public int SrcStoreID { get; set; }

        public int ClientID { get; set; }

        public int ClientIDofZa { get; set; }

        public int? ObjRefID1 { get; set; }

        public int? ObjRefID2 { get; set; }

        public int? ObjRefID3 { get; set; }

        public int? ObjRefID4 { get; set; }

        public int? ObjRefID5 { get; set; }

        public int? ObjRefID6 { get; set; }

        public int? CurID { get; set; }

        public double? Rate { get; set; }

        public double? SumaPosLv { get; set; }

        public double? SumaPosVal { get; set; }

        public double? DiscPerc { get; set; }

        public int SumaPayStat { get; set; }

        public double? SumaPayLv { get; set; }

        public double? SumaPayVal { get; set; }

        public double? DDSPerc { get; set; }

        public double? SumaDDSPosLv { get; set; }

        public double? SumaDDSPosVal { get; set; }

        public double? TotalLv { get; set; }

        public double? TotalVal { get; set; }

        public double? PaidVal { get; set; }

        public double? PaidLvInv { get; set; }

        public double? PaidLvReal { get; set; }

        public int Pay_Stat { get; set; }

        [StringLength(50)]
        public string Note { get; set; }

        public int? PayTypeID { get; set; }

        public DateTime? DatePadej { get; set; }

        public DateTime? LastPayDate { get; set; }

        public int? PayNoteID { get; set; }

        public int? PayReasonID { get; set; }

        public int StatusOS { get; set; }

        [StringLength(16)]
        public string UserID { get; set; }

        public short? MailStat { get; set; }

        public int? Stat1 { get; set; }

        public int? Stat2 { get; set; }

        public int? Stat3 { get; set; }

        public int? Stat4 { get; set; }

        public int? Stat5 { get; set; }

        public DateTime? Dt1 { get; set; }

        public DateTime? Dt2 { get; set; }

        public DateTime? Dt3 { get; set; }

        public DateTime? Dt4 { get; set; }

        public DateTime? Dt5 { get; set; }

        public int? I1 { get; set; }

        public int? I2 { get; set; }

        public double? R1 { get; set; }

        public double? R2 { get; set; }

        public int RabType { get; set; }

        public int RabDocID { get; set; }

        public double RabSuma { get; set; }

        public double RabSumaExec { get; set; }

        public int? DestBranchID { get; set; }

        public int? SrcBranchID { get; set; }

        public int? SrcBranchDocID { get; set; }

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

        public double? V3_F4 { get; set; }

        public double? V3_F5 { get; set; }

        public double? V3_F6 { get; set; }

        public double? V3_F7 { get; set; }

        public double? V3_F8 { get; set; }

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

        public int? V3_ReprID { get; set; }

        public int? V3_SupervID { get; set; }

        public int? V3_RMID { get; set; }

        public int? V3_ATLDocID { get; set; }

        public int? SrvDescID { get; set; }

        public int? SrvID { get; set; }

        public int? SrvLine { get; set; }

        public virtual A_Clients A_Clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ST_CDocsPays> ST_CDocsPays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ST_CDocsPos> ST_CDocsPos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ST_CDocsUslPos> ST_CDocsUslPos { get; set; }
    }
}
