using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverTypeController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CoverType>))]
        [ProducesResponseType(400)]
        public IActionResult GetCoverTypes()
        {
            IEnumerable<CoverType> objList = _unitOfWork.CoverType.GetAll();
            
            return Ok(objList);
        }

        [HttpGet("{covertypeId}", Name = "GetCoverType")]
        [ProducesResponseType(200, Type = typeof(CoverType))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCoverType(int covertypeId)
        {
            var obj = _unitOfWork.CoverType.GetFristOrDefault(x => x.Id == covertypeId);
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
        [ProducesResponseType(201, Type = typeof(CoverType))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateToyType([FromBody] CoverType obj)
        {
            if (obj == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            return CreatedAtRoute("GetCoverType", new { covertypeId = obj.Id }, obj);
        }
        [HttpPatch("{toytypeId}", Name = "UpdateToyType")]
        public IActionResult UpdateToyType(int toytypeId, [FromBody] CoverType coverType)
        {
            if (coverType == null || toytypeId != coverType.Id)
            {
                return BadRequest(ModelState);
            }
           
            _unitOfWork.CoverType.Update(coverType);
            _unitOfWork.Save();
            return NoContent();
        }
        [HttpDelete("{covertypeId}", Name = "DeleteCoverType")]
        public IActionResult DeleteToyType(int covertypeId)
        {
            var obj = _unitOfWork.CoverType.GetFristOrDefault(u => u.Id == covertypeId);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}
