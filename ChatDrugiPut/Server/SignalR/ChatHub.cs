using ChatDrugiPut.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDrugiPut.Server.SignalR
{
	public class ChatHub : Hub
	{
		public async Task Join(Poruka por)
		{
			EF.Baza DB = new EF.Baza();
			var kor = DB.Users.Where(u => u == por.Posiljaoc).First();
			var gru = DB.Grupas.Where(g => g.Naziv == por.Sadrzaj).FirstOrDefault();
			if (gru == null)
			{
				gru = new Grupa(por.Sadrzaj);
				DB.Grupas.Add(gru);
				DB.UG.Add(new UserGrupa(kor, gru));
			}
			else
			{
				DB.UG.Add(new UserGrupa(kor, gru));
			}
			try
			{
				await DB.SaveChangesAsync();
				await Groups.AddToGroupAsync(Context.ConnectionId, por.Sadrzaj);
				await Clients.Group(por.Sadrzaj).SendAsync("PorukaKaKlijentu", new Poruka($"Korisnik {por.Posiljaoc.Username} se prikljucuje grupi :).", null, por.Sadrzaj));
				await Clients.Group(por.Sadrzaj).SendAsync("OsveziGrupu", gru.Naziv, DB.UG.Where(ug => ug.Gru.Naziv == gru.Naziv).Count());
			}
			catch { }

			
			
		}

		public async Task Leave(Poruka p)
		{
			try
			{
				EF.Baza DB = new EF.Baza();
				DB.UG.Where(ug => ug.Gru.Naziv == p.Sadrzaj).ToList();
				var k = DB.Users.Where(u => u.Equals(p.Posiljaoc)).First();
				var g = DB.Grupas.Where(g => g.Naziv == p.Sadrzaj).First();
				var ug = DB.UG.Where(ug => ug.Gru.Naziv == p.Sadrzaj && ug.Kor.Username == p.Posiljaoc.Username).First();
				g.Korisnici.Remove(ug);
				k.AktivneGrupe.Remove(ug);
				if (g.Korisnici.Count == 0)
					DB.Grupas.Remove(g);
			
				await DB.SaveChangesAsync();
				await DobaviGrupe(p.Posiljaoc);
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, g.Naziv);
				await Clients.Group(g.Naziv).SendAsync("PorukaKaKlijentu", new Poruka($"Korisnik {k.Username} napusta grupu :).", null, g.Naziv));
				await Clients.Group(g.Naziv).SendAsync("OsveziGrupu", g.Naziv, DB.UG.Where(ug => ug.Gru.Naziv == g.Naziv).Count());
			}
			catch ( SqlException e )
			{
				Console.WriteLine("Greska sa bazom! -- " + e.ToString());
			}
		}

		public async Task PrimiPoruku(Poruka por)
		{
			if (por.Grupa == null)
				Clients.All.SendAsync("PorukaKaKlijentu", por);
			else
			{
				EF.Baza DB = new EF.Baza();
				if (DB.UG.Where(ug => ug.Gru.Naziv == por.Grupa && ug.Kor.Equals(por.Posiljaoc)).Count() != 0)
					Clients.Group(por.Grupa).SendAsync("PorukaKaKlijentu", por);
			}
		}

		public async Task PrihvatiKorisnika (User u)
		{
			EF.Baza DB = new EF.Baza();
			DB.Add(u);
			DB.SaveChanges();
		}

		public async Task ProveriKorisnika (User LogIn)
		{
			EF.Baza DB = new EF.Baza();
			
			var juzer = DB.Users.Where(us => us.Equals(LogIn)).FirstOrDefault();
			
			if (juzer != null)
				await Clients.Caller.SendAsync("EvoDobrog", juzer);

		}

		public async Task DobaviGrupe (User kor)
		{
			EF.Baza DB = new EF.Baza();
			List<string> grupe = new List<string>();
			DB.Users.Where(p => p.Equals(kor))
					.SelectMany(p => p.AktivneGrupe)
					.Select(pc => pc.Gru).ToList().ForEach(g => grupe.Add(g.Naziv));

			List<int> Korisnici = new List<int>();
			foreach (string gr in grupe)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, gr);
				Korisnici.Add(DB.UG.Where(ug => ug.Gru.Naziv == gr).Count());
			}

			await Clients.Caller.SendAsync("EvoGrupe", grupe, Korisnici);
		}
	}
}
