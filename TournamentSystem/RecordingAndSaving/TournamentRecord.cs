using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentSystem.Core;
using TournamentSystem.RankingSystem;

namespace TournamentSystem.Saving
{
    /// <summary>
    /// Represents the record document for a specific tournament
    /// </summary>
    public class TourrnamentRecord
    {
        /// <summary>
        /// Represents the tournament which took place
        /// </summary>
        public readonly Tournament Tournament;

        /// <summary>
        /// Represents the date when the tournament took place
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Represents the number of the challengers joined the tournament
        /// </summary>
        public readonly int TournamentMembersCount;

        /// <summary>
        /// Represents the members joined the tournament
        /// </summary>
        public readonly Challenger[] Members;

        /// <summary>
        /// Represents the tournament winner
        /// </summary>
        public readonly Challenger Winner;

        /// <summary>
        /// Represents the challenger in the second place
        /// </summary>
        public readonly Challenger Second;

        /// <summary>
        /// Represents the challenger in the third place
        /// </summary>
        public readonly Challenger Third;

        /// <summary>
        /// Represents the challengers lost the tournament by losing 1st, 2nd, 3rd places
        /// </summary>
        public readonly Challenger[] Losers;

        /// <summary>
        /// Supplies the class with the needed information
        /// </summary>
        public TourrnamentRecord(Tournament finsishedTournament)
        {
            if (finsishedTournament == null)
                throw new Exceptions.RecordException("Cannot create a record for a null tournament");
            if (!finsishedTournament.ReadyToRank)
                throw new Exceptions.RecordException("Cannot create a record non-rank-ready tournament");

            Tournament = finsishedTournament;
            var ranker = new Ranker(Tournament);
            ranker.Rank();

            Date = Tournament.Date;
            Members = Tournament.Challengers.ToArray();

            Winner = ranker.First;
            Second = ranker.Second;
            Third = ranker.Third;
            Losers = ranker.Rest;
        }
    }
}
