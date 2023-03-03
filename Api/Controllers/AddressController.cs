using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Store.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly MyStoreContext _storeContext;
        UserManager<UserModel> _userManager;
        public AddressController(ILogger<AddressController> logger, MyStoreContext storeContext, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _storeContext = storeContext;
            _userManager = userManager;
        }

        // GET api/<AddressController>/userId
        [HttpGet("{userId}")]
        public JsonResult GetAll(string userId)
        {
            try
            {
                _logger.LogWarning("parameter userId: {0}", userId);
                var aAddress = (
                    from a in _storeContext.Address
                    where a.UserId == userId
                    select a
                ).ToList<AddressModel>();

                if (aAddress != null)
                {
                    return new JsonResult(aAddress);
                }
                else
                {
                    return new JsonResult(
                        new { status = "fail", message = "Address is not exist." }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new JsonResult(new { status = "error", message = ex.Message });
            }
        }

        // POST api/<AddressController>/add
        [HttpPost("add")]
        public ObjectResult AddOne(dynamic dAddress)
        {
            try
            {
                var id = dAddress.UserId.ToString();
                var user = _userManager.FindByIdAsync(id);
                if (user.Result != null)
                {
                    dAddress.Id = Guid.NewGuid().ToString();
                    var mAddress = new AddressModel();
                    mAddress.SetValues(dAddress);

                    _storeContext.Address.Add(mAddress);
                    var numOfRE = _storeContext.SaveChanges();

                    if (numOfRE != 0)
                    {
                        return StatusCode(201, new JsonResult(new { status = "success", message = "Add successful." }));
                    }
                    else
                    {
                        return StatusCode(501, new JsonResult(new { status = "fail", message = "Add fail." }));
                    }

                }
                else
                {
                    return StatusCode(404, new JsonResult(
                        new { status = "fail", message = "User does not exist." }
                    ));
                }

            }
            catch (RuntimeBinderException ex)
            {
                return StatusCode(500, new JsonResult(new
                {
                    status = "error",
                    message = ex.Message
                }));
            }
        }

        // PUT api/<AddressController>/5
        [HttpPut("{id}")]
        public void Update(string id)
        {
            try
            {

            }
            catch (Exception ex) { }
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            try { }
            catch (Exception ex) { }
        }
    }
}
