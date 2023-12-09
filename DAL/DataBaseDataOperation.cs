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

        //public Guest FindGuest(Guest guest)
        //{
        //    if (dataBase.Guests != null)
        //    {
        //        var foundGuest = dataBase.Guests.FirstOrDefault(g => g.Name == guest.Name && g.Surname == guest.Surname &&
        //        g.Passport == guest.Passport && g.BirthDate == guest.BirthDate);

        //        return foundGuest != null ? foundGuest : null;
        //    }

        //    return null;
        //}

        //public void ReservateRoom(int id)
        //{
        //    dataBase.Rooms.Find(id).Status = "Occupied";
        //    dataBase.SaveChanges();
        //}

        //public void RoomAvailable(int Id)
        //{
        //    dataBase.Rooms.Find(Id).Status = "Available";
        //    dataBase.SaveChanges();
        //}

        public List<IncomeReport> getBookings(DateTime? startDate, DateTime? endDate)
        {
            return dataBase.bookings
                .Join(dataBase.studioHalls,
                booking => booking.studioHallId,
                hall => hall.id,
                (booking, hall) => new
                {
                    studioHall = hall.name,
                    totalPrice = booking.totalPrice,
                    date = booking.date
                })
                .Where(booking => booking.date >= startDate && booking.date <= endDate)
                .OrderBy(booking => booking.date)
                .Select(booking => new IncomeReport
                {
                    studioHall = booking.studioHall,
                    totalIncome = booking.totalPrice.ToString(),
                    date = booking.date.Day + "." + booking.date.Month + "." + booking.date.Year + " " + booking.date.Hour + ":" + booking.date.Minute
                })
                .ToList();
        }

        public void RoomControl()
        {
            //foreach (Booking reservation in dataBase.Reservations.Where(r => r.DepartureDate < DateTime.Now && r.isActive == true).ToList())
            //    foreach (StudioHall room in dataBase.Rooms.Where(r => r.Status == "Occupied" && r.Id == reservation.RoomId).ToList())
            //    {
            //        room.Status = "Cleaning";
            //        reservation.isActive = false;
            //        dataBase.SaveChanges();
            //    }
        }



        // Ready

        public List<StudioHall> getAvailableStudioHalls()
        {
            // todo свободно ли
            return dataBase.studioHalls
                .Where(studioHall => studioHall.id > 0)
                .OrderBy(studioHall => studioHall.hourlyPrice)
                .ToList();
        }

        public void updateBooking(Booking selectedBooking)
        {
            dataBase.bookings.FirstOrDefault(booking => booking.id == selectedBooking.id).totalPrice = selectedBooking.totalPrice;
        }

        public List<StudioServiceMembership> getStudioServiceMemberships(int bookingId, int clientId)
        {
            return dataBase.studioServiceMemberships
                .Where(membership => membership.bookingId == bookingId && membership.clientId == clientId).ToList();
        }

        public void removeStudioServiceMemberships(int bookingId, int clientId)
        {
            var memberships = dataBase.studioServiceMemberships
                .Where(membership => membership.bookingId == bookingId && membership.clientId == clientId).ToList();

            foreach (StudioServiceMembership membership in memberships)
                dataBase.studioServiceMemberships.Remove(membership);

            dataBase.SaveChanges();
        }

        public void createStudioServiceMembership(StudioServiceMembership membership)
        {
            dataBase.studioServiceMemberships.Add(membership);
            dataBase.SaveChanges();
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

        public List<StudioService> getStudioServices()
        {
            return dataBase.studioServices
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

            users = dataBase.users.ToList().Where(user => user.login == login && user.password == password).Select(user => new UserData
            {
                id = user.id,
                role = user.role
            }).ToList();

            return (users.Count > 0) ? users[0] : null;
        }

        public List<User> getAllUsers()
        {
            return dataBase.users.ToList();
        }

        public int getStudioHallsCount(string selectedCategory)
        {
            return dataBase.bookings
                .Where(booking => booking.bookingDate.Year == DateTime.Now.Year
                    && booking.bookingDate.Month == DateTime.Now.Month)
                .Join(dataBase.studioHalls,
                    booking => booking.studioHallId,
                    hall => hall.id,
                (booking, hall) => new
                {
                    category = hall.category
                })
                .Where(hall => hall.category == selectedCategory)
                .ToList().Count;
        }

        public int getCurrentMonthIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.bookingDate.Year == DateTime.Now.Year
                    && booking.bookingDate.Month == DateTime.Now.Month)
                .Sum(booking => booking.totalPrice);
        }

        public int getCurrentQuarterIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.bookingDate.Year == DateTime.Now.Year)
                .Sum(booking => booking.totalPrice);
        }

        public int getCurrentYearIncome()
        {
            return dataBase.bookings
                .Where(booking => booking.bookingDate.Year == DateTime.Now.Year)
                .Sum(booking => booking.totalPrice);
        }
    }
}
