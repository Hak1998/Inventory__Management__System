﻿using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.DTOs.AuthDto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
