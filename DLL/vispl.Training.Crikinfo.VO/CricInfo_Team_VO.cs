using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class Team_VO : IDisposable, ITeam_VO
    {

        /// <summary>
        /// Team Icon
        /// </summary>
        [Display(Name = "Team Icon")]
        public byte[] TeamIcon { get; set; }
        public string TEAM_ICON { get; set; }


        /// <summary>
        /// Team Full Name
        /// </summary>
        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }


        /// <summary>
        /// Team Short Name
        /// </summary>
        [Required]
        [Display(Name = "Team Short Name")]
        public string TeamShortName { get; set; }


        /// <summary>
        /// Team Captain
        /// </summary>
        [Display(Name = "Team Captain")]
        public string TeamCaptain { get; set; }


        /// <summary>
        /// Team Vice Captain
        /// </summary>
        [Display(Name = "Team Vice Captain")]
        public string TeamViceCaptain { get; set; }


        /// <summary>
        /// Team Members
        /// </summary>
        [Display(Name = "Team Members")]
        public List<int?> TeamMembers { get; set; }


        /// <summary>
        /// Wicket Keeper For Team
        /// </summary>
        [Display(Name = "Wicket Keeper")]
        public string TeamKeeper { get; set; }


        /// <summary>
        /// Displaying Team Members
        /// </summary>
        public List<string> Members { get; set; }
        public string name { get; set; }


        /// <summary>
        /// Represnts Total Player Count in that Team
        /// </summary>
        public int? PlayersCount { get; set; }


        public int? PlayerID { get; set; }
        public int? TeamID { get; set; }


        /// <summary>
        /// Disposing VO
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TeamIcon = null;
                TEAM_ICON = null;
                TeamName = null;
                TeamShortName = null;
                TeamCaptain = null;
                TeamViceCaptain = null;
                if(TeamMembers != null)
                {
                    TeamMembers.Clear();
                    TeamMembers = null;
                }
                TeamKeeper = null;
                if(Members != null)
                {
                    Members.Clear();
                    Members = null;
                }
                PlayersCount = null;
                PlayerID = null;
                name = null;
                TeamID = null;
            }
        }
        ~Team_VO()
        {
            Dispose(false);
        }
    }
}
