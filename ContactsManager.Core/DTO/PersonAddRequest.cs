using ContactsManager.Core.Enums;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts  as a DTO for inserting a new person 
    /// </summary>
    public class PersonAddRequest
    {
        [Required (ErrorMessage = "Person Name can't be blank!")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Person Name can't be blank!")]
        [EmailAddress(ErrorMessage ="Email Value should be vaid")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage = "Country ID can't be blank!")]
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }


        /// <summary>
        /// Convert the current object of 
        /// PersonAddRequest into a new objet of person type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email , 
                DateOfBirth = DateOfBirth , 
                Gender = Gender.ToString() , 
                CountryID = CountryID , 
                Address =Address , 
                ReceiveNewsLetters = ReceiveNewsLetters 

            };
        }
    }
     

}
