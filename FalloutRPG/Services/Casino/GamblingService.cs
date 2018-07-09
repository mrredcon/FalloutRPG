using Discord;
using FalloutRPG.Services.Roleplay;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Casino
{
    public class GamblingService
    {
        private List<ulong> _gamblingChannels;
        public readonly ObservableDictionary<IUser, long> UserBalances;

        private readonly IConfiguration _config;
        private readonly CharacterService _charService;

        public readonly long MINIMUM_BET;
        public readonly long MAXIMUM_BET;

        public GamblingService(IConfiguration config, CharacterService charService)
        {
            UserBalances = new ObservableDictionary<IUser, long>();

            UserBalances.CollectionChangedAsync += UserBalances_CollectionChanged;

            _config = config;

            MINIMUM_BET = long.Parse(_config["gambling:minimum-bet"]);
            MAXIMUM_BET = long.Parse(_config["gambling:maximum-bet"]);

            _charService = charService;
            LoadGamblingEnabledChannels();
        }

        private async Task UserBalances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                // should only be one item in NewItems
                foreach (var newItem in e.NewItems)
                {
                    var keyValue = (KeyValuePair<IUser, long>)newItem;

                    var user = keyValue.Key;
                    var newMoney = keyValue.Value;

                    var character = await _charService.GetCharacterAsync(user.Id);

                    character.Money = newMoney;

                    await _charService.SaveCharacterAsync(character);
                }
            }
        }

        public bool IsGamblingEnabledChannel(ulong channelId)
        {
            if (_gamblingChannels.Contains(channelId))
                return true;
            return false;
        }

        /// <summary>
        /// This will create an entry in UserBalances with the specified user, and their balance
        /// </summary>
        /// <param name="user">The user to add their balance to UserBalances</param>
        /// <returns>A boolean stating whether the user's balance was added or not.</returns>
        public async Task<AddUserBalanceResult> AddUserBalanceAsync(IUser user)
        {
            var character = await _charService.GetCharacterAsync(user.Id);

            if (character == null)
                return AddUserBalanceResult.NullCharacter;

            if (UserBalances.ContainsKey(user))
                return AddUserBalanceResult.AlreadyInDictionary;

            UserBalances.Add(user, character.Money);
            return AddUserBalanceResult.Success;
        }

        public enum AddUserBalanceResult
        {
            Success,
            AlreadyInDictionary,
            NullCharacter,
            UnknownError
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
                _gamblingChannels = new List<ulong>();
            }
        }
    }
}
