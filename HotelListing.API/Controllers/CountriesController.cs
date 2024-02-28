using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        //we are injecting dbcontext into our controller
        //since we have registerd Db context as part of services in Program.cs ,it gives us ability to inject it in any file wherever needed
        // in below way we no need to create new object for our dbcontext... we can just inject it.
        //we no need to instantiate new dbcontext
        private readonly HotelListingDbContext _context;

        public CountriesController(HotelListingDbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            //IEnumerable -  it is like IList, or collection. It simply means expect a list of countries
            //return await _context.Countries.ToListAsync(); // --> it is like SELECT * FROM COUNTRIES
            //to return by status code
            var countries = await _context.Countries.ToListAsync();
            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")] //it means we expect a id value....... Suppose if we want have more values in url like api/Countries/5/hotelId/10 ---> then [HttpGet("{id}/hotelId/{hotelId}")]
        public async Task<ActionResult<Country>> GetCountry(int id) //whatever id we send in url will be in this variable. The above hotelId can be accessed here by int hotelId.
        {
            var country = await _context.Countries.FindAsync(id); // --> it is just a query for getting country with that id

            //if there is no country with that id
            if (country == null)
            {
                return NotFound(); //404 not found result
            }

            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country) //Country country -  PUT will replace the entire data of particular id. It will not look for whether name is changed or id is changed. It will just replace the entire thing. So only we have this object Country country.
        {
            //suppose if we are sending coutry id as 5 in body and we have passed 10 in url
            //we have to send it as bad request
            if (id != country.Id)
            {
                return BadRequest("Invalid Record Id"); //Inside double quote is Invalid Record Id
            }

            //In EF, We need to always change the state
            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // we are just updating
            }
            catch (DbUpdateConcurrencyException) //if two different users try to change the same data
            {
                // CountryExists(id) -- see down for reference
                if (!CountryExists(id)) // where the country u r editing might be deleted
                {
                    return NotFound();
                }
                else
                {
                    throw;//to kill runtime of program
                }
            }

            //We did the operation but we have nothing to show to them
            return NoContent();
        }

        //-----------------TASK----------------------
        // A task in C# is used to implement Task-based Asynchronous Programming.
        // The Task object is typically executed asynchronously on a thread pool
        // thread rather than synchronously on the main thread of the application.

        //-----------------ACTION------------------
        // An action is capable of returning a specific data type
        // When multiple return types are possible, it's common to
        // return ActionResult, IActionResult or ActionResult<T>,
        // where T represents the data type to be returned.

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //annotation - it is a declartion to controller that whenever post req comes in with api/countries below method will execute
        public async Task<ActionResult<Country>> PostCountry(Country country) //PostCountry - name of the method
        {
            //ActionResult<Country> -> country object is expected to return
            _context.Countries.Add(country); // here _context is copy of our db context. So we are saying In countries table add country
            await _context.SaveChangesAsync(); // and we are saving changes. The above line will queue it up. and this line will execute it. 

            //then we have to return the action
            //GetCountry - The name of the action to use for generating the URL.
            // new { id = country.Id } - The route data to use for generating the URL.
            //country - The content value to format in the entity body.
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);

            //When posting if we mention id:0 no problem because EF will understand that it has to assign id
            // if we mention any specific id, we will get error because of constraint
            //And it will be like:
            /*{
                "id": 0,
                "name": "string",
                "shortName": "string",
                "hotels": [
                {
                                "id": 0,
                    "name": "string",
                    "address": "string",
                    "rating": 0,
                    "countryId": 0,
                    "country": "string"
                }
                ]
            }*/
            // here we get options for adding hotels as array also
            // but we can delete it
            //
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            // to check if country is there or not
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country); // Now the country with this id will be in deleted state
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
