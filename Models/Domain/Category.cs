using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Category
    {
        [JsonProperty]
        public int CategoryId { get; private set; }

        private string _name;

        public string Icon { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nên điền danh mục một cái tên");
                if (value.Length > 20)
                    throw new ArgumentException("Tên của danh mục có tối đa 20 ký tự");
                _name = value;
            }
        }
        public ICollection<Items> Coupons { get; private set;  }

        protected Category() {
            Coupons = new HashSet<Items>();
        }

        public Category(string name, String icon) : this()
        {
            Name = name;
            Icon = icon;
        }
    }
}
