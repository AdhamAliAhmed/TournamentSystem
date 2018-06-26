using System.Collections.Generic;

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
    }
}
