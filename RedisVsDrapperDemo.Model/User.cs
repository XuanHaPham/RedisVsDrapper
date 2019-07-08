using System;
using System.Collections.Generic;
using System.Text;

namespace RedisVsDrapperDemo.Model
{
    public enum PermissionEnum
    {
        NONE = 0,
        READ = 1,
        UPDATE = 2,
        INSERT = 3
    }

    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public PermissionEnum Permission { get; set; }
    }
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PermissionEnum Permission { get; set; }
    }
}
