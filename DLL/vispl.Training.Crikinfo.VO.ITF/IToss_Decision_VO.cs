namespace Vispl.Training.Crickinfo.VO
{
    public interface IToss_Decision_VO
    {
        int? TossID { get; set; }
        string MatchID { get; set; }
        string TeamASide { get; set; }
        string TeamBSide { get; set; }
        int? TossLostBy { get; set; }
        int? TossWonBy { get; set; }
        string Decision { get; set; }

        void Dispose();
    }
}