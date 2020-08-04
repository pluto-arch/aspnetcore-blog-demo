﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluto.BlogCore.API.Controllers;
using Pluto.BlogCore.API.Models;


namespace Pluto.BlogCore.Test.ApiTest
{
    public class ControllerTest:BaseTest
    {

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