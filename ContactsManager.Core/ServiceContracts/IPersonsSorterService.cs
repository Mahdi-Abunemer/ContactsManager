using ContactsManager.Core.Enums;
using ServiceContracts.DTO;
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
    public  interface IPersonsSorterService
    {
       
        /// <summary>
        /// Return sorted list of persons 
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort </param>
        /// <param name="sortBy">Name of the property(key), based on 
        /// which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DECS</param>
        /// <returns>Return sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse>
            allPersons , string sortBy , SortOrderOptiions sortOrder);
    }
}
