using System;

namespace TournamentSystem.Core
{
    /// <summary>
    /// Defines methods, fields and events every competition type should contain
    /// </summary>
    public interface ICompetition
    {
        /// <summary>
        /// occurs that the competition is started
        /// </summary>
        event EventHandler Started;
        /// <summary>
        /// occurs that the competition is finished
        /// </summary>
        event EventHandler Finished;
        /// <summary>
        /// Fires up the Started event when the competition is started
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event args object</param>
        void OnStarted(object sender, EventArgs e);
        /// <summary>
        /// Fires up the Started event when the competition is finished
        /// </summary>
        /// <param name="source">The sender of the event</param>
        /// <param name="e">Event args object</param>
        void OnFinished(object source, EventArgs e);
        /// <summary>
        /// Starts the competition
        /// </summary>
        void Start();
    }
}
