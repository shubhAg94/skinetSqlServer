namespace Core.Entities.OrderAggregate
{
    /*This is a kind of entity that's considered a value entity. it's going to be owned by our order. So it doesn't have an Id*/
    public class Address
    {
        //Entity Framework Need this
        public Address()
        {

        }
        public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            Zipcode = zipcode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
