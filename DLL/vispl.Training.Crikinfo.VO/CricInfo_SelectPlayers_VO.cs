using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class SelectPlayers_VO : IDisposable, ISelectPlayers_VO
    {
        [DisplayName("Select Striker: ")]
        [Required(ErrorMessage = "Select the Striker")]
        public int? Striker { get; set; }

        [DisplayName("Select Non-Striker: ")]
        [Required(ErrorMessage = "Select the Non-Striker")]
        public int? NonStriker { get; set; }

        [DisplayName("Select Captain: ")]
        [Required(ErrorMessage = "Select the Captain for Batting Team")]
        public int? BattingTeamCaptain { get; set; }

        [DisplayName("Select Bowler: ")]
        [Required(ErrorMessage = "Select the Baller")]
        public int? Bowler { get; set; }

        [DisplayName("Select Captain: ")]
        [Required(ErrorMessage = "Select the Captain for Bowling Team")]
        public int?BallingTeamCaptain { get; set; }

        [DisplayName("Select Playing 11: ")]
        [Required(ErrorMessage = "Please select the Playing 11 for Batting Team.")]
        public List<int?> BattingPlayers { get; set; }

        [DisplayName("Select Playing 11: ")]
        [Required(ErrorMessage = "Please select the Playing 11 for Bowling Team.")]
        public List<int?> BowlingPLayers { get; set; }


        public int? BattingTeamID { get; set; }
        public int? BowlingTeamID { get; set; }
        public int? MatchID { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                Striker = null;
                NonStriker = null;
                Bowler = null;
                BattingTeamCaptain = null;
                BallingTeamCaptain = null;
            }
        }
        ~SelectPlayers_VO()
        {
            Dispose(false);
        }
    }
}
