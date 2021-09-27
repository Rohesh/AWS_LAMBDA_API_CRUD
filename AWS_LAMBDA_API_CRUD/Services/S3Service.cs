using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AWS_LAMBDA_API_CRUD.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AWS_LAMBDA_API_CRUD.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 amazonS3;
        private readonly IConfiguration configuration;
        AmazonS3Client amazonS3Client;
        string accessKey;
        string secretKey;

        public S3Service(IAmazonS3 _amazonS3, IConfiguration _configuration)
        {
            amazonS3 = _amazonS3;
            configuration = _configuration;
            accessKey = configuration.GetValue<string>("AWS:AccessKey");
            secretKey = configuration.GetValue<string>("AWS:SecretKey");
            amazonS3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSouth1 );
        }

        public async Task<bool> AddContentToS3(Company company)
        {
            var request = new PutObjectRequest
            {
                //''BucketName = configuration.GetValue<string>("ServiceConfiguration:BucketName"),
                 BucketName = "testbucketfinal",
                 Key= company.Id,
                ContentType = "application/json",
                ContentBody = JsonSerializer.Serialize(company)
            };
            var response = await amazonS3Client.PutObjectAsync(request);
            return true;
        }

        public async Task<Company> GetCompanyFromS3(string Id)
        {
            var response = await amazonS3Client.GetObjectAsync("testbucketfinal", Id);
            StreamReader reader = new StreamReader(response.ResponseStream);
            var content = reader.ReadToEnd();
            var CompanyName = JsonSerializer.Deserialize<Company>(content);
            return CompanyName;
        }

        public async Task<IEnumerable<Company>> GetAllCompanysFromS3()
        {
            try
            {

                AmazonS3Client amazonS3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSouth1);

                List<Company> products = new List<Company>();
                ListObjectsRequest listObjectsRequest = new ListObjectsRequest
                {
                    
                    BucketName = "testbucketfinal"
                };
                var listObjectResponse = await amazonS3Client.ListObjectsAsync(listObjectsRequest);
                foreach (var item in listObjectResponse.S3Objects)
                {
                    var fileContent = await amazonS3Client.GetObjectAsync("testbucketfinal", item.Key);
                    StreamReader reader = new StreamReader(fileContent.ResponseStream);
                    var content = reader.ReadToEnd();
                    var CompanyName = JsonSerializer.Deserialize<Company>(content);
                    products.Add(CompanyName);
                }
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<Company>();
            }
        }

    }
}
