using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(1000)]
        public string Password { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
