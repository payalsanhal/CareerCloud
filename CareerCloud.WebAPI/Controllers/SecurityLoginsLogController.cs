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
    public class SecurityLoginsLogController : ControllerBase
    {
        private readonly SecurityLoginsLogLogic _logic;

        public SecurityLoginsLogController()
        {
            var repo = new EFGenericRepository<SecurityLoginsLogPoco>();
            _logic = new SecurityLoginsLogLogic(repo);
        }

        [HttpGet]
        [Route("loginlog/{id}")]
        public ActionResult GetSecurityLoginLog(Guid id)
        {
            SecurityLoginsLogPoco poco = _logic.Get(id);
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
        [Route("loginlog")]
        public ActionResult GetSecurityLoginLog()
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
        [Route("loginlog")]
        public ActionResult PostSecurityLoginLog([FromBody]SecurityLoginsLogPoco[] pocos)
        {
            _logic.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("loginlog")]
        public ActionResult PutSecurityLoginLog([FromBody]SecurityLoginsLogPoco[] pocos)
        {
            _logic.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("loginlog")]
        public ActionResult DeleteSecurityLoginLog([FromBody]SecurityLoginsLogPoco[] pocos)
        {
            _logic.Delete(pocos);
            return Ok();
        }

    }
}