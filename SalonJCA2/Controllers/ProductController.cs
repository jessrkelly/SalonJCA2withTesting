using Microsoft.AspNetCore.Mvc;
using SalonJCA2.Models;

namespace SalonJCA2.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.productlist = _db.products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Products model)
        {
            _db.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

		public IActionResult Delete(int id)
		{
			var data = _db.products.Where(x => x.id == id).FirstOrDefault();
			_db.Remove(data);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
