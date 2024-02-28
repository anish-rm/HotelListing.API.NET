namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        //we want country to know it is related to hotel
        //this is one-to-many rel - one country may have many hotels..but one hotel will belong to only one country
        public virtual IList<Hotel>? Hotels { get; set; }
    }
}