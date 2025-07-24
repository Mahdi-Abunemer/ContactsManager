using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
namespace Services
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        // private fileds
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext; 
        //constructor 
        public PersonsDeleterService( IPersonsRepository personsRepository, ILogger<PersonsDeleterService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext; 
           
        }
        
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null) return false;
           Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);
            if (person == null) return false;
            await _personsRepository.DeletePersonByPersonID(personID.Value);
       
            return true;  
        }

    }
}

