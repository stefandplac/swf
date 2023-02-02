using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace swf.Models.DTOsUsers
{
    
    public class UserLoginRequestDTO 
    {
        [Required]
       public string Email { get; set; }
        [Required]
        public string Password { get; set; }    
    }
}
