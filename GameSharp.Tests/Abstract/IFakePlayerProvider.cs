using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Core.Abstract;
using GameSharp.Core.Entities;

namespace GameSharp.Tests.Abstract
{
    internal interface IFakePlayerProvider : IPlayerProvider
    {
        Task<Player> Authenticate(Func<IQueryable<Player>, Task<Player>> action);
        IEnumerable<Player> Players { get; }
    }
}