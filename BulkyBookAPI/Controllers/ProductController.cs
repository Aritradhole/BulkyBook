using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        

        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public IActionResult GetProducts()
        {
            IEnumerable<Product> objList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            
            return Ok(objList);
        }

        [HttpGet("name/{productName}")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsByName(string productName)
        {
            IEnumerable<Product> objList = _unitOfWork.Product.GetAll(x => x.Title.ToLower().Contains(productName.ToLower()), includeProperties: "Category,CoverType");
            
            return Ok(objList);
        }
        [HttpGet("{productId}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]

        public IActionResult GetProduct(int productId)
        {
            var obj = _unitOfWork.Product.GetFristOrDefault(x => x.Id == productId, includeProperties: "Category,CoverType");
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                
                return Ok(obj);
            }
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        /*[Authorize(Roles = SD.Role_Admin)]*/
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();
            return CreatedAtRoute("GetProduct", new { productId = product.Id }, product);
        }
        [HttpPatch("{productId}", Name = "UpdateProduct")]
        /* [Authorize(Roles = SD.Role_Admin)]*/
        public IActionResult UpdateProduct(int productId, [FromBody] Product product)
        {
            if (product == null || productId != product.Id)
            {
                return BadRequest(ModelState);
            }
            
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            return NoContent();
        }
        [HttpDelete("{productId}", Name = "DeleteProduct")]
        /*[Authorize(Roles = SD.Role_Admin)]*/
        public IActionResult DeleteProduct(int productId)
        {
            var obj = _unitOfWork.Product.GetFristOrDefault(u => u.Id == productId);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}
