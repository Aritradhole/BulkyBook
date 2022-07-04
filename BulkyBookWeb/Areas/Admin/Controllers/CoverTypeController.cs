using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _UnitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _UnitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _UnitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
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
    public IActionResult Create(CoverType obj)
    {
        
        if (ModelState.IsValid)
        {
            _UnitOfWork.CoverType.Add(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "CoverType created successfully...";
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
        var CoverTypeFromDbFrist = _UnitOfWork.CoverType.GetFristOrDefault(c => c.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (CoverTypeFromDbFrist == null)
        {
            return NotFound();
        }
        return View(CoverTypeFromDbFrist);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {
       
        if (ModelState.IsValid)
        {
            _UnitOfWork.CoverType.Update(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "CoverType updated successfully...";
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
        var CoverTypeFromDbFrist = _UnitOfWork.CoverType.GetFristOrDefault(c => c.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        if (CoverTypeFromDbFrist == null)
        {
            return NotFound();
        }
        return View(CoverTypeFromDbFrist);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj= _UnitOfWork.CoverType.GetFristOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _UnitOfWork.CoverType.Remove(obj);
        _UnitOfWork.Save();
        TempData["Success"] = "CoverType deleted successfully...";
        return RedirectToAction("Index");
        
       // return View(obj);
    }
}