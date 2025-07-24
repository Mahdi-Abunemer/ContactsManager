using ContactsManager.Core.Enums;
using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExceptionFilter;
using CRUDExample.Filters.ResourceFilter;
using CRUDExample.Filters.ResultFilter;
using CRUDExample.Filters.ResultFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDExample.Controllers
{
    //URL : Persons
    [ResponsHeaderFilterFactoryAttribut("MyController-Custom-Key","MyController-Custom-Value",3 )]

    [Route("[controller]")]
    [TypeFilter(typeof(PersonAlywasRunResultFilter))]
    public class PersonsController : Controller
    {
        //private filed
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;
        //constructor 
        public PersonsController(IPersonsGetterService personsGetterService,  
            IPersonsAdderService personsAdderService , IPersonsUpdaterService personsUpdaterService ,
             IPersonsDeleterService personsDeleterService , IPersonsSorterService personsSorterService , 
            ICountriesService countriesService,
            ILogger<PersonsController> logger)
        {
            _personsGetterService = personsGetterService;
            _personsAdderService = personsAdderService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _personsSorterService = personsSorterService;
          _countriesService = countriesService;
            _logger = logger;
        }


        //Url : Persons/Index
        [Route("[action]")]
        [Route("/")]
        [ServiceFilter(typeof(PersonsListActionFilter) , Order = 1)]
        [ResponsHeaderFilterFactoryAttribut("MyAction-Custom-Key", "MyAction-Custom-Value", 4)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy =
            nameof(PersonResponse.PersonName), SortOrderOptiions sortOrder
            = SortOrderOptiions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"seracgBy :{searchBy} , serachString: {searchString}");
       
            //Search
            List<PersonResponse> persons = await _personsGetterService.GetFilteredPersons(searchBy, searchString);
            
            // sort
            List<PersonResponse> sortedPersons = await _personsSorterService.GetSortedPersons(persons,
                sortBy, sortOrder);
            
            return View(sortedPersons);
        }
        
        
        //Executes with the user clicks on "Create Person" hyperlink
        //(while opening the create view)
        //Url : Persons/create
        [Route("[action]")]
        [HttpGet]
        [ResponsHeaderFilterFactoryAttribut("My-Key", "My-Value",4 )]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countryResponses =
                await _countriesService.GetAllCountries();
            ViewBag.Countries = countryResponses;
            return View();
        }


        [HttpPost]
        //Url : Persons/create
        [Route("[action]")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter))]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            PersonResponse personResponse =
                await _personsAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }


        [HttpGet]
        [Route("[action]/{personID}")] //Eg: /persons/edit/1
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.countryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]

        [TypeFilter(typeof(TokenAuthorizationFilter))]
        
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            
                PersonResponse updatedPerson = await _personsUpdaterService.UpdatePerson(personRequest);
                return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
                return RedirectToAction("Index");

            return View(personResponse);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personUpdateResult.PersonID);
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personsDeleterService.DeletePerson(personUpdateResult.PersonID);
            return RedirectToAction("Index");
        }


        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            //Get list of persons
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();

            //Return view as pdf
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }


        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream",
                "person.csv");
        }


        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}

