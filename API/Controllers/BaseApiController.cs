using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //Q. Why to inherit from ControllerBase instead of Controller like in MVC applications.
    //The reason is because Controller class derives from the ControllerBase and it only adds few members that have needed to support 
    //views inside an MVC application here. Since we do not need any of the view support we can derive from ControllerBase directly.
    public class BaseApiController : ControllerBase
    {

    }
}
