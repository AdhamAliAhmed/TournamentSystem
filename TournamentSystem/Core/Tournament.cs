using System;
using System.Collections.Generic;
using System.Linq;
using TournamentSystem.RankingSystem;

namespace TournamentSystem.Core
{
    /// <summary>
    /// Creates a competition between a punch of challengers and determines the winner challenger among punch of players according to certin rules.
    /// and it the determines the losers as well.
    /// </summary>
    /// <seealso cref="TournamentSystem.Core.Challenger" />
    /// <seealso cref="TournamentSystem.Core.Match" />
    /// <seealso cref="TournamentSystem.Core.Group"/>
    /// <remarks>
    /// Makes a competition between the given challengers, Following the Knockout-once system and the Retention system as well. Meaning that if we considered 
    /// we have 6 challengers to join the group and challenge each other. Every challenger will find an opponent causing 3 winners and 3 losers. 
    /// We cannot create a regular competition. So, following the Retention system we round up to the next power 
    /// of 2 which is 4. Then we subtract the challengers number from the next power of 2. The result is how many challengers are going to remain to the next round.
    /// After we calculated the remaining challengers, we have a pure regular number for a regular competition(power of 2). We create a competition between the
    /// pure number, we take the winners from the first round and add the remaining challengers to them. Now we have a pure regular number again. Now we create
    /// and start a regular competition which will cause one winner and a punch of losers
    /// </remarks>
    /// <example>
    /// <code>
    /// //Array of challengers to join the group
    /// var challengers = new Challenger[]
    ///     { new Challenger{Name = "Adham"},
    ///       new Challenger{Name = "Jhon"},
    ///       new Challenger{Name = "Jane"},
    ///       new Challenger{Name = "Doe"},
    ///       new Challenger{Name = "Adhan"},
    ///       new Challenger{Name = "Jared"},
    ///       new Challenger{Name = "Hugh"},
    ///     };
    /// //Creating an instance of the Group class providing the challengers array we just created
    /// var group = new Group(challengers);
    /// //Starting the group by calling the Start method
    /// group.Start();
    /// Console.WriteLine(group.Winner.Name);
    /// </code>
    /// </example>
    [Serializable]
    public sealed class Tournament : IGrouping, IRankable, IEquatable<Tournament>
    {

        #region Events
        /// <summary>
        /// Occurs when the tournament is started.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Occurs when the tournament is finsished.
        /// </summary>
        public event EventHandler Finished;

        #region Event firing methods
        /// <summary>
        /// Fires "Started" event if it's not null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The EventArgs object which provides to subscriber with important info</param>
        public void OnStarted(object sender, EventArgs e) => Started?.Invoke(sender, e);
        /// <summary>
        /// Fires "Finished" event if it's not null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The EventArgs object which provides to subscriber with important info</param>
        public void OnFinished(object sender, EventArgs e) => Finished?.Invoke(sender, e);
        #endregion

        #endregion

        #region Public properties
        /// <summary>
        /// The Challengers of the tournament
        /// </summary>
        public List<Challenger> Challengers { get; }

        /// <summary>
        /// Matches played during the tournament
        /// </summary>
        public List<MatchRecord> Matches { get; private set; } = new List<MatchRecord>();

        /// <summary>
        /// The first group which contains about the first half of the challengers.
        /// </summary>
        public Group Group1 { get; private set; }
        /// <summary>
        /// The second group which contains about the second half of the challengers.
        /// </summary>
        public Group Group2 { get; private set; }

        /// <summary>
        /// Indicates whether the tournament is ready to be ranked or not.
        /// The tournament becomes ready to be ranked when it's finished
        /// </summary>
        public bool ReadyToRank { get; private set; }

        /// <summary>
        /// The winner of the tournament
        /// </summary>
        public Challenger Winner { get; private set; }

        /// <summary>
        /// A list contains the losers of the tournament
        /// </summary>
        public List<Challenger> Losers { get; private set; }

        /// <summary>
        /// Re-presets the date when the tournament took place
        /// </summary>
        public DateTime Date { get; }

        #endregion

        #region Private fields

        List<Challenger> _challengers;

        Match _finalMatch;

        #endregion

        #region Constructors
        /// <summary>
        /// Takes an list of challengers and divides it into two equal group of challengers(if possible)
        /// </summary>
        /// <param name="challengers">The challengers which are going to join the tournament</param>
        public Tournament(List<Challenger> challengers)
        {
            if (challengers == null || challengers.Count < 2)
                throw new ArgumentException("The challenger array cannot be null or less than two", "challengers");

            Challengers = challengers;
            _challengers = new List<Challenger>(Challengers);

            var tempCount = _challengers.Count;

            /*If the challengers are odd: we take get the half and round it to the next even and this is the Group1. 
             * The rest is group2*/

            if (_challengers.Count % 2 != 0)
                tempCount = _challengers.Count + 1;

            var firstGroup = tempCount / 2;

            Group1 = new Group(_challengers.Take(firstGroup).ToList(),"A");
            Group2 = new Group(_challengers.Skip(firstGroup).ToList(),"B");

            Losers = new List<Challenger>();
            Date = DateTime.Now;

            Finished += Tournament_Finished;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the tournament
        /// </summary>
        public void Start()
        {
            OnStarted(this, null);

            Eliminate();

            ManageTounamentFinalMatch();

            AddLosers(_finalMatch);

            Matches = Enumerable.Concat(Group1.Matches, Group2.Matches).ToList();

            OnFinished(this, null);
        }

        /// <summary>
        /// Eliminate tournament challengers by eliminating "Group1" challengers and "Group2" challengers.
        /// When "Group" and "Group2" is eliminated; only 2 challengers are left. Each from each group.
        /// Then one of the two challengers win the tournament
        /// </summary>
        private void Eliminate()
        {
            Group1.Start();
            Group2.Start();
        }

        /// <summary>
        /// Creates and manages the last match in the tournament between the two challengers left from each group.
        /// </summary>
        private void ManageTounamentFinalMatch()
        {
            Challenger group1Winner = Group1.Winner;
            Challenger group2Winner = Group2.Winner;
            Match finalMatch = new Match(group1Winner, group2Winner);
            finalMatch.Start();
            Winner = finalMatch.Winner;
            _finalMatch = finalMatch;
        }

        /// <summary>
        /// Combines the losers from the whole tournament into one collection
        /// </summary>
        /// <param name="finalMatch"></param>
        private void AddLosers(Match finalMatch)
        {
            Losers.AddRange(Group1.Losers);
            Losers.AddRange(Group2.Losers);
            Losers.Add(finalMatch.Loser);
        }

        /// <summary>
        /// returns a bool indicating whether the current instance is equal to the other instance(the tournament argument provided)
        /// </summary>
        /// <param name="other">instance to test equality</param>
        /// <returns>bool</returns>
        public bool Equals(Tournament other) => this == other;


        /// <summary>
        /// Announces that the tournament is finished and ready to be ranked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tournament_Finished(object sender, EventArgs e)
        {
            ReadyToRank = true;
        }

        #region Object derived methods
        /// <summary>
        /// returns a bool indicating whether the current instance is equal to the other instance(the Object argument provided)
        /// </summary>
        /// <param name="obj">instance to test equality</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Tournament;
            if (obj == null) return false;
            return Equals(other: other);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => Challengers.Count.ToString();

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode() => base.GetHashCode();
        #endregion

        #endregion
    }
}
