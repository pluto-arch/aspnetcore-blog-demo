using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluto.BlogCore.API.Controllers;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.Infrastructure.Providers;


namespace Pluto.BlogCore.Test.ApiTest
{
    public class ControllerTest:BaseTest
    {


        [Test]
        public void Id_Demo()
        {

            IEnumerable<string> ad=new []{"1111","123","!@32"};
            var ads= ad.Where(x => x.Length < 1);
            Console.WriteLine(ads);
            // var aaa=new List<Thread>();
            // for (int i = 0; i < 10; i++)
            // {
            //     new Thread(aad2).Start();
            // }

            // var aaa2=new List<Thread>();
            // for (int i = 0; i < 10; i++)
            // {
            //     aaa2.Add(new Thread(aad2));
            // }

        }



        public void aad()
        {
            var sb=new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                var aaa = EntityIdGenerateProvider.GenerateIntId();
                sb.AppendLine(aaa.ToString());
                Console.WriteLine(aaa);
            }
        }
        public void aad2()
        {
            var sb=new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                var aaa = EntityIdGenerateProvider.GenerateLongId();
                sb.AppendLine(aaa.ToString());
                Console.WriteLine(aaa);
            }
            File.WriteAllText("b.txt",sb.ToString());
        }
        
        
        [Test]
        public async Task GET_api_Demo()
        {

            

            
            
            using (var scope = _Container.BeginLifetimeScope())
            {
                // var _demoController = scope.Resolve<UserController>();
                // var res= await _demoController.PostAsync(new API.Models.Requests.CreateUserRequest
                // {
                //     UserName = Guid.NewGuid().ToString("N"),
                //     Password = "admin123"
                // });
                // Assert.IsTrue(res.Code==AppResponseCode.Success);
            }
        }
    }

}