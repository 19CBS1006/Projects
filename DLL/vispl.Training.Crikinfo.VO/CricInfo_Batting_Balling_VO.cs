using System;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class Batting_Balling_VO : IDisposable, IBatting_Balling_VO
    {
        [Required]
        public int? MatchID { get; set; }

        [Required]
        public int? BattingTeamID { get; set; }

        [Required]
        public int? BallingTeamID { get; set; }

        [Required]
        public int? StrikerId { get; set; }

        [Required]
        public int? BowlerId { get; set; }

        [Required]
        public int? BallNumber { get; set; }

        [Required]
        public int? RunsConceded { get; set; }


        public int? NonStrikerId { get; set; }
        public int? Maiden { get; set; }
        public int? WicketMaiden { get; set; }
        public int? WicketType { get; set; }
        public int? ExtraType { get; set; }
        public int? ExtraRuns { get; set; }
        public int? OverNumber { get; set; }
        public int? OutByBowler { get; set; }
        public int? ZeroRun {  get; set; }
        public int? OneRun { get; set; }
        public int? TwoRun { get; set; }
        public int? ThreeRuns { get; set; }
        public int? FourRuns { get; set; }
        public int? BoundaryFourRuns { get; set; }
        public int? BoundarySixRuns { get; set; }
        public float? StrikeRate { get; set; }

        public string Name { get; set; }
        public string JerseyNumber { get; set; }
        public string Picture { get; set; }


        public int? TotalBallsPlayed { get; set; }
        public int? TotalRunsScored { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MatchID = null;
                StrikerId = null;
                NonStrikerId = null;
                BowlerId = null;
                BattingTeamID = null;
                BallingTeamID = null;
                BallNumber = null;

                Maiden = null;
                WicketMaiden = null;
                WicketType = null;
                ExtraType = null;
                ExtraRuns = null;
                OverNumber = null;
                OutByBowler = null;
                RunsConceded = null;

                Name = null;
                JerseyNumber = null;
                Picture = null;

                ZeroRun = null;
                OneRun = null;
                TwoRun = null;
                ThreeRuns = null;
                FourRuns = null;
                BoundaryFourRuns = null;
                BoundarySixRuns = null;

                TotalRunsScored = null;
                TotalBallsPlayed = null;
            }
        }
        ~Batting_Balling_VO()
        {
            Dispose(false);
        }
    }
}
