using System.Collections.Generic;
using System.Globalization;

namespace IplServerSide.Dtos
{
    public class UserDto
    {
        //public int UserId { get; set; }

        public string UserName { get; set; }

        public string SecretKey { get; set; }

        public string DisplayName { get; set; }

        public string UserGroup { get; set; }

        public bool IsAdmin { get; set; }

        public int UserRole { get; set; }
        public decimal UserAmount { get; set; }

        //public bool IsAllowedToBet { get; set; }

        public List<int> UsersFutureBets { get; set; }
    }
}