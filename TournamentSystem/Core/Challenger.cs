using System;
using TournamentSystem.Exceptions;

namespace TournamentSystem.Core
{
    /// <summary>
    /// A Challenger which go into matches with another challengers and gain points which determines whether he\she won the match or not
    /// </summary>
    /// <seealso cref="TournamentSystem.Core.Tournament"/>
    /// <seealso cref="TournamentSystem.Core.Match" />
    /// <seealso cref="TournamentSystem.Core.Group"/>
    /// <remarks>
    /// Challenger has a name and points. First of all: he\she joins a tournament then he\she falls into(randomly) one of the two groups made. 
    /// Then he\she challenges one of his\her opponents and depending of the points gained he\she wins or loses. 
    /// The system is knockout-once system. Meaning that if he\she lose in a single match. he\she leaves the coming rounds and be sorted as a loser.
    /// But if he\she wins, he\she goes to the next round and soon.
    /// </remarks>
    /// <example>
    /// <code>
    /// //creating two Challenger objects
    /// var challenger = new Challenger("Adham");
    /// var challenger2 = "Ali" //implicit conversion
    /// 
    /// //Creating a match
    /// var match = new Match(challenger, challenger2);
    /// //Stating the match
    /// match.Start();
    /// //Printing out the winner and the loser names
    /// Console.WriteLine($"Winner: {match.Winner.Name}");
    /// Console.WriteLine($"Loser: {match.Loser.Name}");
    /// </code>
    /// </example>
    public sealed class Challenger : IEquatable<Challenger>
    {
        #region Over-loadable operators
        /* ==
         * !=
         * <
         * >
         * <=
         * >=*/

        /// <summary>
        /// Indicates whether the left-hand side instance are equal to the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns>bool</returns>
        public static bool operator ==(Challenger lhs, Challenger rhs) => lhs?.Name == rhs?.Name && lhs?.Points == rhs?.Points;
        /// <summary>
        /// Indicates whether the left-hand side instance are not equal to the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns>bool</returns>
        public static bool operator !=(Challenger lhs, Challenger rhs) => !(lhs?.Name == rhs?.Name && lhs?.Points == rhs?.Points);
        /// <summary>
        /// Indicates whether the left-hand side instance are less than the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns>bool</returns>
        public static bool operator <(Challenger lhs, Challenger rhs) => lhs.Points < rhs.Points;
        /// <summary>
        /// Indicates whether the left-hand side instance are greater than the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns>bool</returns>
        public static bool operator >(Challenger lhs, Challenger rhs) => lhs.Points > rhs.Points;
        /// <summary>
        /// Indicates whether the left-hand side instance are less than or equal to the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns>bool</returns>
        public static bool operator <=(Challenger lhs, Challenger rhs) => lhs.Points <= rhs.Points;
        /// <summary>
        /// Indicates whether the left-hand side instance are greater than or equal to the right-hand side instance.
        /// </summary>
        /// <param name="lhs">The first Challenger argument.</param>
        /// <param name="rhs">The second Challenger argument.</param>
        /// <returns></returns>
        public static bool operator >=(Challenger lhs, Challenger rhs) => lhs.Points >= rhs.Points;
        #endregion

        #region Over-loadable conversions
        /// <summary>
        /// Converts a String into Challenger implicitly
        /// </summary>
        /// <param name="name">a String to convert.</param>
        public static implicit operator Challenger(string name) => new Challenger(name);
        #endregion

        #region Public properties
        /// <summary>
        ///The name of the challenger
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        ///The points matches user to determine whether the player is a winner or a loser
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// Used for UI purposes only
        /// </summary>
        public Xamarin.Forms.Color Color { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes an instance of Challenger class with name and point(as optional parameter)
        /// </summary>
        /// <param name="name">The name of the challenger</param>
        public Challenger(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name, "Challenger name CANNOT be whether null or empty");

            Name = name;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the passed challenger arguments are null and throwing an ArgumentNullException if so 
        /// </summary>
        /// <param name="challengerX"></param>
        /// <param name="challengerY"></param>
        private static void CheckChallenegers(Challenger challengerX, Challenger challengerY)
        {
            if (challengerX == null || challengerY == null)
                throw new ChallengerException("Challengers CANNOT be null", new ArgumentNullException());
        }

        /// <summary>
        /// Determines the winner player 
        /// </summary>
        /// <param name="challengerX">the first challenger</param>
        /// <param name="challengerY">the second challenger</param>
        /// <returns>Challenger</returns>
        public static Challenger Winner(Challenger challengerX, Challenger challengerY)
        {
            CheckChallenegers(challengerX, challengerY);

            return challengerX > challengerY ? challengerX : challengerY;
        }

        /// <summary>
        /// Determines the loser player 
        /// </summary>
        /// <param name="challengerX">the first challenger</param>
        /// <param name="challengerY">the second challenger</param>
        /// <returns>Challenger</returns>
        public static Challenger Loser(Challenger challengerX, Challenger challengerY)
        {
            CheckChallenegers(challengerX, challengerY);

            return challengerX < challengerY ? challengerX : challengerY;
        }


        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified Challenger.
        /// </summary>
        /// <param name="other">Challenger instance to test equality with.</param>
        /// <returns>bool</returns>
        public bool Equals(Challenger other) =>
            this == other;

        #region Object derived methods
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">object instance to test equality with.</param>
        /// <returns></returns>
        public override bool Equals(object obj) =>
            (obj != null) && this == obj as Challenger;

        /// <summary>
        /// Converts this Challenger instance to its equivalent string representation.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $"Challenger name: {Name}, points: {Points}";

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode() => base.GetHashCode();
        #endregion
        #endregion
    }
}
