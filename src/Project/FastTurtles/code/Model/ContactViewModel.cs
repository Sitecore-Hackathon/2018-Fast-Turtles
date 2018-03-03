namespace FastTurtles.Model
{
    using System.ComponentModel;

    public class ContactViewModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}