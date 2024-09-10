using System;

namespace Vispl.Training.Crickinfo.VO
{
    public interface IMatch_VO
    {
        DateTime? MatchSchedule { get; set; }
        string MatchScheduleOffset { get; set; }
        DateTimeOffset? MatchTimings { get; set; }
        string MatchType { get; set; }
        string MatchVenue { get; set; }
        string Team_A { get; set; }
        string Team_B { get; set; }
        int? MatchId { get; set; }
        
        void Dispose();
    }
}