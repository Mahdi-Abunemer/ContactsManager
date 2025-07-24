using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ContactsManager.Core.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Exceptions;
namespace Services
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        // private fileds
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext; 
        //constructor 
        public PersonsUpdaterService( IPersonsRepository personsRepository, ILogger<PersonsUpdaterService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext; 
        }
        
        //    //8/03/2025
       
        public async  Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidtion(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson =await  _personsRepository.GetPersonByPersonID
                (personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new  InvalidPersonIDException("Given person id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            await _personsRepository.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonResponse();
        }

    }
}

