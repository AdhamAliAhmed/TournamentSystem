using System.Collections.Generic;
using TournamentSystem.Core;

namespace TournamentSystem.RankingSystem
{
    /// <summary>
    /// Provides a boolean property indicated whether the type is ready to be ranked or not
    /// </summary>
    public interface IRankable
    {
        /// <summary>
        /// Indicates whether the Type is ready to be ranked or not
        /// </summary>
        bool ReadyToRank { get; }
        /// <summary>
        /// The winner in the 1st place from the competition which the Rankable type made
        /// </summary>
        Challenger Winner { get; }
        /// <summary>
        /// The losers from the competition which the Rankable type made
        /// </summary>
        List<Challenger> Losers { get; }
    }
}
