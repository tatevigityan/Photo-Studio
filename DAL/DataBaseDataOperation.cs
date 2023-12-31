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

        public List<TimeSlot> getAvailableTimeSlots(DateTime date, int hallId)
        {
            var bookingsOnDate = dataBase.bookings
                .Where(b => DbFunctions.TruncateTime(b.dateTime) == date.Date && b.hallId == hallId)
                .ToList();

            var allTimeSlots = generateAllTimeSlots();

            foreach (var booking in bookingsOnDate)
            {
                var bookedTimeSlots = generateBookedTimeSlots(booking.dateTime, booking.durationHours);
                allTimeSlots.RemoveAll(bookedTimeSlots.Contains);
            }

            List<TimeSlot> result = allTimeSlots
                   .Select(slot =>
                   {
                       var parts = slot.Split('-');
                       if (parts.Length == 2)
                       {
                           string startTimeString = parts[0].Trim();
                           if (TimeSpan.TryParse(startTimeString, out TimeSpan startTime))
                           {
                               return new TimeSlot
                               {
                                   text = slot,
                                   startTime = startTime.Hours,
                                   isSelected = false
                               };
                           }
                       }
                       return null;
                   })
                   .Where(slotViewModel => slotViewModel != null)
                   .ToList();

            return result ?? new List<TimeSlot>();
        }

        private List<string> generateAllTimeSlots()
        {
            var allTimeSlots = new List<string>();
            for (int hour = 9; hour < 17; hour++)
            {
                allTimeSlots.Add($"{hour}:00-{hour + 1}:00");
            }
            return allTimeSlots;
        }

        private List<string> generateBookedTimeSlots(DateTime startTime, int durationHours)
        {
            var bookedTimeSlots = new List<string>();
            for (int hour = startTime.Hour; hour < startTime.Hour + durationHours; hour++)
            {
                bookedTimeSlots.Add($"{hour}:00-{hour + 1}:00");
            }
            return bookedTimeSlots;
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

        public int getOrCreateClient(Client client)
        {
            var existingClient = dataBase.clients.FirstOrDefault(c => c.name == client.name || c.phone == client.phone);

            if (existingClient != null)
            {
                return existingClient.id;
            }
            else
            {
                dataBase.clients.Add(client);
                dataBase.SaveChanges();
                return client.id;
            }
        }

        public List<Service> getServices()
        {
            return dataBase.services
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
        
        public int getCurrentUserId(string username)
        {
            return dataBase.users.First(u => u.username == username).id;
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
