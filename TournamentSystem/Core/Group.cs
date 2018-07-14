using System;
using System.Collections.Generic;
using System.Linq;
using TournamentSystem.Others;
using TournamentSystem.RankingSystem;

namespace TournamentSystem.Core
{
    /// <summary>
    /// Creates a competition between a punch of challengers and determines the winner challenger among punch of players. 
    /// It and the determines the losers as well.
    /// It's the small part of the tournament which joins another part to create the full tournament
    /// </summary>
    /// <seealso cref="TournamentSystem.Core.Challenger" />
    /// <seealso cref="TournamentSystem.Core.Match" />
    /// <seealso cref="TournamentSystem.Core.Tournament"/>
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
    ///       new Challenger{Name = "Ali"},
    ///       new Challenger{Name = "Bruce"},
    ///       new Challenger{Name = "Marshall"},
    ///       new Challenger{Name = "Harvey"},
    ///       new Challenger{Name = "Jared"},
    ///       new Challenger{Name = "Harley"},
    ///     };
    ///       
    /// //Creating an instance of the Group class providing the challengers array we just created
    /// var group = new Group(challengers);
    /// //Starting the group by calling the Start method
    /// group.Start();
    /// Console.WriteLine(group.Winner.Name);
    /// </code>
    /// </example>
    public sealed class Group : IGrouping, IRankable, IEquatable<Group>
    { 

        #region Events
        /// <summary>
        /// Occurs when the group starts to run.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Occurs when the group is finished.
        /// </summary>
        public event EventHandler Finished;
        /// <summary>
        /// Occurs when the current round is finished and another one is about to begin.
        /// </summary>
        public event EventHandler CurrentRoundLeveledUp;
        #endregion

        #region Event firing methods
        /// <summary>
        /// Fires Started event if it's not null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The EventArgs object which provides to subscriber with important info</param>
        public void OnStarted(object sender, EventArgs e) => Started?.Invoke(this, null);
        /// <summary>
        /// Fires Finished event if it's not null
        /// </summary>
        /// <param name="source">The sender of the event</param>
        /// <param name="e">The EventArgs object which provides to subscriber with important info</param>
        public void OnFinished(object source, EventArgs e) => Finished?.Invoke(this, null);

        /// <summary>
        /// Fires CurrentRowndLeveledUp event if it's not null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The EventArgs object which provides to subscriber with important info</param>
        public void OnCurrentRoundLeveledUp(object sender, EventArgs e) => CurrentRoundLeveledUp?.Invoke(sender, e);
        #endregion

        #region Back-end private fields
        List<Challenger> _challengers;

        int? _exitRound;

        List<Challenger> _challengersTemp;

        private bool _exit;
        #endregion

        #region Readonly fields
        /// <summary>
        /// Group label. Used for identification. Must be unique among other groups
        /// </summary>
        public readonly string GroupLabel;
        #endregion

        #region Front-end public properties

        /// <summary>
        /// Used by MatchesRecord
        /// </summary>
        public int CurrentRound { get; private set; }

        /// <summary>
        /// indicates whether to shuffle the group before the tournament starts
        /// </summary>
        public bool Shuffle { get; private set; }

        /// <summary>
        /// Matches played during the group 
        /// </summary>
        public List<MatchRecord> Matches { get; private set; } = new List<MatchRecord>();

        /// <summary>
        /// The challengers passed to the group
        /// and the winners from each round later on
        /// </summary>
        public List<Challenger> Challengers { get; }

        /// <summary>
        /// The remaining players if the group number or the winners from some round number is odd
        /// </summary>
        public List<Challenger> Remaining { get; set; }

        /// <summary>
        /// Represents the winner
        /// </summary>
        public Challenger Winner { get; private set; }

        /// <summary>
        /// Represents the losers
        /// </summary>
        public List<Challenger> Losers { get; private set; }
        /// <summary>
        /// Indicates whether the current instance is ready to ranked or not
        /// </summary>
        public bool ReadyToRank { get; set; }

        #endregion

        #region Over-loadable conversions
        /// <summary>
        /// Converts a Challenger array into Group implicitly.
        /// </summary>
        /// <param name="challengers">Challenger array to convert</param>
        public static implicit operator Group(List<Challenger> challengers) => new Group(challengers, null,false);
        #endregion

        #region Constructor/s

        /// <summary>
        /// Initialized a new instance of Group with an Array of challengers and null-able integer indicates in which round the process should be killed
        /// </summary>
        /// <param name="challengers">Challengers to join the tournament</param>
        /// <param name="exitRound">Indicates in which round the process should be killed</param>
        public Group(List<Challenger> challengers, string groupLabel, bool isShuffled, int? exitRound = null)
        {
            if (challengers == null || challengers.Count < 1)
                throw new ArgumentNullException(nameof(challengers), "Challengers list CANNOT be null or less than one");

            //Assigning _challengers to challengers after converting it from an array to a List of Challengers explicitly.
            Challengers = challengers;
            _challengers = new List<Challenger>(Challengers);

            if (_exitRound <= 0)
                throw new ArgumentOutOfRangeException(nameof(exitRound), "Exit round cannot be less tan or equal to 0");
            _exitRound = exitRound;

            GroupLabel = groupLabel;

            Shuffle = isShuffled;
            //initializing the used collections
            _challengersTemp = new List<Challenger>();
            Losers = new List<Challenger>();
            Remaining = new List<Challenger>();
            //subscribing to the CurrentRoundLeveledUp event
            CurrentRoundLeveledUp += Group_CurrentRoundLeveledUp;

            Started += Group_Started;
            Finished += Group_Finished;
        }


        #endregion

        #region Methods
        /// <summary>
        /// returns a bool indicating whether the current instance is equal to the other instance(the Group argument provided)
        /// </summary>
        /// <param name="other">instance to test equality</param>
        /// <returns>bool</returns>
        public bool Equals(Group other) => this == other;
        /// <summary>
        /// Determines the winner player among punch of players and the losers as well. The winner is going to challenge the winner from the second group in order to raise a winner
        /// </summary>

        /// <summary>
        /// Occurs when the group takes place
        /// </summary>
        private void Group_Started(object sender, EventArgs e)
        {
            if (Shuffle)
                HelperMethods.Shuffle(_challengers);
        }

        public void Start()
        {
            //firing an event indicating that the group is started
            OnStarted(this, null);

            IndicateGroupType();

            //firing an event indicating that the group is finished
            OnFinished(this, null);
        }
        private void IndicateGroupType()
        {
            var isRegularGroup = Challengers.Count.IsFromThePowerOfTwo();
            if (isRegularGroup)
            {
                //Managing regular groups
                ManageRegularGroup();
            }
            else
            {
                ManageIrregularGroup();
            }
        }

        #region Regular group
        private void ManageRegularGroup()
        {
            if (_challengers.Count == 1)
                Winner= _challengers.First();

            //Looping through the challengers until they are one. (the "winner")
            while (_challengers.Count != 1)
            {
                //looping through the Challengers list taking every two challengers, putting them in a match and 
                //remove them from the '_challengers' list then add the winner to the '_challengersTemp' list.
                for (int i = 0; i < _challengers.Count; i += 2)
                {
                    //create an object from the Match that will contain the player in the index 'i' with the player in the index 'i+1'.
                    Match match = new Match(_challengers[i], _challengers[i + 1]);
                    Matches.Add(new MatchRecord(_challengers[i], _challengers[i + 1], this));
                    //Starting the match which will end up with a winner and a loser.
                    match.Start();
                    //Adding the winner to the '_challengersTemp' List.
                    _challengersTemp.Add(match.Winner);
                    //Adding the loser to the 'Losers' List.
                    Losers.Add(match.Loser);
                }

                //moving items from challengersTemp to challengers and clean challengers temp in order to set the path for another round
                HelperMethods.MoveItems(_challengersTemp, ref _challengers, true);

                CurrentRound++;
                OnCurrentRoundLeveledUp(this, null);

                if (_exit)
                    return;
            }

            Winner = _challengers.First();
        }
            
        #endregion

        #region Irregular group
        private void ManageIrregularGroup()
        {
            var nativeChallengers = new List<Challenger>(_challengers);
            var remainingChallengers = FetchRemainigChallengers(ref nativeChallengers);

            var tempGroup = new Group(nativeChallengers, this.GroupLabel, Shuffle ,1);
            tempGroup.ManageRegularGroup();

            var challengersFromRoundOne = tempGroup._challengers;

            Matches.AddRange(tempGroup.Matches);
            Losers.AddRange(tempGroup.Losers);

            nativeChallengers = Enumerable.Concat(remainingChallengers, challengersFromRoundOne).ToList();
            _challengers = nativeChallengers;
            ManageRegularGroup();
        }

        private Challenger[] FetchRemainigChallengers(ref List<Challenger> nativeChallengers)
        {
            if (nativeChallengers == null)
                throw new ArgumentNullException(nameof(nativeChallengers), "Challengers list CANNOT be null");

            if (HelperMethods.IsFromThePowerOfTwo(nativeChallengers.Count))
                return null;

            //Getting the remaining count
            var remainingChallengersCount = GetRemainingChallengersCount(nativeChallengers);

            var remainingChallengers = nativeChallengers.Skip(nativeChallengers.Count - remainingChallengersCount).ToArray();
            nativeChallengers = nativeChallengers.Take(nativeChallengers.Count - remainingChallengersCount).ToList();

            return remainingChallengers;
        }

        private int GetRemainingChallengersCount(List<Challenger> nativeChallengers)
        {
            var nativeChallengersCount = nativeChallengers.Count;
            var nextPowerOfTwo = nativeChallengersCount.NextPowerOfTwo();
            var remainingChallengersCount = nextPowerOfTwo - nativeChallengersCount;
            return remainingChallengersCount;
        }
        #endregion

        private void Group_CurrentRoundLeveledUp(object sender, EventArgs e)
        {
            if (_exitRound != null)
            {
                if (_exitRound == CurrentRound)
                    _exit = true;
            }
        }
        private void Group_Finished(object sender, EventArgs e)
        {
            ReadyToRank = true;
        }

        #region ObjectDerivedMethods
        /// <summary>
        /// returns a bool indicating whether the current instance is equal to the obj Object
        /// </summary>
        /// <param name="obj">an object to test equality</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj) =>
            obj is Group ? obj as Group == this : false;

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode() =>
            base.GetHashCode();
        #endregion

        #endregion
    }
}
