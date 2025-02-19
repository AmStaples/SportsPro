using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Models
{
    public class Customer
    {
		public int? CustomerID { get; set; }

		[Required(ErrorMessage = "Please enter a first name.")]
		[StringLength(50, ErrorMessage = "First name must be 50 characters or less.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50, ErrorMessage = "Last name must be 50 characters or less.")]
        public string LastName { get; set; }

		[Required(ErrorMessage = "Please enter an address.")]
        [StringLength(50, ErrorMessage = "Address must be 50 characters or less.")]
        public string Address { get; set; }

		[Required(ErrorMessage = "Please enter a city.")]
        [StringLength(50, ErrorMessage = "City must be 50 characters or less.")]
        public string City { get; set; }

		[Required(ErrorMessage = "Please enter a state.")]
        [StringLength(50, ErrorMessage = "State must be 50 characters or less.")]
        public string State { get; set; }

		[Required(ErrorMessage = "Please enter a postal code.")]
		[StringLength(20, ErrorMessage = "Postal code must be 20 characters or less.")]
		public string PostalCode { get; set; }

		[Required(ErrorMessage = "Please select a country.")]
		public string CountryID { get; set; }
		public Country Country { get; set; }

		[StringLength(20, ErrorMessage = "Phone number must be 20 characters or less.")]
		[RegularExpression("^\\(\\d{3}\\) \\d{3}-\\d{4}$", ErrorMessage = "Phone number must be in (999) 999-9999 format.")]
		public string Phone { get; set; }

		[StringLength(50, ErrorMessage = "Email address must be 50 characters or less.")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
		[Remote("CheckEmail", "Customer", AdditionalFields = nameof(CustomerID))]
		public string Email { get; set; }

		public string FullName => FirstName + " " + LastName;   // read-only property
	}
}