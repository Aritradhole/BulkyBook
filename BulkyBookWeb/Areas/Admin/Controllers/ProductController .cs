using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class ProductController : Controller
{
    private readonly IUnitOfWork _UnitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _UnitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        var productList = _UnitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return View(productList);
    }
   
    //UPSERT METHOD
    //Get
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _UnitOfWork.Category.GetAll().Select(i=>new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
        };
        if (id == null || id == 0)
        {
            //Create product
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CategoryList;

            return View(productVM);
        }
        else
        {
            productVM.Product=_UnitOfWork.Product.GetFristOrDefault(u => u.Id == id);
            return View(productVM);
            //Update Product

        }
        
       
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
       
        if (ModelState.IsValid)
        {
            String wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads= Path.Combine(wwwRootPath, @"images\products");
                var extension= Path.GetExtension(file.FileName);

                if(obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath,obj.Product.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            if (obj.Product.Id == 0)
            {
                _UnitOfWork.Product.Add(obj.Product);
            }
            else
            {
                _UnitOfWork.Product.Update(obj.Product);
            }
            _UnitOfWork.Save();
            TempData["Success"] = "Product created successfully...";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //DELETE METHOD
    //Get
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var CategoryFromDb = _db.Categories.Find(id);
        var ProductFromDbFrist = _UnitOfWork.Product.GetFristOrDefault(c => c.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (ProductFromDbFrist == null)
        {
            return NotFound();
        }
        return View(ProductFromDbFrist);
    }
    ////POST
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _UnitOfWork.Product.GetFristOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _UnitOfWork.Product.Remove(obj);
        _UnitOfWork.Save();
        TempData["Success"] = "CoverType deleted successfully...";
        return RedirectToAction("Index");

        return View(obj);
    }
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _UnitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data = productList });
    }
    //POST
    //[HttpDelete]
    
    //public IActionResult Delete(int? id)
    //{
    //    var obj = _UnitOfWork.Product.GetFristOrDefault(c => c.Id == id);
    //    if (obj == null)
    //    {
    //        return Json(new {success=false,massage="Errorwhile deleting"});
    //    }
    //    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
    //    if (System.IO.File.Exists(oldImagePath))
    //    {
    //        System.IO.File.Delete(oldImagePath);
    //    }

    //    _UnitOfWork.Product.Remove(obj);
    //    _UnitOfWork.Save();
    //    TempData["Success"] = "Deleted successfully...";
    //    return Json(new {success=true,message="Deleted successfully...."});

    //    // return View(obj);
    //}
    #endregion

}