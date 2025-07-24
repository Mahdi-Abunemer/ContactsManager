using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity 
    /// </summary>
    public interface  ICountriesService
    {
        /// <summary>
        /// Adds a counrty object to the list of countries 
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Return the country object after adding it 
        /// (including newly generated country id)</returns>
         Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Return All the countries from the list 
        /// </summary>
        /// <returns>All contries from the list as CountryResponse 
        /// </returns>
        Task<List<CountryResponse>> GetAllCountries();
        /// <summary>
        /// Returns a country object based on the given country id  
        /// </summary>
        /// <param name="counrtyGuid">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? counrtyID);


        /// <summary>
        /// Upload Countries From Excel File into Database
        /// </summary>
        /// <param name="formFile">Excel File with list of countries</param>
        /// <returns>Return numbers of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
