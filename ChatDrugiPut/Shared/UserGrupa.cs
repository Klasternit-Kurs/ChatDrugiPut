using System;
using System.Collections.Generic;
using System.Text;

namespace ChatDrugiPut.Shared
{
	public class UserGrupa
	{
		public User Kor { get; set; }
		public string KorId { get; set; }

		public Grupa Gru { get; set; }
		public int GruId { get; set; }

		public UserGrupa(User k, Grupa g)
		{
			Kor = k;
			KorId = k.Username;
			Gru = g;
			GruId = g.ID;
		}
		public UserGrupa() { }
	}
}
