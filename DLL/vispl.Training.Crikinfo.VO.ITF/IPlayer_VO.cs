using System;

namespace Vispl.Training.Crickinfo.VO
{
    public interface IPlayer_VO
    {
        int? Age { get; set; }
        string AllRounder { get; set; }
        string Baller { get; set; }
        string Batsman { get; set; }
        string BirthPlace { get; set; }
        int? Centuries { get; set; }
        string Country { get; set; }
        DateTime? DebutDate { get; set; }
        DateTime? DOB { get; set; }
        string Fielder { get; set; }
        string FirstName { get; set; }
        string Franchise { get; set; }
        int? HalfCenturies { get; set; }
        int? ICCRanking { get; set; }
        string IsCaptain { get; set; }
        string IsSubstitute { get; set; }
        string Jersey { get; set; }
        string LastName { get; set; }
        int? MatchesPlayed { get; set; }
        string MiddleName { get; set; }
        string picture { get; set; }
        int? Player_ID { get; set; }
        string Position { get; set; }
        byte[] ProfilePicture { get; set; }
        int? RunsScored { get; set; }
        int? WicketsTaken { get; set; }

        void Dispose();
    }
}