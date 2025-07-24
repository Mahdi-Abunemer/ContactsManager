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
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ContactsManager.Core.Enums;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        // private fileds
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext; 
        //constructor 
        public PersonsSorterService( IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext; 
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, 
            string sortBy, SortOrderOptiions sortOrder)
        {
            _logger.LogInformation("GetSortedPerson of PersonsService");
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), (SortOrderOptiions.ASC)) =>
                allPersons.OrderBy(temp => temp.PersonName,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.PersonName,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), (SortOrderOptiions.ASC)) =>
                allPersons.OrderBy(temp => temp.Email,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.Email,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), (SortOrderOptiions.ASC)) =>
                allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), (SortOrderOptiions.ASC)) =>
              allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), (SortOrderOptiions.ASC)) =>
               allPersons.OrderBy(temp => temp.Gender,
               StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.Gender,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), (SortOrderOptiions.ASC)) =>
               allPersons.OrderBy(temp => temp.Country,
               StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.Country,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), (SortOrderOptiions.ASC)) =>
             allPersons.OrderBy(temp => temp.Address,
             StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.Address,
                StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), (SortOrderOptiions.ASC)) =>
            allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), (SortOrderOptiions.DESC)) =>
                allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                
            };
            return sortedPersons; 
        }
        
    }
}

