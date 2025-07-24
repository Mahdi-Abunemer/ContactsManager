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
    public  interface IPersonsDeleterService
    {
        
        /// <summary>
        /// Delete persob based on the given person id 
        /// </summary>
        /// <param name="PersonID">PersonID to delete</param>
        /// <returns>Returns true if the deletion is successful; otherwise false </returns>
        Task<bool> DeletePerson(Guid? personID);

    }
}
