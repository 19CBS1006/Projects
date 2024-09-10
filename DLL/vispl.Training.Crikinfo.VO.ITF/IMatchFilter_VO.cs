using System;

namespace Vispl.Training.Crickinfo.VO
{
    public interface IMatchFilter_VO
    {
        DateTime? MatchFrom { get; set; }
        string MatchFromOffset { get; set; }
        string MatchStatus { get; set; }
        DateTimeOffset? MatchTimings { get; set; }
        DateTime? MatchTo { get; set; }
        string MatchToOffset { get; set; }
        string MatchType { get; set; }
        string MatchResult { get; set; }
        string MatchVenue { get; set; }
        string Team_A { get; set; }
        string Team_B { get; set; }

        void Dispose();
    }
}