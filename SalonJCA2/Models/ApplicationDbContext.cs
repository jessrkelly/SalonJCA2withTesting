using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SalonJCA2.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //Represents the Services table in the database that the USER will see
        public DbSet<Services> services { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Types> types { get; set; }
        public DbSet<Bookings> bookings { get; set; }
        public DbSet<Times> times { get; set; }
        public DbSet<TimeMap> timeMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
            //Seeds a role named "Admin"
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = "2",
                ConcurrencyStamp = "2"
            });
            //Seeds a role named "User"
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "User",
                NormalizedName = "User",
                Id = "3",
                ConcurrencyStamp = "3"
            });

            //Create snd Seeds an admin user
            var appUser = new AppUsers
			{
				Id = "2",
				Email = "admin@abc.com",
				EmailConfirmed = true,
				FirstName = "Admin",
				LastName = "Ofoedu",
				UserName = "admin@abc.com",
 
			NormalizedUserName = "admin@abc.com"
			};

			//Set the user password for the admin user
			PasswordHasher<AppUsers> ph = new PasswordHasher<AppUsers>();
			appUser.PasswordHash = ph.HashPassword(appUser, "Abc.123456");

			//Seed the admin user
			builder.Entity<AppUsers>().HasData(appUser);


            var appUser4 = new AppUsers
            {
                Id = "4",
                Email = "admin2@abc.com",
                EmailConfirmed = true,
                FirstName = "Admin2",
                LastName = "Ofoedu",
                UserName = "admin2@abc.com",

                NormalizedUserName = "admin2@abc.com"
            };

            
            PasswordHasher<AppUsers> ph4 = new PasswordHasher<AppUsers>();
            appUser4.PasswordHash = ph4.HashPassword(appUser4, "Abc.123456");

            
            builder.Entity<AppUsers>().HasData(appUser4);


            //Allocate the Admin role access to the user - can add services, edit etc
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = "2",
				UserId = "2"
			});

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "2",
                UserId = "4"
            });

            //Apply time slots to the Time table - made just hour timeslots. In hindsight I should have made the Services with an allocated time (ie. 1hr for hair cut, 2hr for Colouring etc)
            builder.Entity<Times>().HasData(
                new Times { Id = 1, timeRang = "08.00 - 09.00" },
                new Times { Id = 2, timeRang = "09.00 - 10.00" },
                new Times { Id = 3, timeRang = "11.00 - 12.00" },
                new Times { Id = 4, timeRang = "13.00 - 14.00" },
                new Times { Id = 5, timeRang = "14.00 - 15.00" },
                new Times { Id = 6, timeRang = "15.00 - 16.00" },
                new Times { Id = 7, timeRang = "16.00 - 17.00" }
            );
        }
    }
}