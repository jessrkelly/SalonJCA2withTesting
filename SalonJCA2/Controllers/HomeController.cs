using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        //index to display services and products 
        public IActionResult Index()
        {


            //Retrieve data by joining 'services' with 'types' in the database and projecting into a new 'Services' object.
            var data = (from srvic in _db.services
                        join typs in _db.types on srvic.Typeid equals typs.id
                        select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();


            //Store and get data to view
            ViewBag.services = data; // _db.services.ToList();

            //Retrieve and sort all products by name.
            ViewBag.products = _db.products.OrderBy(x => x.Name).ToList();
            //Create a new booking object to be passed to the view.
            Bookings dr = new Bookings();
            return View(dr);
        }

        [HttpGet]
        public IActionResult Search(int txt)
        {
            //Search for a product by ID.
            var productdata = _db.products.Where(x => x.id == txt).FirstOrDefault();
            //If the product name is "All", retrieve all services.
            if (productdata.Name == "All")
            {

                ViewBag.services = (from srvic in _db.services
                                    join typs in _db.types on srvic.Typeid equals typs.id
                                    select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();


            }
            else
            {
                //Otherwise, get services related to the specified product ID.
                ViewBag.services = (from srvic in _db.services
                                    join typs in _db.types on srvic.Typeid equals typs.id
                                    where srvic.Productid == txt

                                    select new Services { id = srvic.id, Name = srvic.Name, Price = srvic.Price, path = srvic.path, Productid = srvic.Productid, Typeid = srvic.Typeid, TypeName = typs.TypeName }).ToList();

            }
            //Retrieve and sort all products by name, again.
            ViewBag.products = _db.products.OrderBy(x => x.Name).ToList();
            //Create a new booking object, set the service ID and view.
            Bookings dr = new Bookings();
            dr.serviceid = txt;
            return View("Index", dr);
        }

        //Booking method
        public IActionResult Book(int id)
        {
            return View();
        }
        //Upload a file method 
        public IActionResult UploadFiles()
        {
            return View();
        }
        //method for the Privacy page, restricted to users with the 'SuperAdmin' role.
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Privacy()
        {
            return View();
        }

        //Error handling method to provide custom error responses.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Create an API - I will use this in my Salon client console
        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _db.services.Select(s => new {
                s.id,
                s.Name,
                s.Price,
                s.path,
                s.Productid,
                s.Typeid
            }).ToList();
            return Json(services);
        }

    }
}