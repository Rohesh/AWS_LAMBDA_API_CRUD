using AWS_LAMBDA_API_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS_LAMBDA_API_CRUD.Services
{
    public interface IS3Service
    {
        Task<bool> AddContentToS3(Company Company);

        Task<Company> GetCompanyFromS3(string id);

        Task<IEnumerable<Company>> GetAllCompanysFromS3();
    }
}
