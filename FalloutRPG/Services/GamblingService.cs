﻿using Discord;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class GamblingService
    {
        private List<ulong> _gamblingChannels;
        public readonly ObservableDictionary<IUser, long> UserBalances;

        private readonly IConfiguration _config;
        private readonly CharacterService _charService;

        public GamblingService(IConfiguration config, CharacterService charService)
        {
            UserBalances = new ObservableDictionary<IUser, long>();

            UserBalances.CollectionChanged += UserBalances_Changed;

            _config = config;
            _charService = charService;
            LoadGamblingEnabledChannels();
        }

        private async void UserBalances_Changed(object sender, EventArgs e)
        {
            try
            {
                await SaveUserBalancesAsync();
            }
            catch (Exception)
            {
                throw;
                //Console.WriteLine("some bo' 'schitt happened yo ");
            }
        }

        public bool AddUserBalance(IUser user)
        {
            var character = _charService.GetCharacter(user.Id);

            if (character == null)
                return false;

            if (UserBalances.ContainsKey(user))
                return false;

            UserBalances.Add(user, character.Money);
            return true;
        }

        public async Task SaveUserBalancesAsync()
        {
            Console.WriteLine("im running yaaaaaaaay!");
            foreach (var bal in UserBalances)
            {
                var character = _charService.GetCharacter(bal.Key.Id);
                if (character.Money != bal.Value) // only save if money has changed
                {
                    Console.WriteLine("saving value!");
                    character.Money = bal.Value;
                    await _charService.SaveCharacterAsync(character);
                }
            }
        }

        private void LoadGamblingEnabledChannels()
        {
            try
            {
                _gamblingChannels =
                    _config
                    .GetSection("gambling:enabled-channels")
                    .GetChildren()
                    .Select(x => UInt64.Parse(x.Value))
                    .ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("You have not specified any gambling enabled channels in Config.json");
            }
        }
    }
}
