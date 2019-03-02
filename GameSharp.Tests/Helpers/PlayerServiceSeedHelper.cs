﻿using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Entities;
using GameSharp.Tests.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Tests.Helpers
{
    internal sealed class PlayerServiceSeedHelper : IPlayerServiceSeedHelper
    {
        public const string FirstPlayerUsername = "First Player";
        public const string SecondPlayerUsername = "Second Player";
        private readonly IFakePlayerProvider _playerProvider;

        public PlayerServiceSeedHelper(IFakePlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private async Task<Player> SeedPlayerAsync(string username = FirstPlayerUsername,
            CancellationToken token = default)
        {
            await _playerProvider.Authenticate(players => Task.FromResult(new Player
            {
                Username = username
            }));
            return await _playerProvider
                .AddAsync(token);
        }

        public async Task<Player> SeedAndLoginAsync(string username = FirstPlayerUsername,
            CancellationToken token = default) =>
            await LoginPlayerAsync((await SeedPlayerAsync(username, token)).Username, token);

        public async Task<Player> LoginPlayerAsync(string username = FirstPlayerUsername,
            CancellationToken token = default) =>
            await _playerProvider
                .Authenticate(async players => await players.SingleAsync(p => p.Username == username, token));
    }
}