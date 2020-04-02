using System;
using System.Collections.Generic;
using System.Text;

namespace ChatDrugiPut.Shared
{
	public class Grupa
	{
		public int ID { get; set; }
		public string Naziv { get; set; }

		public ICollection<UserGrupa> Korisnici { get; set; }

		public Grupa(string n)
		{
			Naziv = n;
		}

		public Grupa() { }
	}
}
