using System.ComponentModel.DataAnnotations;
namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// </summary>
    public class Country
    {
        [Key] 
        public Guid CountryID { get; set; }

        [StringLength(90)]
        public string? countryName { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }
    }
}
