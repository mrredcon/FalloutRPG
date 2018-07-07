using Discord;
using FalloutRPG.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalloutRPG.Services
{
    public class CrapsService
    {
        private readonly GamblingService _gamblingService;

        private readonly Random _rand;

        private List<IUser> _players;
        private List<Bet> _bets;

        private IUser _shooter;
        private int _shooterIndex;

        private Round _round;
        private int _shooterPoint;

        public CrapsService(GamblingService gamblingService)
        {
            _players = new List<IUser>();
            _bets = new List<Bet>();

            _round = Round.ComeOut;
            _shooterPoint = -1;

            _shooterIndex = 0;

            _gamblingService = gamblingService;

            _rand = new Random();
        }

        public string Roll(IUser user)
        {
            if (user != _shooter)
                return String.Format(Messages.ERR_CRAPS_NOT_SHOOTER, user.Mention);
            if (_bets.Find(x => x.User.Equals(user)) == null)
                return String.Format(Messages.ERR_CRAPS_BET_NOT_SET, user.Mention);

            string result = "";

            int dice1 = _rand.Next(1, 7),
                dice2 = _rand.Next(1, 7),
                sum = dice1 + dice2;

            result = DiceToEmoji(dice1) + " " + DiceToEmoji(dice2) + "\n";

            return result + AdvanceRound(sum);
        }

        public string PlaceBet(IUser user, string betToPlace, int betAmount)
        {
            if (_bets.Find(x => x.User.Equals(user)) != null) // bet already placed
                return String.Format(Messages.ERR_CRAPS_BET_ALREADY_SET, user.Mention);

            if (!_gamblingService.UserBalances.ContainsKey(user)) // can't find user in dictionary
                if (!_gamblingService.AddUserBalance(user)) // failed to add user
                    return String.Format(Messages.ERR_BALANCE_ADD_FAIL, user.Mention);

            if (betAmount > _gamblingService.UserBalances[user])
                //return String.Format(Messages.ERR_BET_TOO_HIGH, user.Mention);

            if (betAmount <= 0)
                return String.Format(Messages.ERR_BET_TOO_LOW, user.Mention);

            betToPlace = betToPlace.ToLower();

            BetType betType = BetType.Error;

            switch (betToPlace)
            {
                case "pass":
                    betType = BetType.Pass;
                    break;
                case "dontpass":
                    betType = BetType.DontPass;
                    break;
                case "come":
                    betType = BetType.Come;
                    break;
                case "dontcome":
                    betType = BetType.DontCome;
                    break;
            }

            if (betType != BetType.Error)
            {
                if (_round == Round.ComeOut)
                {
                    // can't place a Come bet during the come out round
                    if (betType == BetType.Come || betType == BetType.DontCome)
                        return String.Format(Messages.ERR_CRAPS_POINT_NOT_SET, user.Mention);
                }
                else if (_round == Round.Point)
                {
                    // can't place a Pass bet during the point round
                    if (betType == BetType.Pass || betType == BetType.DontPass)
                        return String.Format(Messages.ERR_CRAPS_POINT_ALREADY_SET, user.Mention);
                }

                _bets.Add(new Bet(user, betAmount, betType));

                return String.Format(Messages.BET_PLACED, user.Mention);
            }
            else
            {
                // failed to parse bet type
                return String.Format(Messages.ERR_CRAPS_BET_PARSE_FAIL, user.Mention);
            }
        }

        private string AdvanceRound(int roll)
        {
            string result = "";

            result += AwardPassBets(roll);
            result += AwardComeBets(roll);
            return result;
        }

        public bool JoinMatch(IUser user)
        {
            if (!_gamblingService.AddUserBalance(user))
            {
                return false;
            }
            _players.Add(user);

            if (_players.Count == 1)
                _shooter = user;

            return true;
        }

        public bool LeaveMatch(IUser user)
        {
            if (user == _shooter)
                return false;

            _players.Remove(user);

            return true;
        }

        public bool NextShooter()
        {
            IUser oldShooter = _shooter;

            if (_players.Count > _shooterIndex + 1)
                _shooter = _players[++_shooterIndex];
            else
                _shooterIndex = 0;

            if (_shooter != oldShooter)
                return true;
            else
                return false;
        }

        private string AwardPassBets(int roll)
        {
            string result = "";

            BetType winningBet = BetType.Error;

            if (_round == Round.ComeOut)
            {
                if (roll == 2 || roll == 3 || roll == 12) // crap out
                {
                    winningBet = BetType.DontPass;
                    result += String.Format(Messages.CRAPS_CRAPOUT, _shooter.Mention) + "\n";
                }
                else if (roll == 7 || roll == 11) // natural
                {
                    winningBet = BetType.Pass;
                    result += String.Format(Messages.CRAPS_NATURAL, _shooter.Mention) + "\n";
                }
                else // point
                {
                    _shooterPoint = roll;
                    _round = Round.Point;
                    result += String.Format(Messages.CRAPS_POINT_ROUND, _shooter.Mention) + "\n";
                }
            }
            else if (_round == Round.Point)
            {
                if (roll == 7) // seven out
                {
                    winningBet = BetType.DontPass;

                    result += String.Format(Messages.CRAPS_SEVEN_OUT, _shooter.Mention) + "\n";
                    _round = Round.ComeOut;
                    NextShooter();
                    result += String.Format(Messages.CRAPS_NEW_SHOOTER, _shooter.Mention, roll) + "\n";
                }
                else if (roll == _shooterPoint) // shooter reached point
                {
                    winningBet = BetType.Pass;

                    _round = Round.ComeOut;
                    result += String.Format(Messages.CRAPS_POINT_ROLL, _shooter.Mention) + "\n";
                }
            }
            BetType losingBet = BetType.Error;

            if (winningBet == BetType.Pass)
                losingBet = BetType.DontPass;
            else if (winningBet == BetType.DontPass)
                losingBet = BetType.Pass;

            foreach (var bet in _bets.ToList())
            {
                if (bet.BetType == winningBet)
                    AwardBet(bet);
                else if (bet.BetType == losingBet)
                    AwardBet(bet, false);
            }
            return result;
        }

        private string AwardComeBets(int roll)
        {
            string result = "";

            foreach (var bet in _bets.ToList())
            {
                if (bet.BetType == BetType.Come)
                {
                    if (roll == bet.Point) // come point achieved (win bet)
                    {
                        AwardBet(bet);
                        result += String.Format(Messages.CRAPS_POINT_ROLL, bet.User.Mention) + "\n";
                    }
                    else if (bet.Point == -1) // "come out" roll for new Come Bet
                    {
                        if (roll == 7 || roll == 11) // natural
                        {
                            AwardBet(bet);
                            result += String.Format(Messages.CRAPS_NATURAL, bet.User.Mention) + "\n";
                        }
                        else if (roll == 2 || roll == 3 || roll == 12) // crap out
                        {
                            AwardBet(bet, false);
                            result += String.Format(Messages.CRAPS_CRAPOUT, bet.User.Mention) + "\n";
                        }
                        else // point
                        {
                            bet.Point = roll;
                            result += String.Format(Messages.CRAPS_POINT_SET, bet.User.Mention, roll) + "\n";
                        }
                    }
                }
                else if (bet.BetType == BetType.DontCome)
                {
                    if (roll == bet.Point) // point achieved
                    {
                        AwardBet(bet, false);
                        result += String.Format(Messages.CRAPS_POINT_ROLL_NEG, bet.User.Mention, roll) + "\n";
                    }
                    else if (bet.Point != -1 && roll == 7) // seven out
                    {
                        AwardBet(bet);
                        result += String.Format(Messages.CRAPS_SEVEN_OUT_POS, bet.User.Mention, roll) + "\n";
                    }
                    else if (bet.Point == -1) // come out roll
                    {
                        if (roll == 2 || roll == 3) // crap out
                        {
                            AwardBet(bet);
                            result += String.Format(Messages.CRAPS_CRAPOUT_POS, bet.User.Mention, roll) + "\n";
                        }
                        else if (roll == 12) // crap out push (house always wins)
                        {
                            AwardBet(bet, false, true);
                            result += String.Format(Messages.CRAPS_CRAPOUT_PUSH, bet.User.Mention, roll) + "\n";
                        }
                        else if (roll == 7 || roll == 11) // natural
                        {
                            AwardBet(bet, false);
                            result += String.Format(Messages.CRAPS_NATURAL_NEG, bet.User.Mention, roll) + "\n";
                        }
                        else
                        {
                            bet.Point = roll; // point
                            result += String.Format(Messages.CRAPS_POINT_SET, bet.User.Mention, roll) + "\n";
                        }
                    }
                }
            }

            return result;
        }

        private void AwardBet(Bet bet, bool win = true, bool push = false)
        {
            var balances = _gamblingService.UserBalances;
            if (win)
            {
                balances[bet.User] += bet.BetAmount;
                _bets.Remove(bet);
            }
            else
            {
                balances[bet.User] -= bet.BetAmount;
                _bets.Remove(bet);
            }
        }

        private string DiceToEmoji(int num)
        {
            switch (num)
            {
                case 1:
                    return ":one:";
                case 2:
                    return ":two:";
                case 3:
                    return ":three:";
                case 4:
                    return ":four:";
                case 5:
                    return ":five:";
                case 6:
                    return ":six:";
            }
            return null;
        }
        private enum Round
        {
            ComeOut,
            Point
        }
        private enum RoundResult
        {
            CrapOut,
            Natural,
            SevenOut
        }
        public enum BetType
        {
            Error,
            Pass,
            DontPass,
            Come,
            DontCome
        }
        private class Bet
        {
            public IUser User { get; }
            public int BetAmount { get; }
            public BetType BetType { get; }
            public int Point { get; set; }

            public Bet(IUser user, int betAmount, BetType betType)
            {
                User = user;
                BetAmount = betAmount;
                BetType = betType;
                Point = -1;
            }
        }
    }
}
