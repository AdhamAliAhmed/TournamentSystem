using System.Collections.Generic;
using TournamentSystem.RankingSystem;

namespace TournamentSystem.Core
{
    /// <summary>
    /// Defines extra methods for grouping types such as Tournament and Group
    /// </summary>
    public interface IGrouping : ICompetition
    {
        /// <summary>
        /// The Challengers of the competition
        /// </summary>
        List<Challenger> Challengers { get; }
        /// <summary>
        /// Matches played during the group/tournament 
        /// </summary>
        List<MatchRecord> Matches { get;}


    }
}
