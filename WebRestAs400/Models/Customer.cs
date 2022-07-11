using MsSqlOrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string NickName { get; set; }
        [Virtual]
        public int PageNumber { get; set; }
        [Virtual]
        public int RowsNumber { get; set; }
    }
}