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
    public  interface IPersonsUpdaterService
    {
       
        
        /// <summary>
        /// Updates the specified person details based on the given person ID 
        /// </summary>
        /// <param name="personUpdateRequest">Person deatils to update,
        /// including person id</param>
        /// <returns>Return the person </returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
