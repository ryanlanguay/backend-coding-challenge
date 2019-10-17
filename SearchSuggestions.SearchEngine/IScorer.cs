using System;
using System.Collections.Generic;
using System.Text;

namespace SearchSuggestions.SearchEngine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface representing scoring class
    /// </summary>
    /// <typeparam name="T">The type of the expected and actual values</typeparam>
    public interface IScorer<in T>
    {
        Task<double> GetMatchScore(T expected, T actual);
    }
}
