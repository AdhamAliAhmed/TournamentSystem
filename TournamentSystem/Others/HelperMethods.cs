using System;
using System.Collections.Generic;

namespace TournamentSystem.Others
{
    /// <summary>
    /// Includes some methods which has an abstract logic and can be used in many places not this subject only
    /// </summary>
    public static class HelperMethods
    {
        /// <summary>
        /// Randomizes an IList instance 
        /// </summary>
        /// <typeparam name="T">Argument type for the type of the IList object</typeparam>
        /// <param name="list">A list to shuffle or randomize</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Moves elements from a list to another one
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="itemsToMove">The list containing the items which will be moved</param>
        /// <param name="destintation">The list which will contain the moved items</param>
        /// <param name="clearDistinationList">Indicates whether the destination list is cleared before moving items or not</param>
        public static void MoveItems<T>(List<T> itemsToMove, ref List<T> destintation, bool clearDistinationList)
        {
            if (itemsToMove == null)
                throw new ArgumentNullException(nameof(itemsToMove), "The list which contains the items CANNOT be null");

            if (destintation == null)
                destintation = new List<T>();

            if (clearDistinationList)
                destintation.Clear();


            foreach (var item in itemsToMove)
            {
                destintation.Add(item);
            }
            itemsToMove.Clear();
        }

        /// <summary>
        /// Returns a boolean indicating whether the provided integer argument is from the power of 2 or not
        /// </summary>
        /// <param name="number">Number to test</param>
        /// <returns>bool</returns>
        public static bool IsFromThePowerOfTwo(this int number)
        {
            if (number == 0)
                return false;
            while (number != 1)
            {
                if (number % 2 != 0)
                    return false;
                number = number / 2;
            }
            return true;
        }

        /// <summary>
        /// Rounds up to the next power of two
        /// </summary>
        /// <param name="number">Number to round up</param>
        /// <returns>int</returns>
        public static int NextPowerOfTwo(this int number)

        {
            int value = 1;
            while (value <= number)
            {
                value = value << 1;
            }
            return value;

        }
    }
}
