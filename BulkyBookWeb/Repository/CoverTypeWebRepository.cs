using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class CoverTypeWebRepository : Repository<CoverType>, ICoverTypeWebRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public CoverTypeWebRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
