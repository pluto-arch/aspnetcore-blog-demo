﻿using MediatR;

using System;

namespace Pluto.BlogCore.Application.CommandBus.IdentityCommand
{
    public class IdentifiedCommand<T, R> : IRequest<R>
        where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }
        public IdentifiedCommand(T command)
        {
            Command = command;
            Id = Guid.NewGuid();
        }
    }
}