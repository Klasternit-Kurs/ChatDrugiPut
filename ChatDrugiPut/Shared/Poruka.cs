﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChatDrugiPut.Shared
{
	public class Poruka
	{
		public string Sadrzaj { get; set; } 
		public User Posiljaoc { get; set; } 
		
		public Poruka(string s, User u)
		{
			Sadrzaj = s;
			Posiljaoc = u; 
		}

		public Poruka() { }
	}
}
