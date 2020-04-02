using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDrugiPut.Server.EF
{
	public class Baza : DbContext
	{
		public DbSet<Shared.User> Users { get; set; }
		public DbSet<Shared.Grupa> Grupas { get; set; }
		public DbSet<Shared.UserGrupa> UG { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Shared.User>().HasKey(u => u.Username);
			modelBuilder.Entity<Shared.Grupa>().HasKey(g => g.ID);

			modelBuilder.Entity<Shared.UserGrupa>().HasKey(ug => new { ug.GruId, ug.KorId });
			modelBuilder.Entity<Shared.UserGrupa>().HasOne(ug => ug.Kor).WithMany(k => k.AktivneGrupe).HasForeignKey(ug => ug.KorId);
			modelBuilder.Entity<Shared.UserGrupa>().HasOne(ug => ug.Gru).WithMany(k => k.Korisnici).HasForeignKey(ug => ug.GruId);

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=tcp:blazortest.database.windows.net,1433;Initial Catalog=ChatApp;Persist Security Info=False;User ID=blazor;Password=#12!sifra!21#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		}
	}
}
