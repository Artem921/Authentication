using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Authorization.VKontakte.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string returnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalProvider { get; internal set; }
    }
}

