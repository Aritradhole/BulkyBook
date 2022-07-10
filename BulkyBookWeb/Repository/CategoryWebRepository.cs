using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class CategoryWebRepository : Repository<Category>, ICategoryWebRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public CategoryWebRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
