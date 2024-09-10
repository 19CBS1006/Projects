using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class ScoreBoard_VO: IDisposable
    {
        [DisplayName("Next Striker: ")]
        public int? Striker { get; set; }
        public int? NewStriker { get; set; }

        [DisplayName("Non-Striker: ")]
        public int? NonStriker { get; set; }

        [DisplayName("Next Bowler: ")]
        public int? Bowler { get; set; }
        public int? NewBowler { get; set; }

        [DisplayName("Runs Scored: ")]
        public string Runs { get; set; }

        [DisplayName("Extras: ")]
        public string SpecialBall { get; set; }

        [DisplayName("Out: ")]
        public int? Out { get; set; }

        [Required]
        public int? MatchID { get; set; }

        [Required]
        public int? BowlingTeamID { get; set; }

        [Required]
        public int? BattingTeamID { get; set; }

        [Required]
        public string Comment { get; set; }

        public int? BallNumber { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Striker = null;
                NewStriker = null;
                NewBowler = null;
                NonStriker = null;
                Bowler = null;
                Runs = null;
                SpecialBall = null;
                Out = null;
                MatchID = null;
                BattingTeamID = null;
                BowlingTeamID = null;
                Comment = null;
                BallNumber = null;
            }
        }
        ~ScoreBoard_VO()
        {
            Dispose(false);
        }
    }
}
