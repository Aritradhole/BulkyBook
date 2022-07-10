
using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
//[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{
    private readonly ICategoryWebRepository _npRepo;
    public CategoryController(ICategoryWebRepository npRepo)
    {
        _npRepo = npRepo;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> objCategoryList = await _npRepo.GetAllAsync(SD.CategoryAPIPath);
        return View(objCategoryList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.Id == 0)
            {
                await _npRepo.CreateAsync(SD.CategoryAPIPath, obj);
            }
            TempData["success"] = "Category created successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(obj);
        }
    }
    public async Task<IActionResult> Edit(int? Id)
    {
        Category obj = new Category();
        if (Id == null)
        {
            //this will be true for Insert/Create
            return View(obj);
        }
        //Flow will come here for update
        obj = await _npRepo.GetAsync(SD.CategoryAPIPath, Id.GetValueOrDefault());
        if (obj == null)
        {
            return NotFound();
        }
        return View(obj);


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            
            await _npRepo.Updatesync(SD.CategoryAPIPath + obj.Id, obj);
            TempData["success"] = "Category Updated Successfully.";
            
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(obj);
        }
    }

    //GET
    public async Task<IActionResult> Delete(int? Id)
    {
        if (Id == null || Id == 0)
        {
            return NotFound();
        }
        Category obj = await _npRepo.GetAsync(SD.CategoryAPIPath, Id.GetValueOrDefault());

        if (obj == null)
        {
            return NotFound();
        }
        return View(obj);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*[Authorize(Roles = "Admin")]*/
    public async Task<IActionResult> DeletePOST(int Id)
    {
        var status = await _npRepo.DeleteAsync(SD.CategoryAPIPath, Id);
        TempData["Success"] = "Category Deleted Successfully.";
        return RedirectToAction(nameof(Index));
    }
}
