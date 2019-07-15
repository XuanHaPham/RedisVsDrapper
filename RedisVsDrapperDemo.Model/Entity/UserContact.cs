using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedisVsDrapperDemo.Model.Entity
{
    public partial class UserContact
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Address { get; set; }

        public virtual User User { get; set; }
    }
}
