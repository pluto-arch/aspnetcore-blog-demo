﻿using System;
using System.Threading.Tasks;
using Pluto.BlogCore.Infrastructure.Exceptions;

namespace Pluto.BlogCore.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        public async Task<bool> ExistAsync(Guid id)
        {
            await Task.Delay(1);
            // todo 可以存redis或者数据库查询command 是否已经被执行了
            return false;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id,string cmdString)
        {
            await Task.Delay(1);
            var exists = await ExistAsync(id);
            // command 执行过了，抛出异常，否则创建request，然后将request信息保存到redis或者数据库
            var request = exists ? 
                              throw new RepeatedCommandException($"请勿重复执行：{id}") : 
                              new ClientRequest()
                              {
                                  Id = id,
                                  Name = typeof(T).Name,
                                  Time = DateTime.UtcNow,
                                  RequestData = cmdString
                              };
            // todo 保存request
        }
    }
}