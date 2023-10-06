using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MasterDetailes.Models
{
    public class Client
    {

        public Client()
        {
            this.BookingEntries = new List<BookingEntry>();
        }

        public int ClientId { get; set; }
        [Required,StringLength(50),Display(Name ="Client Name")]
        public string ClientName { get; set; }
        [Required, Column(TypeName ="date"), Display(Name ="Birth Date"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public bool MaritalStatus { get; set; }

        public virtual ICollection<BookingEntry> BookingEntries { get; set; }
    }



    public class Spot
    {
        public Spot()
        {
            this.BookingEntries= new List<BookingEntry>();  
        }
        public int SpotId { get; set; }
        [Required, StringLength(50), Display(Name = "Spot Name")]
        public string SpotName { get; set; }

        //nev
        public virtual ICollection<BookingEntry> BookingEntries { get; set; }

    }

    public class BookingEntry
    {
        public  int  BookingEntryId { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Spot")]
        public int SpotId { get; set; }

        //making association for foreignkey
        //bcz clientid and spotid is a primary key in their table
        public virtual Client Client { get; set; }

        public virtual Spot Spot { get; set; }

    }


    public class TravelDbContext:DbContext
    {
        public  DbSet<BookingEntry> BookingEntries { get;  set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Spot> Spots { get; set; }   


    }



}