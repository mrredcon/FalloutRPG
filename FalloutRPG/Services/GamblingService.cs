using Discord;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
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

            UserBalances.CollectionChangedAsync += UserBalances_CollectionChanged;

            _config = config;
            _charService = charService;
            LoadGamblingEnabledChannels();
        }

        private async Task UserBalances_CollectionChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Event firing!");

            foreach (var bal in UserBalances)
            {
                Console.WriteLine("Starting loop!");

                var user = bal.Key;
                var newMoney = bal.Value;

                var character = await _charService.GetCharacterAsync(user.Id);
                if (character.Money != newMoney)
                {
                    character.Money = newMoney;
                    await _charService.SaveCharacterAsync(character);
                }
                Console.WriteLine("Ending loop!");
                //                    await Task.Delay(2000);
            }
            //throw new NotImplementedException();
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
                return AddUserBalanceResult.AlreadyIn;

            UserBalances.Add(user, character.Money);
            return AddUserBalanceResult.Success;
        }

        public enum AddUserBalanceResult
        {
            Success,
            AlreadyIn,
            NullCharacter,
            UnknownError
        }

        /// <summary>
        /// Saves every user's changed balance in UserBalances into the database.
        /// </summary>
        public async Task SaveUserBalancesAsync()
        {
            foreach (var bal in UserBalances)
            {
                var character = _charService.GetCharacter(bal.Key.Id);
                if (character.Money != bal.Value) // only save if money has changed
                {
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
                _gamblingChannels = new List<ulong>();
            }
        }
    }
}
