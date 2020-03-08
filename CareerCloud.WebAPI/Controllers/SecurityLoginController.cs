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
    [Route("api/careercloud/security/v1")]
    [ApiController]
    public class SecurityLoginController : ControllerBase
    {
        private readonly SecurityLoginLogic _logic;

        public SecurityLoginController()
        {
            var repo = new EFGenericRepository<SecurityLoginPoco>();
            _logic = new SecurityLoginLogic(repo);
        }

        [HttpGet]
        [Route("login/{id}")]
        public ActionResult GetSecurityLogin(Guid id)
        {
            SecurityLoginPoco poco = _logic.Get(id);
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
        [Route("login")]
        public ActionResult GetSecurityLogin()
        {
            var poco = _logic.GetAll();
            if (poco == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poco);
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult PostSecurityLogin([FromBody]SecurityLoginPoco[] pocos)
        {
            _logic.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("login")]
        public ActionResult PutSecurityLogin([FromBody]SecurityLoginPoco[] pocos)
        {
            _logic.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("login")]
        public ActionResult DeleteSecurityLogin([FromBody]SecurityLoginPoco[] pocos)
        {
            _logic.Delete(pocos);
            return Ok();
        }

    }
}