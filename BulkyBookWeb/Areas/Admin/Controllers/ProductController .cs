using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using BulkyBookWeb.Repository.IRepository;
using BulkyBook.Utility;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class ProductController : Controller
{
    private readonly IProductWebRepository _npRepo;
    private readonly ICategoryWebRepository _npCategoryRepo;
    private readonly ICoverTypeWebRepository _npCoverRepo;

    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IProductWebRepository npRepo, ICategoryWebRepository npCategoryRepo, ICoverTypeWebRepository npCoverRepo, IWebHostEnvironment hostEnvironment)
    {
        _npRepo = npRepo;
        _npCategoryRepo = npCategoryRepo;
        _npCoverRepo = npCoverRepo;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> productList = await _npRepo.GetAllAsync(SD.ProductAPIPath);
        
        return View(productList);
    }
   
    //UPSERT METHOD
    //Get
    public async Task<IActionResult> Upsert(int? id)
    {
        IEnumerable<Category> objCategoryList = await _npCategoryRepo.GetAllAsync(SD.CategoryAPIPath);
        IEnumerable<CoverType> objCoverList = await _npCoverRepo.GetAllAsync(SD.CoverTypeAPIPath);
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = objCategoryList.Select(i=>new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            CoverTypeList =objCoverList.Select(i => new SelectListItem
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
            productVM.Product = await _npRepo.GetAsync(SD.ProductAPIPath, id.GetValueOrDefault());
            return View(productVM);
            //Update Product

        }
        
       
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM obj, IFormFile? file)
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
                await _npRepo.CreateAsync(SD.ProductAPIPath, obj.Product);
            }
            else
            {
                await _npRepo.Updatesync(SD.ProductAPIPath+obj.Product.Id, obj.Product);

            }
            
            TempData["Success"] = "Product created successfully...";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //DELETE METHOD
    //Get
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        IEnumerable<Category> objCategoryList = await _npCategoryRepo.GetAllAsync(SD.CategoryAPIPath);
        IEnumerable<CoverType> objCoverList = await _npCoverRepo.GetAllAsync(SD.CoverTypeAPIPath);
        //var CategoryFromDb = _db.Categories.Find(id);
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = objCategoryList.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            CoverTypeList = objCoverList.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
        };
        productVM.Product = await _npRepo.GetAsync(SD.ProductAPIPath, id.GetValueOrDefault());
        
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (productVM.Product == null)
        {
            return NotFound();
        }
        return View(productVM);
    }
    ////POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int? id)
    {
        var obj = await _npRepo.GetAsync(SD.ProductAPIPath, id.GetValueOrDefault());
        if (obj == null)
        {
            return NotFound();
        }

        await _npRepo.DeleteAsync(SD.ProductAPIPath, id.GetValueOrDefault());
        TempData["Success"] = "CoverType deleted successfully...";
        return RedirectToAction("Index");

        return View(obj);
    }
    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Product> productList = await _npRepo.GetAllAsync(SD.ProductAPIPath);
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