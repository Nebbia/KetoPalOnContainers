using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using KetoPal.Core;
using KetoPal.Core.Models;
using KetoPal.Infrastructure;
using LaunchDarkly.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KetoPal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productsProvider;
        private readonly IUsersProvider _usersProvider;
        private readonly LdClient _ldClient;

        public ProductsController(LdClient ldClient, IProductsProvider productsProvider, IUsersProvider usersProvider)
        {
            _ldClient = ldClient;
            _productsProvider = productsProvider;
            _usersProvider = usersProvider;
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Product>>> Get([FromQuery] int userId)
        {
            var ldUser = LaunchDarkly.Client.User.WithKey("1");
            bool killSwitchEnabled = _ldClient.BoolVariation("killswitch.getproducts", ldUser, false);
            if (killSwitchEnabled)
            {
                // application code to show the feature
                return Ok(new List<Product>());
            }

            List<Product> products;

            if (userId > 0)
            {
                Core.Models.User user = await _usersProvider.FindUserById(userId);
                products = await _productsProvider.GetFoodProductsByCarbsForUser(user);
            }
            else
            {
                products = await _productsProvider.GetFoodProductsByCarbs();
            }

            products = products ?? new List<Product>();

            return Ok(products.ToList());
        }

        // POST api/products/_actions/consume
        [Route("_actions/consume")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<ActionResult> Consume([FromBody]ConsumeProductCommand command)
        {
            if (command.UserId > 0)
            {
                Core.Models.User user = await _usersProvider.FindUserById(command.UserId);
                user.RecordConsumption(command.CarbAmount);

                return NotModified();
            }

            return NotFound();
        }

        private ActionResult NotModified()
        {
            return new EmptyResult();
        }
    }

    public class ConsumeProductCommand
    {
        public int UserId { get; set; }
        public double CarbAmount { get; set; }
    }
}
