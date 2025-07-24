using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Cryptography.X509Certificates;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private filed 
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
         _countriesRepository = countriesRepository;
        }
        
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Validation : check if countryAddRequest is not null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest)); 
            }


            //Validate all properties of countryAddRequest 
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest));
            }

            // check if the country exists!!
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) !=null) 
            {
                throw new ArgumentException("Country cann't be duplicated!!");
            }
            //
            //Convert the object form countryAddRequest to country type 
            Country country = countryAddRequest.ToCountry();


            //Generate a new CountryID
            country.CountryID = Guid.NewGuid(); 



            //Add the country to the _countries instance 
            await _countriesRepository.AddCountry(country);
          
            CountryResponse response = country.ToCountryResponse();

            return response; 
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries())
                .Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? counrtyID)
        {
            if (counrtyID == null)
                return null;
            Country? country_from_list =
               await _countriesRepository.GetCountryByCountryID(counrtyID.Value); // value because the guid here is nullable 
            if (country_from_list == null)
                return null;
            return country_from_list.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if (_countriesRepository.GetCountryByCountryName(countryName) == null)
                        {
                            Country country = new Country() { countryName = countryName };
                            await _countriesRepository.AddCountry(country);
                          
                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}
