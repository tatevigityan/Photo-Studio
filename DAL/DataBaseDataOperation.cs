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
    }
}
