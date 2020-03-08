using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCloud.WebAPI.Controllers
{
    [Route("api/careercloud/system/v1")]
    [ApiController]
    public class SystemCountryCodeController : ControllerBase
    {
        private readonly SystemCountryCodeLogic _logic;

        public SystemCountryCodeController()
        {
            var repo = new EFGenericRepository<SystemCountryCodePoco>();
            _logic = new SystemCountryCodeLogic(repo);
        }

        [HttpGet]
        [Route("countrycode/{code}")]
        public ActionResult GetSystemCountryCode(string code)
        {
            SystemCountryCodePoco poco = _logic.Get(code);
            if (poco == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poco);
            }
        }

        [HttpGet]
        [Route("countrycode")]
        public ActionResult GetSystemCountryCode()
        {
           var pocos= _logic.GetAll();
            if (pocos==null)
            {
                return NotFound();

            }
            else {
                return Ok(pocos);
            }
        }


        [HttpPost]
        [Route("countrycode")]
        public ActionResult PostSystemCountryCode([FromBody]SystemCountryCodePoco[] pocos)
        {
            _logic.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("countrycode")]
        public ActionResult PutSystemCountryCode([FromBody]SystemCountryCodePoco[] pocos)
        {
            _logic.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("countrycode")]
        public ActionResult DeleteSystemCountryCode([FromBody]SystemCountryCodePoco[] pocos)
        {
            _logic.Delete(pocos);
            return Ok();
        }

    }
}