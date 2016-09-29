namespace SalesOrder.Models.Atlas
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Atlas : DbContext
    {
        public Atlas() : base ("name=Atlas") { }

        public virtual DbSet<A_Clients> A_Clients { get; set; }
        public virtual DbSet<ST_CDocs> ST_CDocs { get; set; }
        public virtual DbSet<ST_CDocsPays> ST_CDocsPays { get; set; }
        public virtual DbSet<ST_CDocsPos> ST_CDocsPos { get; set; }
        public virtual DbSet<ST_CDocsUslPos> ST_CDocsUslPos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Region)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.CliGrp)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.ClientName)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.ClientNameEng)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.ECode)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.DanNomer)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Bulstat)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Grad)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Address)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.AddressEng)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.Fax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.EMail)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.BankName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.BankCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.BankAcc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.MOLName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.MOLData)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.AEU)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S1)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S2)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S3)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S4)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S5)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S6)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .Property(e => e.V3_S7)
                .IsUnicode(false);

            modelBuilder.Entity<A_Clients>()
                .HasMany(e => e.ST_CDocs)
                .WithRequired(e => e.A_Clients)
                .HasForeignKey(e => new { e.FirmID, e.ClientID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ST_CDocs>()
                .Property(e => e.V3_S1)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocs>()
                .Property(e => e.V3_S2)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocs>()
                .Property(e => e.V3_S3)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocs>()
                .Property(e => e.V3_S4)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocs>()
                .Property(e => e.V3_S5)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocs>()
                .HasMany(e => e.ST_CDocsPays)
                .WithRequired(e => e.ST_CDocs)
                .HasForeignKey(e => new { e.FirmID, e.BranchID, e.DocID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ST_CDocs>()
                .HasMany(e => e.ST_CDocsPos)
                .WithRequired(e => e.ST_CDocs)
                .HasForeignKey(e => new { e.FirmID, e.BranchID, e.DocID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ST_CDocs>()
                .HasMany(e => e.ST_CDocsUslPos)
                .WithRequired(e => e.ST_CDocs)
                .HasForeignKey(e => new { e.FirmID, e.BranchID, e.DocID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ST_CDocsPays>()
                .Property(e => e.V3_S1)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsPos>()
                .Property(e => e.V3_S1)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsPos>()
                .Property(e => e.V3_S2)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsPos>()
                .Property(e => e.V3_S3)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsPos>()
                .Property(e => e.V3_S4)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsUslPos>()
                .Property(e => e.V3_S1)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsUslPos>()
                .Property(e => e.V3_S2)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsUslPos>()
                .Property(e => e.V3_S3)
                .IsUnicode(false);

            modelBuilder.Entity<ST_CDocsUslPos>()
                .Property(e => e.V3_S4)
                .IsUnicode(false);
        }
    }
}
