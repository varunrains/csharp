using IplServerSide.Enums;
using System.Collections.Generic;
namespace IplServerSide.Models
{
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Bets = new List<Bet>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string UserGroup { get; set; }

        public string PassKey { get; set; }

        public decimal UserAmount { get; set; }

        //public bool IsAllowedToBet { get; set; }

        public UserRoleEnum UserRole { get; set; }

       // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bet> Bets { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; set; }
    }
}
