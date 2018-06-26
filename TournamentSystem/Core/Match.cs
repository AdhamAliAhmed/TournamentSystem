using System;
using TournamentSystem.Exceptions;
using Troschuetz;
namespace TournamentSystem.Core
{
    /// <summary>
    /// A match between two challengers that determine(randomly) the winner and the loser
    /// </summary>
    /// <remarks>
    /// Generates a random points for each challenger, makes a match which will determine the winner and the loser depending on the points.
    /// </remarks>
    /// <seealso cref="TournamentSystem.Core.Challenger"/>
    /// <seealso cref="TournamentSystem.Core.Group"/>
    /// <seealso cref="TournamentSystem.Core.Tournament"/>
    /// <seealso cref="TournamentSystem.Core.ICompetition"/>
    public sealed class Match : ICompetition
    {
        #region Events
        /// <summary>
        /// Occurs when the match is started.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Occurs when the match is finished.
        /// </summary>
        public event EventHandler Finished;
        #endregion

        #region Events firing methods
        /// <summary>
        /// Fires the Started event if it's not null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The EventArgs for the event</param>
        public void OnStarted(object sender, EventArgs e)
        {
            Started?.Invoke(this, null);
        }
        /// <summary>
        /// Fires the Finished event if it's not null
        /// </summary>
        /// <param name="source">The sender of the event</param>
        /// <param name="e">The EventArgs for the event</param>
        public void OnFinished(object source, EventArgs e)
        {
            Finished?.Invoke(this, null);
        }
        #endregion

        #region Read-only data members
        /// <summary>The first Challenger initialized by the constructor</summary>
        public readonly Challenger ChallengerX;

        /// <summary>The second Challenger initialized by the constructor</summary>
        public readonly Challenger ChallengerY;
        #endregion

        #region Front-end public properties

        /// <summary>The winner of the match</summary>
        public Challenger Winner { get; set; }

        /// <summary>The loser of the match</summary>
        public Challenger Loser { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// The default constructor asks for the first challenger and the second challenger of the match
        /// </summary>
        /// <param name="challengerX">The challenger1 of the match</param>
        /// <param name="challengerY">The challenger2 of the match</param>
        public Match(Challenger challengerX, Challenger challengerY)
        {
            if (challengerX == null || challengerY == null)
                throw new ChallengerException("Challengers CANNOT be null", new ArgumentNullException());

            ChallengerX = challengerX;
            ChallengerY = challengerY;
        }
        #endregion

        #region Methods
        /// <summary>
        /// The main method for the class which generates random points for each challenger and determines the winner and the loser depending on the generated points
        /// </summary>
        private void KickOff()
        {
            //generating and assigning the random points for each player
            GenerateAndAssignPoints(ChallengerX, ChallengerY);

            //assigning the winner and the loser as well
            ManageResults(ChallengerX, ChallengerY);
        }

        /// <summary>
        /// Generates 2 random numbers and assign each one to the point property for the challengers arguments provided
        /// </summary>
        /// <param name="challengerX">The First challenger</param>
        /// <param name="challengerY">The Second challenger</param>
        public void GenerateAndAssignPoints(Challenger challengerX, Challenger challengerY)
        {
            Troschuetz.Random.Generators.StandardGenerator generator = new Troschuetz.Random.Generators.StandardGenerator();
            challengerX.Points = generator.Next();
            challengerY.Points = generator.Next();
        }
        /// <summary>
        /// Assigns a winner and a loser from 2 challengers
        /// </summary>
        /// <param name="challengerX">The first challenger</param>
        /// <param name="challengerY">The second challenger</param>
        public void ManageResults(Challenger challengerX, Challenger challengerY)
        {
            //assigning the winner
            Winner = Challenger.Winner(challengerX, challengerY);
            //assigning the loser
            Loser = Challenger.Loser(challengerX, challengerY);
        }
        /// <summary>
        /// Starts the match process with some utilities such as firing some events for match starting and match finishing as well
        /// </summary>
        public void Start()
        {
            //Indicate the match is started
            OnStarted(this, null);
            //Kick-off the match
            KickOff();
            //Indicate the match is finished
            OnFinished(this, null);
        }
        #endregion
    }
}
