namespace WestcoastEducation.api.ViewModels
{
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        public string BirthOfDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}