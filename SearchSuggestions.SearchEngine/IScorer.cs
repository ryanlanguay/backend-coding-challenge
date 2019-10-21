namespace SearchSuggestions.SearchEngine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface representing a class for scoring
    /// </summary>
    /// <typeparam name="T">The type of the received and actual values</typeparam>
    public interface IScorer<in T>
    {
        /// <summary>
        /// Given a received result and the actual result, compute the match score
        /// </summary>
        /// <param name="received">The received value</param>
        /// <param name="actual">The actual value</param>
        /// <returns>The score</returns>
        Task<double> GetMatchScore(T received, T actual);
    }
}