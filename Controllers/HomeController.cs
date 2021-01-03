using ImageUpload.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageDbContext _imageDbContext;
        public HomeController(IWebHostEnvironment hostEnvironment, ImageDbContext imageDbContext)
        {
            _imageDbContext = imageDbContext;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var imageModel =  _imageDbContext.Images.ToList();
            return View(imageModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([Bind("ImageId,Title,ImageFile")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _imageDbContext.Add(imageModel);
                await _imageDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }
        public  ActionResult Delete()
        {
            var imageModel =  _imageDbContext.Images.ToList();
            return View(imageModel);
        }
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var imageModel = await _imageDbContext.Images.FindAsync(id);
            if (imageModel != null)
            {
                //delete image from wwwroot/image
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", imageModel.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete the record
                _imageDbContext.Images.Remove(imageModel);
                await _imageDbContext.SaveChangesAsync();
               return View(imageModel);
            }
            return RedirectToAction(nameof(Delete));
        } 

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
       

   

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
    }
}
