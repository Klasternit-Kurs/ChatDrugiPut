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

		public async Task PrimiPoruku(Poruka por)
		{
			Console.WriteLine("U metodi :) ");
			Console.WriteLine($"{por.Posiljaoc.Username}: {por.Sadrzaj}");
			await Clients.Caller.SendAsync("PorukaKaKlijentu", por);
			await Clients.Others.SendAsync("PorukaKaKlijentu", por);
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
