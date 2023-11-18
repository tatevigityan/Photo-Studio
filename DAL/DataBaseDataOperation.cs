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
    }
}
