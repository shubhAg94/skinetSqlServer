﻿using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    /*
     For validation we're going to take a look at a higher level and a higher level than
     our database(at database level validation we did for Product Entity in ProductConfiguration) is our entities.

     Now do we want to add validation at this level(address Entity). Is the question?
     --->And well we could but the downside to that is that we be adding extra responsibility and the dependency
     onto our entities and we want to avoid that and really to validate at this level you would choose the
     database validation in our entity configuration rather than doing it in our entities themselves.

     But we want to do it at the level where the user passes us the information.
     Now if we go to our controller and we take a look at the account controller and we take a look at the
     UpdateUserAddress method then we've actually got our address coming in as a dto and a dto is a good thing
     to use to validate against bad data because this is when we receive it from the user.

     So what we'll do is we'll add validation to our AddressDto to take care of a validation, where we
     don't want to accept no values in an address.
     */
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zipcode { get; set; }
    }
}
