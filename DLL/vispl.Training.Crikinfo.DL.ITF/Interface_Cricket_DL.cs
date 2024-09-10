using System.Collections.Generic;
using System.Data;
using Vispl.Training.Crickinfo.VO;

namespace Vispl.Training.Crickinfo.DL
{
    public interface Interface_Cricket_DL 
    {
        object isValidLogin(string user);
        object TotalPlayers();
        object TotalTeams();
        object TotalMatches();


        int? IsLive(string ID);
        

        void addingPlayerDetails(Player_VO vo);
        void UpdatingTeamWithPlayers(Player_VO vo);
        void addingTeamDetails(Team_VO vo);
        void addingTeamPlayers(Team_VO vo);
        void UpdatingFranchise(Team_VO vo);
        void addingMatchDetails(Match_VO vo);
        void addingTossDetails(Toss_Decision_VO vo);
        int addingFinalDecison(Toss_Decision_VO vo);
        

        //Select Players Functions
        void addingBattingData(SelectPlayers_VO vo);
        void addingBowlingData(SelectPlayers_VO vo);
        void addingEachBallData(SelectPlayers_VO vo);
        void UpdatingNonStriker(SelectPlayers_VO vo);
        void UpdatingMatchStatus(SelectPlayers_VO vo);
        void UpdatingCaptains(SelectPlayers_VO vo);


        //Live Players
        int? CurrentBall(ScoreBoard_VO VO);
        int? PreviousBall(ScoreBoard_VO VO);
        DataTable BATTING_TEAM_LIVE(string ID);
        DataTable BOWLING_TEAM_LIVE(string ID);
        DataTable NOT_OUT_BATTING_PLAYERS(string ID);
        DataTable LIVE_MATCH_BOWLERS(string ID);
        DataTable BATTING_PLAYERS(string ID);
        DataTable BOWLING_PLAYERS(string ID);
        List<string> COMMENTARY(string ID);
        (int? BattingID, int? BowlingID) BattingBowlingIDS(string ID);
        (int? Striker, int? NonStriker, int? BattingTeamID) LiveMatchDetailsForBattingTeam(string ID);
        (int? Bowler, int? BowlingTeam, int? CurrentBall) LiveMatchDetailsForBowlingTeam(string ID);
        int? RunsCount(string ID);
        int? OutsCount(string ID);
        int? BallCount(string ID);

        int? RunsCount_2(string ID);
        int? BallCount_2(string ID);
        int? OutsCount_2(string ID);

        //For 4 & 6 Runs Count Seprately
        void Updating4RunsCount(ScoreBoard_VO Vo);
        void Updating6RunsCount(ScoreBoard_VO Vo);

        //For Commentary
        void COMMENTARY_RUNS_CASE1(ScoreBoard_VO Vo);

        //For 0 Runs
        void UpdatingBallCount(ScoreBoard_VO Vo);
        void InsertEachBallNewRecord(ScoreBoard_VO VO);

        //For NewBowler
        void NEWBOWLER_STRIKER(ScoreBoard_VO Vo);
        void NEWBOWLER_BOWLER(ScoreBoard_VO Vo);

        //CASE1:
        void NEWBOWLER_RUNS_CASE1(ScoreBoard_VO Vo);
        //CASE2:
        void NEWBOWLER_EXTRACASE_STRIKER_BOWLER(ScoreBoard_VO Vo);
        void NEWBOWLER_EXTRAS_CASE2(ScoreBoard_VO Vo);
        //CASE3:
        void NEWBOWLER_NEWSTRIKER_CASE3_PART1(ScoreBoard_VO Vo);
        void NEWBOWLER_NEWSTRIKER_CASE3_PART2(ScoreBoard_VO Vo);

        //Batting & Bowling Scoreboards
        DataTable HALF_SCOREBOARD(string ID);
        DataTable HALF_BOWLING(string ID);


        DataTable Players();
        DataTable Teams();

        DataTable Out();
        DataTable Countries();
        DataTable MatchResult();
        DataTable MatchVenue();
        DataTable MatchType();
        DataTable MatchStatus();
        DataTable TimeZoneOffset();

        

        DataTable GetPlayerData();
        DataTable GetTeamData();
        DataTable GetMatchData();
        DataTable GetFilteredData(MatchFilter_VO VO);
    }
}