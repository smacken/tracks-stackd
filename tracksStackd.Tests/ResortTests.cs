
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tracksStackd;
using ServiceStack.Text;

namespace tracksStackd.Tests
{
    [TestFixture(Category="Integration")]
    public class With_Resorts
    {
        AppHost _appHost;
        string _restBase = "http://localhost:1337/";
        
        [TestFixtureSetUp]
        public void Pre_test()
        {
            _appHost = new AppHost();
            _appHost.Init();
            _appHost.InitDb();
            _appHost.Start(_restBase);
        }

        [TestFixtureTearDown]
        public void Post_test()
        {
            _appHost.Stop();
        }

        [Test]
        public void Should_Delete_Resort()
        {
            var response = Path.Combine(_restBase, "api/resort?Name=Revelstoke").DeleteFromUrl();

            Assert.That(response != string.Empty);
        }

        //[Test]
        //public void Should_Add_Resort()
        //{
        //    // arrange
        //    var newResort = new Resorts.Resort
        //    {
        //        Name = "Revelstoke",
        //        Region = "B.C.",
        //        Country = "Canada"
        //    };

        //    //var deleteResponse = Path.Combine(_restBase, "api/resort").DeleteFromUrl();
        //    //Assert.That(newResort != null);

        //    // act
        //    var response = Path.Combine(_restBase + "api/resort").PostToUrl(newResort);

        //    // assert
        //    Assert.That(response.GetResponseStatus().Value == System.Net.HttpStatusCode.Created);
        //}

        [Test]
        public void Should_retrieve_resort_list()
        {
            var resorts = Path.Combine(_restBase, "api/resorts").GetJsonFromUrl().FromJson<List<Resorts.Resort>>();
            //var resorts = _restBase + "api/resorts".GetJsonFromUrl().FromJson<List<Resorts.Resort>>();
            Assert.That(resorts.Count() > 0);
        }
            
        
    }
}
