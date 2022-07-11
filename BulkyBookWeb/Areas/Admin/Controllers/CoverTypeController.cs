using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CoverTypeController : Controller
{
    private readonly ICoverTypeWebRepository _npRepo;

    public CoverTypeController(ICoverTypeWebRepository npRepo)
    {
        _npRepo = npRepo;
    }

    public async Task<IActionResult> Index()
    {
        //code to consume API to fech data 
        IEnumerable<CoverType> objCoverTypeList = await _npRepo.GetAllAsync(SD.CoverTypeAPIPath);
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
    public async Task<IActionResult> Create(CoverType obj)
    {
        
        if (ModelState.IsValid)
        {
            await _npRepo.CreateAsync(SD.CoverTypeAPIPath, obj);
            TempData["Success"] = "CoverType created successfully...";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //EDIT METHOD
    //Get
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var CategoryFromDb = _db.Categories.Find(id);
        var CoverTypeFromDbFrist = await _npRepo.GetAsync(SD.CoverTypeAPIPath, id.GetValueOrDefault());
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
    public async Task<IActionResult> Edit(CoverType obj)
    {
       
        if (ModelState.IsValid)
        {
            await _npRepo.Updatesync(SD.CoverTypeAPIPath + obj.Id, obj);
            TempData["Success"] = "CoverType updated successfully...";
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
        //var CategoryFromDb = _db.Categories.Find(id);
        var CoverTypeFromDbFrist = await _npRepo.GetAsync(SD.CoverTypeAPIPath,id.GetValueOrDefault());
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
    public async Task<IActionResult> DeletePost(int? id)
    {
       
        var obj = await _npRepo.DeleteAsync(SD.CoverTypeAPIPath, id.GetValueOrDefault());

        TempData["Success"] = "CoverType deleted successfully...";
        return RedirectToAction("Index");
        
       // return View(obj);
    }
}