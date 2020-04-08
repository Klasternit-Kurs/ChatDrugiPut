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
			optionsBuilder.UseSqlServer("Data Source=DESKTOP-75VO5EN;Initial Catalog = ChatBaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
		}
	}
}
