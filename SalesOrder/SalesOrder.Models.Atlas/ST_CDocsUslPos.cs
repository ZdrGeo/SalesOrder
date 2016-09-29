namespace SalesOrder.Models.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ST_CDocsUslPos
    {
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

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Line { get; set; }

        public int? ArtID { get; set; }

        public int UslID { get; set; }

        public int UslTypeID { get; set; }

        [Required]
        [StringLength(128)]
        public string UslText { get; set; }

        public int? B1 { get; set; }

        public int? B2 { get; set; }

        public int? I1 { get; set; }

        public int? I2 { get; set; }

        public double? QtyR { get; set; }

        public double? Qty { get; set; }

        public double? QtySecond { get; set; }

        public double? QtyBlock { get; set; }

        public double? QtyStorno { get; set; }

        public double? PriceLv { get; set; }

        public double? PriceR { get; set; }

        public double? R1 { get; set; }

        public double? R2 { get; set; }

        public double? DiscPerc { get; set; }

        public double? NadcPerc { get; set; }

        public double? TotalLv { get; set; }

        public double? DDSPerc { get; set; }

        public double? PriceVal1 { get; set; }

        public int? CurID1 { get; set; }

        public double? Rate1 { get; set; }

        public double? PriceVal2 { get; set; }

        public int? CurID2 { get; set; }

        public double? Rate2 { get; set; }

        public int PKDocID { get; set; }

        public int PKLine { get; set; }

        public int? RelDocID { get; set; }

        public int? RelLine { get; set; }

        public int? ZaDocID { get; set; }

        public int? ZaLinID { get; set; }

        public short? MailStat { get; set; }

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

        public int? SrvDocID { get; set; }

        public int? SrvLine { get; set; }

        public virtual ST_CDocs ST_CDocs { get; set; }
    }
}
