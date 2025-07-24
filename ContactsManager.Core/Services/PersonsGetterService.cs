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

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        // private fileds
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService > _logger;
        private readonly IDiagnosticContext _diagnosticContext; 
        //constructor 
        public PersonsGetterService( IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext; 
        }
        
        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPerson of PersonService");
            var persons = await _personsRepository.GetAllPersons();
           return persons.Select(temp =>
            temp.ToPersonResponse()).ToList(); 
            // i can use _db.sp-GetAllPersons() but i have to recreate a new mcration with
            //the new cloum TIN so that it will overried the sotred prousedsre 
           
        }

        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;
            Person? p = await _personsRepository.GetPersonByPersonID(personID.Value);
            if (p == null) return null;
            return p.ToPersonResponse(); 
        }

        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {

            _logger.LogInformation("GetFilteredPersons of PersonService");
            List<Person> people;
            using (Operation.Time("Time For Filtered Persons From Database"))
            {
                people = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                        await _personsRepository.GetFilteredPersons(temp =>
                      temp.PersonName.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                         await _personsRepository.GetFilteredPersons(temp =>
                       temp.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                          await _personsRepository.GetFilteredPersons(temp =>
                        temp.DateOfBirth.Value.ToString("dd MM yyyy")
                                        .Contains(searchString)),

                    nameof(PersonResponse.Gender) =>
                          await _personsRepository.GetFilteredPersons(temp =>
                        temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryID) =>
                         await _personsRepository.GetFilteredPersons(temp =>
                       temp.Country.countryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                          await _personsRepository.GetFilteredPersons(temp =>
                        temp.Address.Contains(searchString)),


                    _ => await _personsRepository.GetAllPersons()


                };
            }
            _diagnosticContext.Set("Persons", people);
            return people.Select(temp => temp.ToPersonResponse()).ToList(); 
        }
      
      

        

      
        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
            //PersonName,Email,,,
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.PersonID));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.CountryID));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.NextRecord();

            csvWriter.WriteHeader<PersonResponse>(); //
            //PersonID,PersonName
            csvWriter.NextRecord();
            List<PersonResponse> persons = await GetAllPersons();

            foreach(PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.PersonID);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.CountryID);
                csvWriter.WriteField(person.DateOfBirth);
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Address);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
                  
            memoryStream.Position = 0;
            return memoryStream; 
        }

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {

           MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Date of Birth";
                workSheet.Cells["D1"].Value = "Age";
                workSheet.Cells["E1"].Value = "Gender";
                workSheet.Cells["F1"].Value = "Country";
                workSheet.Cells["G1"].Value = "Address";
                workSheet.Cells["H1"].Value = "Receive News Letters";

                using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    workSheet.Cells[row, 1].Value = person.PersonName;
                    workSheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                        workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    workSheet.Cells[row, 4].Value = person.Age;
                    workSheet.Cells[row, 5].Value = person.Gender;
                    workSheet.Cells[row, 6].Value = person.Country;
                    workSheet.Cells[row, 7].Value = person.Address;
                    workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                    row++;
                }

                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

