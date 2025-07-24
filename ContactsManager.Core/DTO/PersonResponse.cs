﻿using ContactsManager.Core.Enums;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type 
    /// of most mthods of Person Service
    /// </summary>
    public  class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }
        public string? Country { get; set; }
        /// <summary>
        /// Compares the current object data with the parameter object 
        /// </summary>
        /// <param name="obj">The PersonResponse Object to compare </param>
        /// <returns>True or False , indicating whether all 
        /// person details are matched with the specified parameter object
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;
            PersonResponse person = (PersonResponse)obj;
            return PersonID == person.PersonID && PersonName == person.PersonName &&
                Email == person.Email && DateOfBirth == person.DateOfBirth &&
                Gender == person.Gender && CountryID == person.CountryID &&
                Address == person.Address && ReceiveNewsLetters ==
                person.ReceiveNewsLetters; 
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); 
        }
        public override string ToString()
        {
            return $"PersonID :{PersonID} , PersonName :{PersonName}," +
                $"Email: {Email} , Age :{Age} , DateOfBirth :{DateOfBirth?.ToString("dd-MM-yyyy")}" +
                $"Addres:{Address}"
                ;
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID , ReceiveNewsLetters = ReceiveNewsLetters ,
                Email = Email  , Address = Address , CountryID =CountryID ,
                DateOfBirth = DateOfBirth , PersonName = PersonName , 
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions) ,Gender , true) 
            };
        }
    }
    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class into 
        /// PersonResponse class
        /// </summary>
        /// <param name="person">The Person Object to convert</param>
        /// <returns>Returns the converted PersonResponse object </returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            // person => PersonResponse
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                Address = person.Address,
                PersonName = person.PersonName,
                DateOfBirth = person.DateOfBirth,
                Email = person.Email,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Age = (person.DateOfBirth != null) ?
                Math.Round((DateTime.Now - person.DateOfBirth.Value)
                .TotalDays / 365.25) : null,
                Country = person.Country?.countryName
            };
        }
    }
}
