using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            IEnumerable<Category> objList = _unitOfWork.Category.GetAll();
            
            return Ok(objList);
        }
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(int categoryId)
        {
            var obj = _unitOfWork.Category.GetFristOrDefault(x => x.Id == categoryId);
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
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }
        [HttpPatch("{categoryId}", Name = "UpdateCategory")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category obj)
        {
            if (obj == null || categoryId != obj.Id)
            {
                return BadRequest(ModelState);
            }
            
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            return NoContent();
        }
        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var categoryFromDb = _unitOfWork.Category.GetFristOrDefault(u => u.Id == categoryId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}
