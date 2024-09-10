using System.Collections.Generic;

namespace Vispl.Training.Crickinfo.VO
{
    public interface ITeam_VO
    {
        List<string> Members { get; set; }
        List<int?> TeamMembers { get; set; }
        int? PlayersCount { get; set; }
        string TEAM_ICON { get; set; }
        string TeamCaptain { get; set; }
        byte[] TeamIcon { get; set; }
        string TeamKeeper { get; set; }
        string TeamName { get; set; }
        string TeamShortName { get; set; }
        string TeamViceCaptain { get; set; }
        int? PlayerID { get; set; }
        int? TeamID { get; set; }
        string name { get; set; }

        void Dispose();
    }
}