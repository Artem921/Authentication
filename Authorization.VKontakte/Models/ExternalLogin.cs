using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Authorization.VKontakte.Models
{
	public class ExternalLogin
	{

		public string Email { get; set; }

		public string ReturnUrl { get; set; }
	
	}
}
