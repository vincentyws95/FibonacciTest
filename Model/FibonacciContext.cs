using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FibonacciTest.Model
{
    public class FibonacciContext : DbContext
    {
        public DbSet<RunSummary> RunSummary { get; set; }
        public DbSet<RunOutput> RunOutput { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(@$"Server=your_server_name;Database=Fibonacci;Encrypt=false;Integrated Security=True;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RunSummary>().ToTable("run_summary").HasKey(rs => rs.RunId);
            modelBuilder.Entity<RunOutput>().ToTable("run_output").HasKey(ro => ro.Id); 

            modelBuilder.Entity<RunOutput>()
           .HasOne(ro => ro.RunSummary)
           .WithOne(rs => rs.RunOutput)
           .HasForeignKey<RunSummary>(ro => ro.RunId);
        }
    }

    public class RunSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("run_id")]
        public int RunId { get; set; }

        [Column("run_datetime")]
        public DateTime RunDateTime { get; set; }
        public string UserName { get; set; }

        [Column("sequence_count")]
        public int SequenceCount { get; set; }

        public RunOutput RunOutput { get; set; }

    }

    public class RunOutput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("RunSummary")]
        [Column("run_id")]
        public int RunId { get; set; }
        public string Output { get; set; }
        public RunSummary RunSummary { get; set; }
    }
}
