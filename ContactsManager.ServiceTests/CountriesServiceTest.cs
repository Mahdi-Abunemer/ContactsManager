using AutoFixture;
using Entities;
using FluentAssertions;

using Moq;

using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IFixture _fixture;
        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object; 
            _countriesService = new CountriesService(_countriesRepository);
        }
        #region AddCountry
        //When CountryAddRequest is null ,it should throw ArgumentNullException 
        [Fact]
        public async Task AddCountry_NullCountry_ToThrowArgumentNullException()
        {
            // Arrange
            CountryAddRequest? request = null;
            //Assert
           await Assert.ThrowsAsync<ArgumentNullException>(async ()=>
            {
                //Act
               await _countriesService.AddCountry(request);
            });

        }
        //When CountryName is null ,it should throw ArgumentException 
        [Fact]
        public async Task AddCountry_NullName_ToThrowArgumentException()
        {
            // Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string).Create();
            Country country = request.ToCountry();
            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
                 //Assert
                 await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                //Act
               await _countriesService.AddCountry(request);
            });

        }
        //When CountryName is duplicated ,it should throw ArgumentException 
        [Fact]
        public async Task AddCountry_DuplicatedName_ToThrowArgumentException()
        {
            // Arrange
            CountryAddRequest request1 = _fixture.Build<CountryAddRequest>().
                With(temp => temp.CountryName,"USA").
                Create();
           
            Country country = request1.ToCountry();
           
            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
            _countriesRepositoryMock.Setup(repo => repo.GetCountryByCountryName("USA"))
         .ReturnsAsync(country); // Simulate that "USA" already exists

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
               await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request1);
            });

        }
        //When you supply proper country name , it should insert 
        //(add) country to the existing list of countries
        [Fact]
        public async Task AddCountry_FullCountry_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            Country country = country_request.ToCountry();
            CountryResponse country_response = country.ToCountryResponse();

            _countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);


            //Act
            CountryResponse country_from_add_country = await _countriesService.AddCountry(country_request);

            country.CountryID = country_from_add_country.CountryID;
            country_response.CountryID = country_from_add_country.CountryID;

            //Assert
            country_from_add_country.CountryID.Should().NotBe(Guid.Empty);
            country_from_add_country.Should().BeEquivalentTo(country_response);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        //The list of the countries should be empty by default
        //(before adding any country)
        public async Task GetAllCountries_ToBeEmptyList()
        {
            //Arrange
            List<Country> country_empty_list = new List<Country>();
            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_empty_list);

            //Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

            //Assert
            actual_country_response_list.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountries_ShouldHaveFewCountries()
        {
            //Arrange
            List<Country> country_list = new List<Country>() {
                    _fixture.Build<Country>()
                    .With(temp => temp.Persons, null as List<Person>).Create(),
                    _fixture.Build<Country>()
                    .With(temp => temp.Persons, null as List<Person>).Create()
                  };

            List<CountryResponse> country_response_list = country_list.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_list);

            //Act
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert
            actualCountryResponseList.Should().BeEquivalentTo(country_response_list);
        }

        #endregion
        #region GetCountryByCountryID
        [Fact]
        // if we supply null as CountryID , it should return null as CountryResponse
        public async Task GetCountryByCountryID_NullCountryID_ToBeNull()
        {
            //Arrange
            Guid? countryID = null;

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(null as Country);

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);


            //Assert
            country_response_from_get_method.Should().BeNull();
        }

        [Fact]
        // If we supply a vaild country id , it should return the matching country 
        //details as CountryObject object
        public async Task GetCountryByCountryID_ValidCountryID_ToBeSuccessful()
        {
            //Arrange
            Country country = _fixture.Build<Country>()
              .With(temp => temp.Persons, null as List<Person>)
              .Create();
            CountryResponse country_response = country.ToCountryResponse();

            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(country);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country.CountryID);


            //Assert
            country_response_from_get.Should().Be(country_response);
        }

        #endregion
    }
}
