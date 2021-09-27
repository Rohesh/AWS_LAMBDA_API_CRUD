using System;
using Xunit;
using Moq;
using Amazon.S3;
using System.Threading;
using Amazon.S3.Model;
using System.Threading.Tasks;
using AWS_LAMBDA_API_CRUD.Controllers;
using AWS_LAMBDA_API_CRUD.Services;
using System.Collections.Generic;
using AWS_LAMBDA_API_CRUD.Models;

namespace AWS_LAMBDA_API_CRUD.Test
{
    public class Xunit_Test
    {
        //private Mock<IAmazonS3> mockAmazonClient;
        private Mock<IS3Service> mockS3Service;
        protected CatalogController catalogController;


        [Fact]
        public void GetCatalogByIdTest()
        {
            TestSetup();
            //mockAmazonClient.Setup(m => m.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            //    .Returns(Task.FromResult(new GetObjectResponse
            //    {
            //        BucketName = "sachinmicroservicebucket",
            //        HttpStatusCode = System.Net.HttpStatusCode.OK
            //    }));
            mockS3Service.Setup(m => m.GetCompanyFromS3("1"))
                .Returns(Task.FromResult(new Models.Company
                {
                    Id = "1",
                }));

            var result = catalogController.GetCatalogById("1");
            Assert.NotNull(result);
        }

        private void TestSetup()
        {
            //mockAmazonClient = new Mock<IAmazonS3>();
            mockS3Service = new Mock<IS3Service>();
            catalogController = new CatalogController(mockS3Service.Object);
        }

        [Fact]
        public void GetAllCatalogTest()
        {
            IEnumerable<Company> products = GetList();

            TestSetup();
            mockS3Service.Setup(m => m.GetAllCompanysFromS3()).Returns(
                Task.FromResult(products));
            var result = catalogController.GetAllCatalogDetails();
            Assert.NotNull(result.Result);
        }
        private IEnumerable<Company> GetList()
        {
            List<Company> products = new List<Company>();
            products.Add(new Company
            {
                Id = "2",
            });
            return products;
        }
    }

}
