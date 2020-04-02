using ChatDrugiPut.Shared;
using Microsoft.AspNetCore.SignalR;
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
			await Groups.AddToGroupAsync(Context.ConnectionId, por.Sadrzaj);
			Clients.Group(por.Sadrzaj).SendAsync("PorukaKaKlijentu", new Poruka($"Korisnik {por.Posiljaoc.Username} se prikljucuje grupi :).", null, por.Sadrzaj));
		}

		public async Task PrimiPoruku(Poruka por)
		{
			if (por.Grupa == null)
				Clients.All.SendAsync("PorukaKaKlijentu", por);
			else
				Clients.Group(por.Grupa).SendAsync("PorukaKaKlijentu", por);
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
	}
}
