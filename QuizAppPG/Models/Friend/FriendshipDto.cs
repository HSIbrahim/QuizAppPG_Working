﻿using System.ComponentModel.DataAnnotations;

namespace QuizAppPG.DTOs 
{
    public class FriendRequestDto
    {
        [Required(ErrorMessage = "Mottagarens användarnamn är obligatoriskt.")]
        public string ReceiverUsername { get; set; } = string.Empty;
    }

    public class FriendDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsAccepted { get; set; }
        public bool IsSender { get; set; } 
    }
}