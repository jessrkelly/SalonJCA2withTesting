using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonJCA2.Models;

namespace SalonJCA2.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private ApplicationDbContext _db;
        public BookingsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult DateChange(int id,DateTime dt)
        {
            var Admins = _db.UserRoles.Count();
            var timeSlots = _db.times.FromSqlRaw("select * from times where not timeRang in ( SELECT  [TimeSlot]  FROM [Salon].[dbo].[timeMaps] where date='"+dt.ToString()+"' group by [TimeSlot] having count(id)>="+Admins.ToString()+")").ToList();

       

            var data = _db.services.Where(x => x.id == id).FirstOrDefault();

            ViewBag.servicename = data.Name;
            ViewBag.serviceprice = data.Price;
            ViewBag.typename = _db.types.Where(x => x.id == data.Typeid).FirstOrDefault().TypeName;
            ViewBag.times = timeSlots;
            ViewBag.bookinglist = _db.bookings.ToList();
            Bookings bk = new Bookings();
            bk.serviceid = id;
            bk.Date=dt;
        
            return View("Index",bk);
        }
        public IActionResult Index(int id)
        {
            var Admins = _db.UserRoles.Count();

            // Retrieving booked slots with adequate admin coverage
            var bookedSlots = _db.timeMaps
                                 .GroupBy(tm => new { tm.TimeSlot, tm.Date })
                                 .Where(g => g.Count() >= Admins && g.Key.Date == DateTime.Now.Date)
                                 .Select(g => g.Key.TimeSlot)
                                 .ToList();  // This forces the execution of the above query and allows using the result in further in-memory operations.

            // Retrieving available time slots that are not fully booked
            var timeSlots = _db.times
                               .Where(t => !bookedSlots.Contains(t.timeRang))
                               .ToList();

            var data = _db.services.FirstOrDefault(x => x.id == id);

            if (data != null)
            {
                ViewBag.servicename = data.Name;
                ViewBag.serviceprice = data.Price;
                ViewBag.typename = _db.types.FirstOrDefault(x => x.id == data.Typeid)?.TypeName;
            }
            else
            {
                ViewBag.servicename = "Not found";
                ViewBag.serviceprice = "Not found";
                ViewBag.typename = "Not found";
            }

            ViewBag.times = timeSlots;
            ViewBag.bookinglist = _db.bookings.ToList();

            var bk = new Bookings
            {
                serviceid = id,
                Date = DateTime.Now
            };

            return View(bk);
        }
//Now Display the Booking View to the user (With the date and time etc)
        public IActionResult BookingsView()
        {
            ViewBag.bookinglist = _db.bookings.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Bookings model)
        {
            var slot = new TimeMap
            {
                Date=model.Date,
                TimeSlot=model.Time
            };
            _db.Add(slot);  
            _db.Add(model);
            _db.SaveChanges();
            return RedirectToAction("BookingsView", new {id=model.serviceid});
        }

        public IActionResult Delete(int id)
        {
         
            var data = _db.bookings.Where(x => x.id == id).FirstOrDefault();
            var timeSlot = _db.timeMaps.Where(x => x.Date == data.Date && x.TimeSlot == data.Time).FirstOrDefault();
            _db.Remove(timeSlot);
            _db.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("BookingsView");
        }

        public IActionResult Edit(int id)
        {
          
           
            ViewBag.times = _db.times.ToList();
            var data = _db.bookings.Where(x => x.id == id).FirstOrDefault();
            var servicedata = _db.services.Where(x => x.id == data.serviceid).FirstOrDefault();
            ViewBag.servicename = servicedata.Name;
            ViewBag.serviceprice = servicedata.Price;

            ViewBag.typename = _db.types.Where(x => x.id == servicedata.Typeid).FirstOrDefault().TypeName;

            return View(data);
        }

        public IActionResult EditPOST(Bookings model)
        {
            
            _db.Update(model);
            _db.SaveChanges();

            return RedirectToAction("BookingsView");
        }


    }
}
