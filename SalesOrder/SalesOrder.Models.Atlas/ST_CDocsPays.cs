namespace SalesOrder.Models.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ST_CDocsPays
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

        public DateTime? PayDate { get; set; }

        public short? PayNoteID { get; set; }

        public int? PayCurID { get; set; }

        public double PayRate { get; set; }

        public double CrossRate { get; set; }

        public double? PaySumVal { get; set; }

        public int? StatusOS { get; set; }

        [StringLength(16)]
        public string UserID { get; set; }

        public int? V3_I1 { get; set; }

        public int? V3_I2 { get; set; }

        public int? V3_I3 { get; set; }

        public int? V3_I4 { get; set; }

        public int? V3_I5 { get; set; }

        public DateTime? V3_DT1 { get; set; }

        public DateTime? V3_DT2 { get; set; }

        public DateTime? V3_DT3 { get; set; }

        public double? V3_F1 { get; set; }

        public double? V3_F2 { get; set; }

        public double? V3_F3 { get; set; }

        public double? V3_F4 { get; set; }

        [StringLength(50)]
        public string V3_S1 { get; set; }

        public virtual ST_CDocs ST_CDocs { get; set; }
    }
}
