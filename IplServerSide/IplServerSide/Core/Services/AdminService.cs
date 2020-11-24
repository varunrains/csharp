using IplServerSide.Core.Repositories;
using IplServerSide.Dtos;
using IplServerSide.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebPush;

namespace IplServerSide.Core.Services
{
    public class AdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BettingContext _bettingContext;
        private const string Public_Key = @"BDk-QSoBtl9BNgY5mGVzP9iiUtAdQFPsnUl3VwnZz05zO54OsS_d8yiBxrdubhd236AWcx0E2E0JFfVo1t-sc0E";
        private const string Private_Key = @"LaUAkhjAM7G16wNevX0CJTQPzpLFrI_jR0mYpK9IXeI";
        public AdminService(IUnitOfWork unitOfWork, BettingContext bettingContext)
        {
            _unitOfWork = unitOfWork;
            _bettingContext = bettingContext;
        }

        public void UpdateResult(MatchDto matchDetails)
        {
            var bets = _unitOfWork.Bets.Find(bet => bet.MatchId == matchDetails.MatchId).ToList();
            if (matchDetails.IsMatchAbandoned)
            {
                UpdateResultForAbandonedMatch(bets);
                return;
            }
            UpdateMatchResult(matchDetails);
            UpdateBettingDetails(bets, matchDetails);
            UpdateUsersWallet(bets);

            _unitOfWork.Complete();
        }

        public void RemoveAllBets()
        {
            return;
            //var bets = _unitOfWork.Bets.GetAll();
            //_unitOfWork.Bets.RemoveRange(bets);
            //_unitOfWork.Complete();
        }

        public void SendNotifications(NotificationChild notificationInformation)
        {

            var users = _unitOfWork.Users.GetAll().ToDictionary(x => x.UserId, y=> y.DisplayName);
            var userNotifications = _bettingContext.UserNotifications.ToList();
            var subject = @"mailto:example@example.com";
            var webPushClient = new WebPushClient();
            userNotifications.ForEach(userNotification =>
            {

                var subscriptionDetails = JsonConvert.DeserializeObject<SubscriptionObject>(userNotification.NotificationObject);
                var endPoint = subscriptionDetails.endpoint;
                var p256dh = subscriptionDetails.keys.p256dh;
                var auth = subscriptionDetails.keys.auth;

                if (string.IsNullOrWhiteSpace(endPoint) || string.IsNullOrWhiteSpace(p256dh) || string.IsNullOrWhiteSpace(auth)) return;
                var subscription = new PushSubscription(endPoint, p256dh, auth);
                var vapidDetails = new VapidDetails(subject, Public_Key, Private_Key);

                var notificationDetails = GetNotificationDetails(notificationInformation, userNotification.UserId, users);
                try
                {
                    webPushClient.SendNotification(subscription, JsonConvert.SerializeObject(notificationDetails), vapidDetails);
                }
                catch (WebPushException exception)
                {
                    Console.WriteLine("Http STATUS code" + exception.StatusCode);
                }
            });
        }

        private Notification GetNotificationDetails(NotificationChild notificationChild, int userId, Dictionary<int,string> userDetails)
        {
            return new Notification()
            {
                notification = new NotificationChild()
                {
                    title = string.IsNullOrWhiteSpace(notificationChild.title) ? $"Hi {userDetails.First(x => x.Key == userId).Value}!!": notificationChild.title,
                    body = string.IsNullOrWhiteSpace(notificationChild.body) ? "Start betting!!": notificationChild.body,
                    icon = string.IsNullOrWhiteSpace(notificationChild.icon) ? "/assets/icons/icon-96x96.png" : $"/assets/icons/{notificationChild.icon}.png",
                    tag = "iplbet",
                    badge= "/assets/icons/icon-96x96.png",
                    lang="en-US",
                    renotify = true,
                    vibrate = new int[] { 100, 50, 100 }
                }
            };
        }

        #region Private methods
        private void UpdateResultForAbandonedMatch(List<Bet> bets)
        {
            bets.ForEach(bet =>
            { 
                bet.NetAmountWon = bet.BetAmount;
            });
            UpdateUsersWallet(bets);
            _unitOfWork.Complete();
        }
    

        private void UpdateMatchResult(MatchDto matchDetails)
        {
            var match = _unitOfWork.Matches.Get(matchDetails.MatchId);
            match.Result = matchDetails.Result;
        }

        private void UpdateBettingDetails(List<Bet> bets, MatchDto matchDetails)
        {
            var totalBetAmountOfMatch = (decimal) bets.Sum(bet => bet.BetAmount);
            var winnersAmount = (decimal)bets.Where(bet => bet.BettingTeamId == matchDetails.Result).Sum(bet => bet.BetAmount);
            var losersAmount = totalBetAmountOfMatch - winnersAmount;
            bets.ForEach(bet =>
            {
                bet.WinningTeamId = matchDetails.Result;
                if (bet.WinningTeamId == bet.BettingTeamId)
                {
                    bet.NetAmountWon = bet.BetAmount / winnersAmount * losersAmount;
                }
                else
                {
                    bet.NetAmountWon = -1 * bet.BetAmount;
                }

            });
        }

        private void UpdateUsersWallet(List<Bet> bets)
        {
            var bettingUserIds = bets.Select(x => x.UserId);
            var users = _unitOfWork.Users.Find(user => bettingUserIds.Contains(user.UserId)).ToList();
            users.ForEach(user =>
            {
                var amt = bets.FirstOrDefault(x => x.UserId == user.UserId && x.NetAmountWon >= 0);
                if (amt != null)
                    user.UserAmount += amt.BetAmount + amt.NetAmountWon.GetValueOrDefault();
            });

        }
        #endregion



    }
}