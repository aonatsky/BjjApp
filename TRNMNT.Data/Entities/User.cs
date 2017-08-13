using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace TRNMNT.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
