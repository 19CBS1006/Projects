using System.Collections.Generic;
using Vispl.Training.Crickinfo.VO;

namespace Vispl.Training.Crickinfo.BM
{
    public interface Interface_Cricket_BM
    {
        bool savingCredentials(Login_VO vo);


        object TotalPlayers();
        object TotalTeams();
        object TotalMatches();

        int? IsLive(string ID);

        void addingPlayerData(Player_VO VO);
        void addingTeamData(Team_VO VO);
        void addingMatchData(Match_VO VO);
        void addingTossDetails(Toss_Decision_VO VO);
        int? FinalDecisionValue(Toss_Decision_VO vo);
        void addingBattingBowlingData(SelectPlayers_VO vo);


        List<object> Out();
        List<object> Teams();
        List<object> Players();
        List<object> Countries();
        List<object> MatchStatus();
        List<object> MatchVenue();
        List<object> MatchType();
        List<object> MatchResult();
        List<object> TimeZoneOffset();


        List<object> BATTING_PLAYERS(string ID);
        List<object> BOWLING_PLAYERS(string ID);
        List<Team_VO> BATTING_TEAM(string ID);
        List<Team_VO> BOWLING_TEAM(string ID);
        List<Match_VO> MATCH_DATA(string ID);


        int? TotalRuns(string ID);
        int? TotalOuts(string ID);
        int? TotalBalls(string ID);
        int? TotalRuns_2(string ID);
        int? TotalOuts_2(string ID);
        int? TotalBalls_2(string ID);
        int? CurrentBall(ScoreBoard_VO VO);
        int? PreviousBall(ScoreBoard_VO VO);
        List<string> COMMENTARY(string ID);
        List<BATTING_VO> STRIKER(string ID);
        List<BATTING_VO> NON_STRIKER(string ID);
        List<BATTING_VO> BOWLER(string ID);
        List<BOWLING_VO> HALF_BOWLING(string ID);
        List<Team_VO> BATTING_TEAM_LIVE(string ID);
        List<Team_VO> BOWLING_TEAM_LIVE(string ID);
        (int? BattingID, int? BowlingID) BattingBowlingIDS(string ID);
        (int? Bowler, int? BowlingTeam, int? CurrentBall) LiveMatchDetailsForBowlingTeam(string ID);
        (int? Striker, int? NonStriker, int? BattingTeamID) LiveMatchDetailsForBattingTeam(string ID);
        (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount(string ID);
        (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount_2(string ID);
        List<object> NOT_OUT_BATTING_PLAYERS(string ID);
        List<object> LIVE_MATCH_BOWLERS(string ID);

        //Commentary
        void COMMENTARY_RUNS_CASE1(ScoreBoard_VO Vo);

        //Batting & Bowling Scoreboard
        List<BATTING_VO> BATTING_SCOREBOARD_1(string ID);
        List<BATTING_VO> BATTING_SCOREBOARD_2(string ID);
        List<BOWLING_VO> BOWLING_SCOREBOARD_1(string ID);
        List<BOWLING_VO> BOWLING_SCOREBOARD_2(string ID);
        List<BATTING_VO> HALF_SCOREBOARD(string ID);


        ///For Runs
        void UpdatingBallCount(ScoreBoard_VO VO);
        void InsertEachBallNewRecord(ScoreBoard_VO VO);


        //For 4 & 6 Runs
        void Updating4RunsCount(ScoreBoard_VO Vo);
        void Updating6RunsCount(ScoreBoard_VO Vo);


        //For Special Balls
        void SpecialBallHandling(ScoreBoard_VO Vo);


        //For New Player on Strike
        void NewStrikerAdding(ScoreBoard_VO Vo);


        //Changing Striker & NonStriker Posis after each over
        void ChangingStrikerNonStriker(ScoreBoard_VO Vo);


        //For New Bowler
        void NEWBOWLER_STRIKER(ScoreBoard_VO Vo);
        void NEWBOWLER_BOWLER(ScoreBoard_VO Vo);

        //CASE1
        void NEWBOWLER_RUNS_CASE1(ScoreBoard_VO Vo);
        //CASE2
        void NEWBOWLER_EXTRACASE_STRIKER_BOWLER(ScoreBoard_VO Vo);
        void NEWBOWLER_EXTRAS_CASE2(ScoreBoard_VO Vo);
        //CASE3:
        void NEWBOWLER_NEWSTRIKER_CASE3_PART1(ScoreBoard_VO Vo);
        void NEWBOWLER_NEWSTRIKER_CASE3_PART2(ScoreBoard_VO Vo);

        List<Player_VO> playerDetails();
        List<Team_VO> teamDetails();
        List<Match_VO> matchDetails();
        List<MatchFilter_VO> FilteredMatchData(MatchFilter_VO VO);


        List<Match_VO> LiveMatchData(string ID);
        List<Player_VO> Live_TeamA_PlayersDetails(string ID);
        List<Player_VO> Live_TeamB_PlayersDetails(string ID);
        List<Team_VO> Live_TeamA_Data(string ID);
        List<Team_VO> Live_TeamB_Data(string ID);


    }
}
