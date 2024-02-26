using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data
{
    public class Hotel
    {
        //When entity framework sees this model, it will know that it is auto incrementing primary key Id

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        [ForeignKey(nameof(CountryId))] // or ForeignKey("CountryId") // nameOf will just change to magic string ""
        public int CountryId { get; set; } // this is a foreign key for a table called Country
        
        // we have to refer to the country entity
        public Country Country { get; set; }
    }
}
