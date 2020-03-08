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
    [Route("api/careercloud/applicant/v1")]
    [ApiController]
    public class ApplicantProfileController : ControllerBase
    {
        private readonly ApplicantProfileLogic _logic;

        public ApplicantProfileController()
        {
            var repo = new EFGenericRepository<ApplicantProfilePoco>();
            _logic = new ApplicantProfileLogic(repo);
        }

        [HttpGet]
        [Route("profile/{id}")]
        public ActionResult GetApplicantProfile(Guid id)
        {
            ApplicantProfilePoco poco = _logic.Get(id);
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
        [Route("profile")]
        public ActionResult GetApplicantProfile()
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
        [Route("profile")]
        public ActionResult PostApplicantProfile([FromBody]ApplicantProfilePoco[] pocos)
        {
            _logic.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("profile")]
        public ActionResult PutApplicantProfile([FromBody]ApplicantProfilePoco[] pocos)
        {
            _logic.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("profile")]
        public ActionResult DeleteApplicantProfile([FromBody]ApplicantProfilePoco[] pocos)
        {
            _logic.Delete(pocos);
            return Ok();
        }

    }
}