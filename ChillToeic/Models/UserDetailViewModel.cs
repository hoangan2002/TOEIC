using ChillToeic.Models.Entity;
using System.Collections.Generic;

namespace ChillToeic.ViewModels
{
    public class UserDetailViewModel
    {
        public List<User> Users { get; set; }
        public List<Order> Orders { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}
