using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonJCA2.Models;

namespace SalonJCA2.Controllers
{
    [Authorize]
    //Allows only Authorized users to access the bookings - mist login
    public class BookingsController : Controller
    {
        //Interaction with Database
        private ApplicationDbContext _db;
        public BookingsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Method to get the count of Admins
        private async Task<int> GetAdminCountAsync()
        {
            return await _db.UserRoles.CountAsync(ur => _db.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Admin"));
        }


        //Changes the date of a booking and updates available time slots.
        public async Task<IActionResult> DateChange(int id, DateTime dt)
        {
            var AdminCount = await GetAdminCountAsync();
            //Use of SQL to find the table in dtabaste on number of Admins and time slots avaialble based on number admins
            var timeSlots = await _db.times.FromSqlRaw(
                "SELECT * FROM times WHERE Id NOT IN (SELECT TimeId FROM timeMaps WHERE Date = {0} GROUP BY TimeId HAVING COUNT(*) >= {1})", dt.Date, AdminCount
            ).ToListAsync();

            var data = await _db.services.FirstOrDefaultAsync(x => x.id == id);
            //Populate view model
            if (data != null)
            {
                ViewBag.servicename = data.Name;
                ViewBag.serviceprice = data.Price;
                ViewBag.typename = (await _db.types.FirstOrDefaultAsync(x => x.id == data.Typeid))?.TypeName;
            }

            ViewBag.times = timeSlots;
            ViewBag.bookinglist = await _db.bookings.ToListAsync();
            return View("Index", new Bookings { serviceid = id, Date = dt });
        }

        //Displays the initial booking page with available time slots - again using SQL.
        public async Task<IActionResult> Index(int id)
        {
            var AdminCount = await GetAdminCountAsync();

            var bookedSlots = await _db.timeMaps
                                       .Where(tm => tm.Date == DateTime.Now.Date)
                                       .GroupBy(tm => tm.TimeSlot)
                                       .Where(g => g.Count() >= AdminCount)
                                       .Select(g => g.Key)
                                       .ToListAsync();

            var timeSlots = await _db.times.Where(t => !bookedSlots.Contains(t.timeRang)).ToListAsync();

            var data = await _db.services.FirstOrDefaultAsync(x => x.id == id);

            if (data != null)
            {
                ViewBag.servicename = data.Name;
                ViewBag.serviceprice = data.Price;
                ViewBag.typename = (await _db.types.FirstOrDefaultAsync(x => x.id == data.Typeid))?.TypeName;
            }
            else
            {
                ViewBag.servicename = "Not found";
                ViewBag.serviceprice = "Not found";
                ViewBag.typename = "Not found";
            }

            ViewBag.times = timeSlots;
            ViewBag.bookinglist = await _db.bookings.ToListAsync();

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

        //Add a booking 
        [HttpPost]
        public async Task<IActionResult> Add(Bookings model)
        {
            if (ModelState.IsValid)
            {
                var slot = new TimeMap
                {
                    Date = model.Date,
                    TimeSlot = model.Time
                };
                _db.Add(slot);
                _db.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("BookingsView", new { id = model.serviceid });
            }
            //Handle the case when model is not valid
            return View(model);
        }


        //Delete a booking, error display and update database with chnages
        public IActionResult Delete(int id)
        {
            var data = _db.bookings.FirstOrDefault(x => x.id == id);
            if (data == null)
            {
                //add a user-friendly error message or log the error
                return View("Error");
            }

            var timeSlot = _db.timeMaps.FirstOrDefault(x => x.Date == data.Date && x.TimeSlot == data.Time);
            if (timeSlot != null)
            {
                _db.timeMaps.Remove(timeSlot);
            }

            _db.bookings.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("BookingsView");
        }

        //Edit booking, update database with changes 
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
