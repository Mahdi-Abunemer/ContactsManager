using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ContactsManager.Core.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
    {
        // private fileds
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService > _logger;
        private readonly IDiagnosticContext _diagnosticContext; 
        //constructor 
        public PersonsAdderService( IPersonsRepository personsRepository, ILogger<PersonsAdderService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;   
        }
       
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest )
        {
            //check if the personAddRequest is null 
          if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest)); 
            }
           
            //Model Validations 
            ValidationHelper.ModelValidtion(personAddRequest);
              //convert personAddRequest into Person type 
              Person person = personAddRequest.ToPerson();
            //generat PersonID 
            person.PersonID = Guid.NewGuid();
            
            await  _personsRepository.AddPerson(person);
           
            // convert Person object into PersonResponse type
            return person.ToPersonResponse();
        }

    
    }
}

