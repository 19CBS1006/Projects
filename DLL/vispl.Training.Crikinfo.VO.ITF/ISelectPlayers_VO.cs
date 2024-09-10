namespace Vispl.Training.Crickinfo.VO
{
    public interface ISelectPlayers_VO
    {
        int? Bowler { get; set; }
        int? NonStriker { get; set; }
        int? Striker { get; set; }
        int? BattingTeamCaptain { get; set; }
        int? BallingTeamCaptain { get; set; }

        void Dispose();
    }
}