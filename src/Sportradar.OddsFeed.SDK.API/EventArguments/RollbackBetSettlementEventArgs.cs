﻿/*
* Copyright (C) Sportradar AG. See LICENSE for full license governing this code
*/
using System;
using System.Collections.Generic;
using Dawn;
using System.Globalization;
using System.Linq;
using System.Text;
using Sportradar.OddsFeed.SDK.Entities;
using Sportradar.OddsFeed.SDK.Entities.Internal;
using Sportradar.OddsFeed.SDK.Entities.REST;
using Sportradar.OddsFeed.SDK.Messages.Feed;

namespace Sportradar.OddsFeed.SDK.API.EventArguments
{
    /// <summary>
    /// Event arguments for <see cref="IEntityDispatcher{T}.OnRollbackBetSettlement"/> class
    /// </summary>
    /// <typeparam name="T">A <see cref="ICompetition"/> derived instance specifying the type of sport event associated with contained <see cref="IRollbackBetSettlement{T}"/></typeparam>
    public class RollbackBetSettlementEventArgs<T> : EventArgs where T : ISportEvent
    {
        /// <summary>
        /// A <see cref="IFeedMessageMapper"/> used to map feed message to the one dispatched to the user
        /// </summary>
        private readonly IFeedMessageMapper _messageMapper;

        /// <summary>
        /// A <see cref="rollback_bet_settlement"/> message received from the feed
        /// </summary>
        private readonly rollback_bet_settlement _feedMessage;

        /// <summary>
        /// A <see cref="IEnumerable{CultureInfo}"/> specifying the default languages to which the received message is translated
        /// </summary>
        private readonly IEnumerable<CultureInfo> _defaultCultures;

        /// <summary>
        /// The raw message
        /// </summary>
        private readonly byte[] _rawMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddsChangeEventArgs{T}"/> class
        /// </summary>
        /// <param name="messageMapper">A <see cref="IFeedMessageMapper"/> used to map feed message to the one dispatched to the user</param>
        /// <param name="feedMessage">A <see cref="rollback_bet_settlement"/> message received from the feed</param>
        /// <param name="cultures">A <see cref="IEnumerable{CultureInfo}"/> specifying the default languages to which the received message is translated</param>
        /// <param name="rawMessage">A raw message received from the feed</param>
        internal RollbackBetSettlementEventArgs(IFeedMessageMapper messageMapper, rollback_bet_settlement feedMessage, IEnumerable<CultureInfo> cultures, byte[] rawMessage)
        {
            Guard.Argument(messageMapper, nameof(messageMapper)).NotNull();
            Guard.Argument(feedMessage, nameof(feedMessage)).NotNull();
            Guard.Argument(cultures, nameof(cultures)).NotNull();//.NotEmpty();
            if (!cultures.Any())
                throw new ArgumentOutOfRangeException(nameof(cultures));


            _messageMapper = messageMapper;
            _feedMessage = feedMessage;
            _defaultCultures = cultures as IReadOnlyCollection<CultureInfo>;
            _rawMessage = rawMessage;
        }

        /// <summary>
        /// Gets the <see cref="IRollbackBetSettlement{T}"/> implementation representing the received bet settlement rollback message translated to the specified languages
        /// </summary>
        /// <param name="culture">A <see cref="CultureInfo"/> specifying the language of which to translate the message or a null reference to translate the message
        /// to languages specified in the configuration</param>
        /// <returns>Returns the <see cref="IRollbackBetSettlement{T}"/> implementation representing the received bet settlement rollback message translated to the specified languages</returns>
        public IRollbackBetSettlement<T> GetBetSettlementRollback(CultureInfo culture = null)
        {
            return _messageMapper.MapRollbackBetSettlement<T>(
                _feedMessage,
                culture == null
                    ? _defaultCultures
                    : new[] { culture },
                _rawMessage);
        }

        /// <summary>
        /// Gets the raw xml message received from the feed
        /// </summary>
        /// <returns>Returns the raw xml message received from the feed</returns>
        [Obsolete("The message was moved to event")]
        public string GetRawMessage()
        {
            return _rawMessage == null ? null : Encoding.UTF8.GetString(_rawMessage);
        }
    }
}
