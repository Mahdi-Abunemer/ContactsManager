using ServiceContracts.DTO;
using ContactsManager.Core;
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
    public  interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add </param>
        /// <returns>Returns the same person details, 
        /// along with newly generated PersonID</returns>
       Task <PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
