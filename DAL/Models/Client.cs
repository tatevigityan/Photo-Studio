﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Client")]
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }
}