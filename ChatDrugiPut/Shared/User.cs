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
		public bool LoggedIn { get; set; }
	}
}
