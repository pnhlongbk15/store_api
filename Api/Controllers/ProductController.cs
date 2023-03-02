using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Store.Models;
using System.Data;

namespace Store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly MyStoreContext _storeContext;
        public ProductController(ILogger<AddressController> logger, MyStoreContext storeContext)
        {
            _logger = logger;
            _storeContext = storeContext;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public JsonResult GetAll()
        {
            _logger.LogWarning("get all.");
            var aProducts = (from p in _storeContext.Product
                             select p).ToList<ProductModel>();

            return new JsonResult(aProducts);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public JsonResult GetOne(string id)
        {
            var product = (from p in _storeContext.Product
                           where p.Id == id
                           select p).FirstOrDefault();
            if (product != null)
            {
                return new JsonResult(product);
            }
            else
            {
                return new JsonResult(new { status = "fail", message = "Product does not exist." });
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public JsonResult AddOne(dynamic dProduct)
        {
            try
            {
                dProduct.Id = Guid.NewGuid().ToString();
                ProductModel mProduct = new ProductModel();
                mProduct.SetValues(dProduct);

                _storeContext.Add(mProduct);
                var numOfRE = _storeContext.SaveChanges();
                if (numOfRE != 0)
                {
                    return new JsonResult(new { status = "success", message = "Add successful." });
                }
                else
                {
                    return new JsonResult(new { status = "fail", message = "Add fail." });
                }
            }
            catch (RuntimeBinderException ex)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public JsonResult UpdateOne(string id, dynamic dProduct)
        {
            var product = (from p in _storeContext.Product
                           where p.Id == id
                           select p).FirstOrDefault();
            try
            {
                if (product != null)
                {
                    Console.WriteLine("Product: {0}", dProduct);
                    product.SetValues(dProduct);
                    _storeContext.SaveChanges();
                    return new JsonResult(new { status = "success", message = "Update successful." });
                }
                else
                {
                    return new JsonResult(new { status = "fail", message = "Product does not exist." });
                }
            }
            catch (RuntimeBinderException ex)
            {
                return new JsonResult(new { status = "error", message = ex.Message });
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public JsonResult RemoveOne(string id)
        {
            try
            {
                var product = (from p in _storeContext.Product
                               where p.Id == id
                               select p).FirstOrDefault();
                if (product != null)
                {
                    _storeContext.Remove(product);
                    _storeContext.SaveChanges();
                    return new JsonResult(new { status = "success", message = "Delete successful." });
                }
                else
                {
                    return new JsonResult(new { status = "fail", message = "Product does not exist." });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = "error", message = ex.Message });
            }
        }
    }
}
