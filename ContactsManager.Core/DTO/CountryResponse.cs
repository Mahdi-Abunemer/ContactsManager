using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type from most 
    /// of CountriesService methods 
    /// </summary>
    public class CountryResponse 
    {
        public Guid CountryID { get; set; }
        public string? countryName { get; set; }
        /// <summary>
        /// overrideing the Equals method becaues it cmpare just the 
        /// refernce not the acutl value 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false; 
            }
            if(obj.GetType() != typeof(CountryResponse))
            {
                return false; 
            }
            CountryResponse country_to_camper = (CountryResponse)obj;
            return CountryID == country_to_camper.CountryID &&
                countryName == country_to_camper.countryName; 
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class CountryExtensions
    {
        //Country object To Country Response object
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                countryName = country.countryName,
                CountryID = country.CountryID
            }; 
        }
    }  
}


