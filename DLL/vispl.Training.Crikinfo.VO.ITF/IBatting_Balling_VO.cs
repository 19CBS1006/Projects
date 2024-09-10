namespace Vispl.Training.Crickinfo.VO
{
    public interface IBatting_Balling_VO
    {
        int? BallingTeamID { get; set; }
        int? BallNumber { get; set; }
        int? StrikerId { get; set; }
        int? NonStrikerId { get; set; }
        int? BattingTeamID { get; set; }
        int? BoundaryFourRuns { get; set; }
        int? BoundarySixRuns { get; set; }
        int? BowlerId { get; set; }
        int? ExtraRuns { get; set; }
        int? ExtraType { get; set; }
        int? FourRuns { get; set; }
        int? MatchID { get; set; }
        int? OneRun { get; set; }
        int? OutByBowler { get; set; }
        int? OverNumber { get; set; }
        int? RunsConceded { get; set; }
        float? StrikeRate { get; set; }
        int? ThreeRuns { get; set; }
        int? TotalBallsPlayed { get; set; }
        int? TotalRunsScored { get; set; }
        int? TwoRun { get; set; }
        int? WicketType { get; set; }
        int? Maiden { get; set; }
        int? WicketMaiden { get; set; }

        void Dispose();
    }
}