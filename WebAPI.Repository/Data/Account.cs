using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Repository.Data
{
    public class Account : Entity
    {
        public Account() { }
        public Account(int id) { }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        private readonly List<Subscription> subscriptions = new();

        public Collection<Subscription> Subscriptions => new Collection<Subscription>(subscriptions);

    }
}
