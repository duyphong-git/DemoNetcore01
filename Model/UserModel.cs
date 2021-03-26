using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class UserModel
    {
        public int? Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8)]
        public string Password { get; set; }
    }
}
