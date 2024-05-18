
using System.Collections.Generic;

namespace SiliconApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

       public List<Contact> contacts { get; set; }

    }
}
