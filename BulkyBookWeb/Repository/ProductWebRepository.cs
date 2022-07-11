using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class ProductWebRepository : Repository<Product>, IProductWebRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductWebRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
