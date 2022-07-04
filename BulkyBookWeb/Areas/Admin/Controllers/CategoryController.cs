using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _UnitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _UnitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _UnitOfWork.Category.GetAll();
        return View(objCategoryList);
    }
    //CREATE METHOD
    //Get
    public IActionResult Create()
    {
        return View();
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The Display Order Cannot Be Same as Name");
        }
        if (ModelState.IsValid)
        {
            _UnitOfWork.Category.Add(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "Category created successfully...";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //EDIT METHOD
    //Get
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var CategoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFrist = _UnitOfWork.Category.GetFristOrDefault(c => c.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (categoryFromDbFrist == null)
        {
            return NotFound();
        }
        return View(categoryFromDbFrist);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The Display Order Cannot Be Same as Name");
        }
        if (ModelState.IsValid)
        {
            _UnitOfWork.Category.Update(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "Category updated successfully...";
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
        var categoryFromDbFrist = _UnitOfWork.Category.GetFristOrDefault(c => c.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (categoryFromDbFrist == null)
        {
            return NotFound();
        }
        return View(categoryFromDbFrist);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj= _UnitOfWork.Category.GetFristOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _UnitOfWork.Category.Remove(obj);
        _UnitOfWork.Save();
        TempData["Success"] = "Category deleted successfully...";
        return RedirectToAction("Index");
        
       // return View(obj);
    }
}