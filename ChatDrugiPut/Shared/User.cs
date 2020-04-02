using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace ChatDrugiPut.Shared
{
	public class User
	{
		[Required]
		[StringLength(50, MinimumLength = 5, ErrorMessage ="Check length!")]
		public string Username { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5, ErrorMessage = "Check length (password:D)!")]
		public string Password { get; set; }

		public ICollection<UserGrupa> AktivneGrupe { get; set; }


		public override bool Equals(object obj)
		{
			if (obj is User u && u.Username == this.Username && u.Password == this.Password)
				return true;
			return false;
		}

		public override int GetHashCode()
		{
			return this.Username.GetHashCode();
		}
	}
}
