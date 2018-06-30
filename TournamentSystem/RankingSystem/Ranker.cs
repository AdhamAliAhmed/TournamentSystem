using System;
using System.Collections.Generic;
using System.Linq;
using TournamentSystem.Core;

namespace TournamentSystem.RankingSystem
{
    /// <summary>
    /// Provides a method to rank a challengers to 1st, 2nd, 3rd place
    /// </summary>
    public sealed class Ranker
    {
        /// <summary>
        /// Constructors initializes the Tournament read-only field and takes one argument
        /// </summary>
        /// <param name="rankable">A tournament to rank</param>
        public Ranker(IRankable rankable)
        {
            if (rankable == null | !rankable.ReadyToRank)
                throw new RankException();

            Rankable = rankable;
        }

        #region Properties
        /// <summary>
        /// Tournament passed to rank
        /// </summary>
        public readonly IRankable Rankable;

        /// <summary>
        /// The tournament winner in place 1
        /// </summary>
        public Challenger First { get; private set; }

        /// <summary>
        /// The challenger in place 2
        /// </summary>
        public Challenger Second { get; private set; }

        /// <summary>
        /// The challenger in place 3
        /// </summary>
        public Challenger Third { get; private set; }

        /// <summary>
        /// The losers of the tournament
        /// </summary>
        public Challenger[] Losers { get; private set; }

        /// <summary>
        /// Rank the Tournament property called Tournament to 1st place, 2nd place, 3rd place And the rest of the challengers which are losers
        /// </summary>
        #endregion

        public readonly List<MatchRecord> Matches = new List<MatchRecord>();
        public void Rank()
        {
            First = Rankable.Winner;
            Second = Rankable.Losers[0];
            Third = Rankable.Losers[1];
            Losers = Rankable.Losers.Skip(2).ToArray();
        }
    }

    public class MatchRecord
    {
        public Challenger Challenger1 { get; set; }
        public Challenger Challenger2 { get; set; }
        public Group ContainingGroup { get; set; }
        public int Round { get; set; }

        public MatchRecord(Challenger challenger1, Challenger challenger2, Group containingGroup)
        {
            if (challenger1 is null | challenger2 is null | containingGroup is null)
                throw new ArgumentNullException("Neither of challenger1, challenger2 or containingGroup can be null");
            Challenger1 = challenger1;
            Challenger2 = challenger2;
            ContainingGroup = containingGroup;
            Round = containingGroup.CurrentRound;
        }
    }

    /// <summary>
    /// Thrown when attempting to rank a tournament that is not ready to be ranked.
    /// </summary>
    public class RankException : Exception { }
}
