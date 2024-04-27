using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonJCA2.Models;

namespace SalonJCA2.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {

        private ApplicationDbContext _db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;
        public ServicesController(ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _db = db;
            _hostEnvironment = environment;

        }
        //View the Services
        public IActionResult Index()
        {
            ViewBag.products = _db.products.ToList();
            ViewBag.types = _db.types.ToList();
            ViewBag.servicelist = _db.services.ToList();
            return View();
        }
        //Delete the Services
        public IActionResult Delete(int id)
        {
            var data = _db.services.Where(x => x.id == id).FirstOrDefault();
            _db.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Search the Service
        public IActionResult Search(int id)
        {
            ViewBag.products = _db.products.ToList();
            ViewBag.types = _db.types.Where(x => x.productid == id).ToList();
            ViewBag.servicelist = _db.services.ToList();
            Services srv = new Services();
            srv.Productid = id;
            return View("Index", srv);
        }

        //Add a Service Model
        [HttpPost]
        public IActionResult Add(Services model)
        {
            string fil = "";
            if (Request.Form.Files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var uploads = Path.Combine(wwwRootPath, @"Images");
                IFormFile file = Request.Form.Files[0];
                var extension = Path.GetExtension(file.FileName);


                fil = "\\images\\" + fileName + extension;
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
            }

            model.path = fil;

            //Add the Service and save the DB changes 
            _db.Add(model);
            _db.SaveChanges();

            //Redirect to Index
            return RedirectToAction("Index");
        }
    }
}
