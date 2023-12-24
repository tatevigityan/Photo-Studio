using System;
using System.Collections.Generic;
using DAL.Models;
using System.Linq;
using System.Data;
using System.Data.Entity;

namespace DAL
{
    public class DataBaseDataOperation
    {
        private PhotoStudioModel dataBase;

        public DataBaseDataOperation()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<PhotoStudioModel>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PhotoStudioModel>());
            dataBase = new PhotoStudioModel();
        }

        public List<IncomeReport> getBookings(DateTime? startDate, DateTime? endDate)
        {
            return dataBase.bookings
                .Join(dataBase.halls,
                booking => booking.hallId,
                hall => hall.id,
                (booking, hall) => new
                {
                    studioHall = hall.name,
                    totalPrice = booking.totalPrice,
                    date = booking.dateTime
                })
                .Where(booking => booking.date >= startDate && booking.date <= endDate)
                .OrderBy(booking => booking.date)
                .Select(booking => new IncomeReport
                {
                    Hall = booking.studioHall,
                    Price = booking.totalPrice.ToString(),
                    Date = booking.date.Day + "." + booking.date.Month + "." + booking.date.Year + " " + booking.date.Hour + ":" + booking.date.Minute
                })
                .ToList();
        }

        public List<Hall> getHalls(int categoryId)
        {
            return dataBase.halls
                .Where(hall => hall.categoryId == categoryId)
                .OrderBy(studioHall => studioHall.hourlyPrice)
                .ToList();
        }

        public void createNewBooking(Booking booking)
        {
            dataBase.bookings.Add(booking);
            dataBase.SaveChanges();
        }

        public void addNewClient(Client client)
        {
            dataBase.clients.Add(client);
            dataBase.SaveChanges();
        }

        public List<Service> getServices()
        {
            return dataBase.services
                .OrderBy(service => service.hourlyPrice)
                .ToList();
        }

        public List<Client> getClients()
        {
            return dataBase.clients.ToList();
        }

        public List<Booking> getAllBookings()
        {
            return dataBase.bookings
                .Include("clients")
                .ToList();
        }

        public UserData getCurrentUser(string login, string password)
        {
            List<UserData> users;

            users = dataBase.users
                .ToList()
                .Where(user => user.username == login && user.password == password)
                .Join(dataBase.roles,
                    user => user.roleId,
                    role => role.id,
                    (user, role) => new UserData
                    {
                        username = user.username,
                        role = role.name
                    })
                .ToList();

            return (users.Count > 0) ? users[0] : null;
        }

        public List<User> getAllUsers()
        {
            return dataBase.users.ToList();
        }

        public List<CategoryHall> getHallCategories()
        {
            return dataBase.hallCategories.ToList();
        }

        public int getHallsCount(int selectedCategory)
        {
            return dataBase.bookings
                .Where(booking => booking.dateTime.Year == DateTime.Now.Year
                    && booking.dateTime.Month == DateTime.Now.Month)
                .Join(dataBase.halls,
                    booking => booking.hallId,
                    hall => hall.id,
                (booking, hall) => new
                {
                    category = hall.categoryId
                })
                .Where(hall => hall.category == selectedCategory)
                .ToList().Count;
        }

        public int getCurrentMonthIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.dateTime.Year == DateTime.Now.Year
                    && booking.dateTime.Month == DateTime.Now.Month)
                .Sum(booking => booking.totalPrice);
        }

        public int getCurrentQuarterIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.dateTime.Year == DateTime.Now.Year)
                .Sum(booking => booking.totalPrice);
        }

        public int getCurrentYearIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.dateTime.Year == DateTime.Now.Year)
                .Sum(booking => booking.totalPrice);
        }
    }
}
