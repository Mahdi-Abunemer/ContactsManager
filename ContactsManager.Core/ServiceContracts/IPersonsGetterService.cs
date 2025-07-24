using ServiceContracts.DTO;
using ContactsManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Repersents business logic for manipulating 
    /// Person entity
    /// </summary>
    public  interface IPersonsGetterService
    {
        
        /// <summary>
        /// Returns all persons 
        /// </summary>
        /// <returns>Returns a list of objects of PersonsResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the  person object based on the given person id 
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Return mathcing person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);
        /// <summary>
        /// Return all person object that matches with the given 
        /// search filed and search string 
        /// </summary>
        /// <param name="searchBy">Search filed to search</param>
        /// <param name="searchString">Search string  to search</param>
        /// <returns>Return all matches persons based on the given search filed and
        ///search string  </returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy,
            string? searchString);
      

        /// <summary>
        /// Return persons as CSV
        /// </summary>
        /// <returns>Returns the memory streeam with CSV data</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns Persons AS Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
