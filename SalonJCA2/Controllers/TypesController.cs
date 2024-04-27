using Microsoft.AspNetCore.Mvc;
using SalonJCA2.Models;

namespace SalonJCA2.Controllers
{
    //Won't comment as it follows the same Structure as previous controllers

    public class TypesController : Controller
    {
        private ApplicationDbContext _db;
        public TypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {


            var data = (from typ in _db.types
                        join prodct in _db.products on typ.productid equals prodct.id
                        select new Types { id = typ.id, TypeName = typ.TypeName, Productname = prodct.Name, productid = prodct.id }).ToList();

            ViewBag.typelist = data; //_db.types.ToList();
            ViewBag.products = _db.products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Types model)
        {
            _db.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Types model)
        {
            _db.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var data = _db.types.Where(x => x.id == id).FirstOrDefault();
            _db.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
