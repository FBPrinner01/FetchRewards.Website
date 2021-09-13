using FetchRewards.Website.Models;
using FetchRewards.Website.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FetchRewards.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(JsonFileProductService productService)
        {
            this.ProductService = productService;
        }

        public JsonFileProductService ProductService { get;  }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return ProductService.GetAccounts();

        }

        //[HttpPatch] "[FromBody]"
        //[Route("Rate")]
        //[HttpGet]
        //public ActionResult Get(
        //    )
        //{
        //    //ProductService.AddRating(ProductID, Rating);
        //    return Ok();
        //}
    }
}
