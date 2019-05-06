using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();

        Category GetByName(string naam);

        Dictionary<Category, int> GetTop9WithAmount();
    }
}
