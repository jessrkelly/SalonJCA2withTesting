using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonJCA2.Models;
using System.Diagnostics;


namespace SalonJCA2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        public IActionResult Index()
        {



            var data = (from srvic in _db.services
                        join typs in _db.types on srvic.Typeid equals typs.id
                        select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();



            ViewBag.services = data; // _db.services.ToList();

            ViewBag.products = _db.products.OrderBy(x => x.Name).ToList();
            Bookings dr = new Bookings();
            return View(dr);
        }

        [HttpGet]
        public IActionResult Search(int txt)
        {

            var productdata = _db.products.Where(x => x.id == txt).FirstOrDefault();

            if (productdata.Name == "All")
            {

                ViewBag.services = (from srvic in _db.services
                                    join typs in _db.types on srvic.Typeid equals typs.id
                                    select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();


            }
            else
            {

                ViewBag.services = (from srvic in _db.services
                                    join typs in _db.types on srvic.Typeid equals typs.id
                                    where srvic.Productid == txt

                                    select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();

            }

            ViewBag.products = _db.products.OrderBy(x => x.Name).ToList();
            Bookings dr = new Bookings();
            dr.serviceid = txt;
            return View("Index", dr);
        }


        public IActionResult Book(int id)
        {
            return View();
        }
        public IActionResult UploadFiles()
        {
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}