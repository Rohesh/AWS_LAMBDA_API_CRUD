using System.Collections.Generic;
using System.Threading.Tasks;
using AWS_LAMBDA_API_CRUD.Models;
using AWS_LAMBDA_API_CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace AWS_LAMBDA_API_CRUD.Controllers
{
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {


        private readonly IS3Service S3Service;

        public CatalogController(IS3Service _s3service)
        {
            S3Service = _s3service;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        

        [HttpPost]
        [Route("UploadCatalog")]
        public async Task<IActionResult> UploadCatalogDetails([FromBody] Company company)
        {
            return Ok(await S3Service.AddContentToS3(company));
        }

        [HttpGet("{key}", Name = "GetCompanyFromS3")]
        public async Task<IActionResult> GetCatalogById(string key)
        {
            return Ok(await S3Service.GetCompanyFromS3(key));
        }

        [HttpGet]
        [Route("GetAllCatalogDetails")]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCatalogDetails()
        {
            return Ok(await S3Service.GetAllCompanysFromS3());
        }
    }
}
