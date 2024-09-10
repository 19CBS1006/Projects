using System;
using System.Data;
using System.Collections.Generic;
using Vispl.Training.Crickinfo.DL;
using Vispl.Training.Crickinfo.VO;

namespace Vispl.Training.Crickinfo.BM
{
    /// <summary>
    /// Business Model Layer
    /// </summary>
    public class Cricket_BM : Interface_Cricket_BM,IDisposable
    {
        private Cricket_DL dl;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (dl != null)
                {
                    dl.Dispose();
                    dl = null;
                }
            }
        }
        ~Cricket_BM()
        {
            Dispose(false);
        }



        /// <summary>
        /// Verifying Login Credentials
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public bool savingCredentials(Login_VO vo)
        {
            try
            {
                dl = new Cricket_DL();
                string sqlPass = dl.isValidLogin(vo.Username)?.ToString();

                if (sqlPass == vo.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Total Players Count
        /// </summary>
        /// <returns></returns>
        public object TotalPlayers()
        {
            try
            {
                dl = new Cricket_DL();
                return dl?.TotalPlayers();
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Total Teams Count
        /// </summary>
        /// <returns></returns>
        public object TotalTeams()
        {
            try
            {
                dl = new Cricket_DL();
                return dl?.TotalTeams();
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Tota Match Count
        /// </summary>
        /// <returns></returns>
        public object TotalMatches()
        {
            try
            {
                dl = new Cricket_DL();
                return dl?.TotalMatches();
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Adding Player Details From DL Layer
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vo"></param>
        public void addingPlayerData(Player_VO VO)
        {
            try
            {
                dl = new Cricket_DL();
                dl.addingPlayerDetails(VO);

                if(VO.Franchise != null && VO.Player_ID != null)
                {
                    dl.UpdatingTeamWithPlayers(VO);
                }
            }
            finally
            {
                dl = null;
            }
            
        }


        /// <summary>
        /// Adding Team Details From DL Layer
        /// </summary>
        /// <param name="VO"></param>
        public void addingTeamData(Team_VO VO)
        {
            try
            {
                dl = new Cricket_DL();
                dl.addingTeamDetails(VO);

                if(VO.TeamMembers != null && VO.TeamMembers.Count > 0)
                {
                    dl.addingTeamPlayers(VO);
                    dl.UpdatingFranchise(VO);
                }
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Adding Match Details From DL Layer
        /// </summary>
        /// <param name="VO"></param>
        public void addingMatchData(Match_VO VO)
        {
            try
            {
                dl = new Cricket_DL();

                dl.addingMatchDetails(VO);
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Adding Toss Data 
        /// </summary>
        /// <param name="VO"></param>
        public void addingTossDetails(Toss_Decision_VO VO)
        {
            try
            {
                dl = new Cricket_DL();

                dl.addingTossDetails(VO);
            }
            finally
            {
                dl = null;
            }
        }


        /// <summary>
        /// Retriving Final Decision ID
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int? FinalDecisionValue(Toss_Decision_VO vo)
        {
            dl = new Cricket_DL();
            int value = dl.addingFinalDecison(vo);

            if(dl != null)
            {
                dl = null;
            }

            return value;
        }


        /// <summary>
        /// Adding Batting & Bowling Data To Batting Table
        /// </summary>
        /// <param name="vo"></param>
        public void addingBattingBowlingData(SelectPlayers_VO vo)
        {
            dl = new Cricket_DL();

            dl.addingBattingData(vo);
            dl.addingBowlingData(vo);
            dl.addingEachBallData(vo);
            dl.UpdatingCaptains(vo);
            dl.UpdatingNonStriker(vo);
            dl.UpdatingMatchStatus(vo);

            if (dl != null)
            {
                dl = null;
            }
        }


        /// <summary>
        /// List of Players 
        /// </summary>
        /// <returns></returns>
        public List<object> Players()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.Players())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["FIRST NAME"]?.ToString() + " " + row["MIDDLE NAME"]?.ToString() + " " + row["LAST NAME"]?.ToString();
                        id = row["ID"];

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    name = null;
                    id = null;
                    details = null;
                }
            }   
        }


        /// <summary>
        /// List of Teams
        /// </summary>
        /// <returns></returns>
        public List<object> Teams()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.Teams())
            {
                object name = null;
                object id = null;
                List<object> details = new List<object>();
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["TEAM NAME"]?.ToString();
                        id = Convert.ToString(row["TEAM ID"]);

                        details.Add(new {Text = name, Value = id});
                    }
                    return details;
                }
                finally
                {
                    dl = null; 
                    id = null;
                    name = null;
                    details = null;
                    table.Clear();
                    table.Dispose();
                }
            }
        }


        /// <summary>
        /// List of All Countries
        /// </summary>
        /// <returns></returns>
        public List<object> Countries()
        {
            if (dl == null)
            {
                dl = new Cricket_DL();
            }
            using(DataTable table = dl.Countries())
            {
                object name;
                object id;
                List<object> details = new List<object>();

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["country"].ToString();
                        id = row["ID"].ToString();
                        details.Add(new { text = name, value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    name = null;
                    id = null;
                    details = null;
                }
            }
        }


        /// <summary>
        /// Player Details for View
        /// </summary>
        public List<Player_VO> playerDetails()
        {
            dl = new Cricket_DL();

            using (DataTable table = dl.GetPlayerData())
            {
                Player_VO vo;
                List<Player_VO> details = new List<Player_VO>();

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Player_VO()
                        {
                            ProfilePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            Jersey = row["JERSEY NUMBER"].ToString(),
                            FirstName = row["FIRST NAME"].ToString(),
                            MiddleName = row["MIDDLE NAME"].ToString(),
                            LastName = row["LAST NAME"].ToString(),
                            DOB = Convert.ToDateTime(row["DOB"]),
                            Age = Convert.ToInt32(row["AGE"]),
                            BirthPlace = row["BIRTH PLACE"].ToString(),
                            IsCaptain = (row["CAPTAIN"]).ToString(),
                            IsSubstitute = (row["SUBSTITUTE"]).ToString(),
                            Franchise = row["FRANCHISE"].ToString(),
                            MatchesPlayed = Convert.ToInt32(row["MATCHES PLAYED"]),
                            RunsScored = Convert.ToInt32(row["RUNS SCORED"]),
                            WicketsTaken = Convert.ToInt32(row["WICKETS TAKEN"]),
                            Centuries = Convert.ToInt32(row["CENTURIES"]),
                            HalfCenturies = Convert.ToInt32(row["HALF CENTURIES"]),
                            DebutDate = Convert.ToDateTime(row["DEBUT DATE"]),
                            ICCRanking = Convert.ToInt32(row["ICC RANKING"]),
                            Country = row["COUNTRY"].ToString(),
                            Position = row["POSITION"].ToString()
                        };

                        if (vo.ProfilePicture != null)
                        {
                            vo.picture = Convert.ToBase64String(vo.ProfilePicture);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                catch(Exception ex)
                {
                    throw new Exception (ex.Message);
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Team Details for View
        /// </summary>
        /// <returns></returns>
        public List<Team_VO> teamDetails()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.GetTeamData())
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"] + " " + row["LAST NAME"];
                        }
                        else
                        {
                            name = row["FIRST NAME"] + " " + row["MIDDLE NAME"] + " " + row["LAST NAME"];
                        }
                        

                        vo = new Team_VO()
                        {
                            name = name.ToString(),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamName = row["TEAM NAME"].ToString(),
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                            TeamCaptain = row["TEAM CAPTAIN"].ToString(),
                            TeamViceCaptain = row["TEAM VICE CAPTAIN"].ToString(),
                            TeamKeeper = row["TEAM WICKET KEEPER"].ToString().Trim()
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }   
        }


        /// <summary>
        /// Match Details for View
        /// </summary>
        /// <returns></returns>
        public List<Match_VO> matchDetails()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.GetMatchData())
            {
                List<Match_VO> details = new List<Match_VO>();
                Match_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Match_VO()
                        {
                            MatchId = (int)row["ID"],
                            Team_A = row["TEAM_A"].ToString(),
                            Team_B = row["TEAM_B"].ToString(),
                            MatchType = row["MATCH_TYPE"].ToString(),
                            MatchVenue = row["MATCH_VENUE"].ToString(),
                            MatchTimings = (DateTimeOffset)row["MATCH_SCHEDULE"]
                        };
                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Filtered Match Details
        /// </summary>
        /// <returns></returns>
        public List<MatchFilter_VO> FilteredMatchData(MatchFilter_VO VO)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.GetFilteredData(VO))
            {
                List<MatchFilter_VO> details = new List<MatchFilter_VO>();

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        VO = new MatchFilter_VO()
                        {
                            Team_A = row["TEAM_A"].ToString(),
                            Team_B = row["TEAM_B"].ToString(),
                            MatchResult = row["MATCH_STATUS"].ToString(),
                            MatchType = row["MATCH_TYPE"].ToString(),
                            MatchVenue = row["MATCH_VENUE"].ToString(),
                            MatchStatus = row["MATCH_STATUS"].ToString(),
                            MatchTimings = (DateTimeOffset)(row["MATCH_SCHEDULE"])
                        };
                        details.Add(VO);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                }
            }
        }


        /// <summary>
        /// Match Result Extraction
        /// </summary>
        /// <returns></returns>
        public List<object> MatchResult()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.MatchResult())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["RESULT"]?.ToString();
                        id = (string)name + "~" + (int)row["ID"];

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    name = null;
                    id = null;
                    details = null;
                    table.Clear();
                    table.Dispose();
                }
            }
        }


        /// <summary>
        /// Match Status Extraction
        /// </summary>
        /// <returns></returns>
        public List<object> MatchStatus()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.MatchStatus())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["STATUS"]?.ToString();
                        id = (string)name + "~" + (int)row["ID"];

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    name = null;
                    id = null;
                    details = null;
                    table.Clear();
                    table.Dispose();
                }
            }
        }


        /// <summary>
        /// Match Type Extraction
        /// </summary>
        /// <returns></returns>
        public List<object> MatchType()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.MatchType())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["TYPE"]?.ToString();
                        id = (int)row["ID"];

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    name = null;
                    id = null;
                    details = null;
                    table.Clear();
                    table.Dispose();
                }
            }
        }


        /// <summary>
        /// Match Venue Extraction
        /// </summary>
        /// <returns></returns>
        public List<object> MatchVenue()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.MatchVenue())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["VENUE"]?.ToString();
                        id = (int)row["ID"];

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    name = null;
                    id = null;
                    details = null;
                    table.Clear();
                    table.Dispose();
                }
            }
        }


        /// <summary>
        /// Time Zone Offset Extraction
        /// </summary>
        /// <returns></returns>
        public List<object> TimeZoneOffset()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TimeZoneOffset())
            {
                List<object> details = new List<object>();
                object name = null;
                object id = null;
                object offset = null;
                object showing = null;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        name = row["ZONE"]?.ToString();
                        offset = row["OFFSET"]?.ToString();
                        showing = name + " (" + offset + ")";

                        id = (string)row["OFFSET"];

                        details.Add(new { Text = showing, Value = id });
                    }
                    return details;
                }
                finally
                {
                    if(dl != null)
                    {
                        dl = null;
                        name = null;
                        id = null;
                        offset = null;
                        showing = null;
                        details = null;
                        table.Clear();
                        table.Dispose();
                    }
                }
            }
        }


        /// <summary>
        /// List of Out Types
        /// </summary>
        /// <returns></returns>
        public List<object> Out()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.Out())
            {
                List<object> details = new List<object>();

                object name = null;
                object id = null;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        id = row["ID"]?.ToString();
                        name = row["out"]?.ToString();

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    if (dl != null)
                    {
                        dl = null;
                        name = null;
                        id = null;
                        details = null;
                        table.Clear();
                        table.Dispose();
                    }
                }
            }
        }


        //LIVE...


        //(PENDING) Make it for Todays Date
        /// <summary>
        /// Matches
        /// </summary>
        /// <returns></returns>
        public List<Match_VO> TodayMatchData()
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.GetMatchData())
            {
                List<Match_VO> details = new List<Match_VO>();
                Match_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Match_VO()
                        {
                            MatchId = (int)row["ID"],
                            Team_A = row["TEAM_A"].ToString(),
                            Team_B = row["TEAM_B"].ToString(),
                            MatchType = row["MATCH_TYPE"].ToString(),
                            MatchVenue = row["MATCH_VENUE"].ToString(),
                            MatchTimings = (DateTimeOffset)row["MATCH_SCHEDULE"]
                        };
                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// To Check if the match is already in the DB
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int? IsLive(string ID)
        {
            dl = new Cricket_DL();

            return dl.IsLive(ID);
        }


        /// <summary>
        /// For Showing Match Details of Particular Match
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Match_VO> LiveMatchData(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.MatchData(ID))
            {
                List<Match_VO> details = new List<Match_VO>();
                Match_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Match_VO()
                        {
                            Team_A = row["TEAM_A"].ToString(),
                            Team_B = row["TEAM_B"].ToString(),
                            MatchType = row["MATCH_TYPE"].ToString(),
                            MatchVenue = row["MATCH_VENUE"].ToString(),
                            MatchTimings = (DateTimeOffset)row["MATCH_SCHEDULE"]
                        };
                        details.Add(vo);
                    }
                    return details;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Player Details of Team A
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Player_VO> Live_TeamA_PlayersDetails(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TeamA_Players(ID))
            {
                List<Player_VO> details = new List<Player_VO>();
                Player_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Player_VO()
                        {
                            ProfilePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            Player_ID = Convert.ToInt32(row["ID"]),
                            Jersey = row["JERSEY NUMBER"].ToString(),
                            FirstName = row["FIRST NAME"].ToString(),
                            MiddleName = row["MIDDLE NAME"].ToString(),
                            LastName = row["LAST NAME"].ToString(),
                        };

                        if (vo.ProfilePicture != null)
                        {
                            vo.picture = Convert.ToBase64String(vo.ProfilePicture);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Player Details of Team B
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Player_VO> Live_TeamB_PlayersDetails(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TeamB_Players(ID))
            {
                List<Player_VO> details = new List<Player_VO>();
                Player_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Player_VO()
                        {
                            ProfilePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            Player_ID = Convert.ToInt32(row["ID"]),
                            Jersey = row["JERSEY NUMBER"].ToString(),
                            FirstName = row["FIRST NAME"].ToString(),
                            MiddleName = row["MIDDLE NAME"].ToString(),
                            LastName = row["LAST NAME"].ToString(),
                        };

                        if (vo.ProfilePicture != null)
                        {
                            vo.picture = Convert.ToBase64String(vo.ProfilePicture);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Team A Details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> Live_TeamA_Data(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TeamA_LiveData(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Team B Details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> Live_TeamB_Data(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TeamB_LiveData(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Generating Player ID & Player Name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<object> BOWLING_PLAYERS(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLING_PLAYERS(ID))
            {
                List<object> details = new List<object>();

                object name;
                object id;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        id = Convert.ToInt32(row["ID"]);

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    name = null;
                    id = null;
                }
            }
        }


        /// <summary>
        /// Generating Batting Players Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<object> BATTING_PLAYERS(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BATTING_PLAYERS(ID))
            {
                List<object> details = new List<object>();

                object name;
                object id;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        id = Convert.ToInt32(row["ID"]);

                        details.Add(new { Text = name, Value = id});
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    name = null;
                    id = null;
                }
            }
        }


        /// <summary>
        /// Generating Batting Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> BATTING_TEAM(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BATTING_TEAM(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamName = row["TEAM NAME"].ToString(),
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Generating Balling Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> BOWLING_TEAM(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLING_TEAM(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamName = row["TEAM NAME"].ToString(),
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Generating Match Details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Match_VO> MATCH_DATA(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.TOSS_MATCH_DATA(ID))
            {
                List<Match_VO> details = new List<Match_VO>();
                Match_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Match_VO()
                        {
                            MatchId = Convert.ToInt32(row["ID"]),
                            MatchType = row["MATCH_TYPE"].ToString(),
                            MatchVenue = row["MATCH_VENUE"].ToString(),
                            MatchTimings = (DateTimeOffset)row["MATCH_SCHEDULE"]
                        };
                        details.Add(vo);
                    }
                    return details;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Generating Batting Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> BATTING_TEAM_LIVE(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BATTING_TEAM_LIVE(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamName = row["TEAM NAME"].ToString(),
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Generating Balling Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Team_VO> BOWLING_TEAM_LIVE(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLING_TEAM_LIVE(ID))
            {
                List<Team_VO> details = new List<Team_VO>();
                Team_VO vo;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vo = new Team_VO()
                        {
                            TeamID = Convert.ToInt32(row["TEAM ID"]),
                            TeamIcon = row["TEAM_ICON"] != DBNull.Value ? (byte[])row["TEAM_ICON"] : null,
                            TeamName = row["TEAM NAME"].ToString(),
                            TeamShortName = row["TEAM SHORT NAME"].ToString(),
                        };

                        if (vo.TeamIcon != null)
                        {
                            vo.TEAM_ICON = Convert.ToBase64String(vo.TeamIcon);
                        }

                        details.Add(vo);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    vo = null;
                }
            }
        }


        /// <summary>
        /// Players which are not out YET
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<object> NOT_OUT_BATTING_PLAYERS(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.NOT_OUT_BATTING_PLAYERS(ID))
            {
                List<object> details = new List<object>();

                object name;
                object id;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        id = Convert.ToInt32(row["ID"]);

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    name = null;
                    id = null;
                }
            }
        }


        /// <summary>
        /// Players 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<object> LIVE_MATCH_BOWLERS(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.LIVE_MATCH_BOWLERS(ID))
            {
                List<object> details = new List<object>();

                object name;
                object id;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        id = Convert.ToInt32(row["ID"]);

                        details.Add(new { Text = name, Value = id });
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    name = null;
                    id = null;
                }
            }
        }


        /// <summary>
        /// Live Bowling Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? Bowler, int? BowlingTeam, int? CurrentBall) LiveMatchDetailsForBowlingTeam(string ID)
        {
            dl = new Cricket_DL();
            return dl.LiveMatchDetailsForBowlingTeam(ID);
        }


        /// <summary>
        /// Live Batting Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? Striker, int? NonStriker, int? BattingTeamID) LiveMatchDetailsForBattingTeam(string ID)
        {
            dl = new Cricket_DL();
            return dl.LiveMatchDetailsForBattingTeam(ID);
        }


        public (int? BattingID, int? BowlingID) BattingBowlingIDS(string ID)
        {
            if(dl != null)
            {
                return dl.BattingBowlingIDS(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.BattingBowlingIDS(ID);
            }
        }


        public (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount(string ID)
        {
            if(dl != null)
            {
                return dl.ExtrasCount(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.ExtrasCount(ID);
            }
        }
        public (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount_2(string ID)
        {
            if (dl != null)
            {
                return dl.ExtrasCount_2(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.ExtrasCount_2(ID);
            }
        }


        public int? TotalRuns(string ID)
        {
            if(dl != null)
            {
                return dl.RunsCount(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.RunsCount(ID);
            }
        }
        public int? TotalOuts(string ID)
        {
            if (dl != null)
            {
                return dl.OutsCount(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.OutsCount(ID);
            }
        }
        public int? TotalBalls(string ID)
        {
            if(dl != null)
            {
                return dl.BallCount(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.BallCount(ID);
            }
        }
        public int? TotalRuns_2(string ID)
        {
            if(dl != null)
            {
                return dl.RunsCount_2(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.RunsCount_2(ID);
            }
        }
        public int? TotalOuts_2(string ID)
        {
            if (dl != null)
            {
                return dl.OutsCount_2(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.OutsCount_2(ID);
            }
        }
        public int? TotalBalls_2(string ID)
        {
            if (dl != null)
            {
                return dl.BallCount_2(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.BallCount_2(ID);
            }
        }


        public int? CurrentBall(ScoreBoard_VO VO)
        {
            if (dl != null)
            {
                return dl.CurrentBall(VO);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.CurrentBall(VO);
            }
        }
        public int? PreviousBall(ScoreBoard_VO VO)
        {
            if (dl != null)
            {
                return dl.PreviousBall(VO);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.PreviousBall(VO);
            }
        }


        public List<BATTING_VO> STRIKER(string ID)
        {
            dl = new Cricket_DL();
            using(DataTable table = dl.STRIKER(ID))
            {
                List<BATTING_VO> list = new List<BATTING_VO>();
                BATTING_VO vo;

                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            TotalRuns = Convert.ToString(row["RUNS"]),
                            BatsmanName = name.ToString(),
                        };

                        if (vo.BytePicture != null)
                        {
                            vo.BatsmanPicture = Convert.ToBase64String(vo.BytePicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                    name = null;
                }
            }
        }
        public List<BATTING_VO> NON_STRIKER(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.NON_STRIKER(ID))
            {
                List<BATTING_VO> list = new List<BATTING_VO>();
                BATTING_VO vo;

                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            TotalRuns = Convert.ToString(row["RUNS"]),
                            BatsmanName = name.ToString(),
                        };

                        if (vo.BytePicture != null)
                        {
                            vo.BatsmanPicture = Convert.ToBase64String(vo.BytePicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                    name = null;
                }
            }
        }
        public List<BATTING_VO> BOWLER(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLER(ID))
            {
                List<BATTING_VO> list = new List<BATTING_VO>();
                BATTING_VO vo;

                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BatsmanName = name.ToString(),
                        };

                        if (vo.BytePicture != null)
                        {
                            vo.BatsmanPicture = Convert.ToBase64String(vo.BytePicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                    name = null;
                }
            }
        }


        public List<BATTING_VO> BATTING_SCOREBOARD_1(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BATTING_SCOREBOARD_1(ID))
            {
                List<BATTING_VO> details = new List<BATTING_VO>();
                BATTING_VO bat;
                object name;
                object bowler;
                object fielder;
                object final = null;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        //Batsman Name
                        if (string.IsNullOrEmpty(row["BATSMAN MIDDLE NAME"].ToString()))
                        {
                            name = row["BATSMAN FIRST NAME"].ToString() + " " + row["BATSMAN LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["BATSMAN FIRST NAME"].ToString() + " " + row["BATSMAN MIDDLE NAME"].ToString() + " " + row["BATSMAN LAST NAME"].ToString();
                        }

                        //Bowler Name
                        if (!string.IsNullOrEmpty(row["BOWLER FIRST NAME"].ToString()))
                        {
                            if (string.IsNullOrEmpty(row["BOWLER MIDDLE NAME"].ToString()))
                            {
                                bowler = row["BOWLER FIRST NAME"].ToString() + " " + row["BOWLER LAST NAME"].ToString();
                            }
                            else
                            {
                                bowler = row["BOWLER FIRST NAME"].ToString() + " " + row["BOWLER MIDDLE NAME"].ToString() + " " + row["BOWLER LAST NAME"].ToString();
                            }
                        }
                        else
                        {
                            bowler = "";
                        }

                        //Fielder Name
                        if (!string.IsNullOrEmpty(row["FIELDER FIRST NAME"].ToString()))
                        {
                            if (string.IsNullOrEmpty(row["FIELDER MIDDLE NAME"].ToString()))
                            {
                                fielder = row["FIELDER FIRST NAME"].ToString() + " " + row["FIELDER LAST NAME"].ToString();
                            }
                            else
                            {
                                fielder = row["FIELDER FIRST NAME"].ToString() + " " + row["FIELDER MIDDLE NAME"].ToString() + " " + row["FIELDER LAST NAME"].ToString();
                            }
                        }
                        else
                        {
                            fielder = "";
                        }

                        //final Bowler Changes
                        if (!string.IsNullOrEmpty(row["WICKET TYPE"].ToString()))
                        {
                            //CAUGHT
                            if (row["WICKET TYPE"].ToString() == "1" && !string.IsNullOrEmpty(bowler.ToString().Trim()) && !string.IsNullOrEmpty(fielder.ToString().Trim()))
                            {
                                final = "c " + fielder + " b " + bowler; 
                            }
                            //BOWLED
                            else if(row["WICKET TYPE"].ToString() == "2" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "b " + bowler;
                            }
                            //RUN-OUT
                            else if(row["WICKET TYPE"].ToString() == "3" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "run out (" + bowler + ")";
                            }
                            //LEG-BEFORE-WICKET
                            else if(row["WICKET TYPE"].ToString() == "4" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "lbw b " + bowler;
                            }
                            //STUMPED
                            else if(row["WICKET TYPE"].ToString() == "5" && !string.IsNullOrEmpty(bowler.ToString().Trim()) && !string.IsNullOrEmpty(fielder.ToString().Trim()))
                            {
                                final = "st " + fielder + " b " + bowler;
                            }
                            //HIT-WICKET
                            else if(row["WICKET TYPE"].ToString() == "6" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "hit wicket b " + bowler;
                            }
                        }
                        else if(string.IsNullOrEmpty(row["WICKET TYPE"].ToString()) && string.IsNullOrEmpty(bowler.ToString().Trim()))
                        {
                            final = "";
                        }


                        bat = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BatsmanName = name.ToString().Trim(),
                            BowlerName = final.ToString().Trim(),
                            TotalRuns = row["RUNS"] != DBNull.Value ? row["RUNS"].ToString() : "0",
                            TotalBalls = row["BALLS"] != DBNull.Value ? row["BALLS"].ToString() : "0",
                            FOUR = row["FOUR"] != DBNull.Value ? row["FOUR"].ToString() : "0",
                            SIX = row["SIX"] != DBNull.Value ? row["SIX"].ToString() : "0",
                            StrikeRate = row["STRIKE RATE"] != DBNull.Value ? (decimal?)Math.Round(Convert.ToDecimal(row["STRIKE RATE"]), 5) : 0
                        };

                        if (bat.BytePicture != null)
                        {
                            bat.BatsmanPicture = Convert.ToBase64String(bat.BytePicture);
                        }

                        details.Add(bat);
                        name = null;
                        bowler = null;
                        fielder = null;
                        final = null;
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    bat = null;
                    name = null;
                    bowler = null;
                    fielder = null;
                    final = null;
                }
            }
        }
        public List<BATTING_VO> BATTING_SCOREBOARD_2(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BATTING_SCOREBOARD_2(ID))
            {
                List<BATTING_VO> details = new List<BATTING_VO>();
                BATTING_VO bat;
                object name;
                object bowler;
                object fielder;
                object final = null;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        //Batsman Name
                        if (string.IsNullOrEmpty(row["BATSMAN MIDDLE NAME"].ToString()))
                        {
                            name = row["BATSMAN FIRST NAME"].ToString() + " " + row["BATSMAN LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["BATSMAN FIRST NAME"].ToString() + " " + row["BATSMAN MIDDLE NAME"].ToString() + " " + row["BATSMAN LAST NAME"].ToString();
                        }

                        //Bowler Name
                        if (!string.IsNullOrEmpty(row["BOWLER FIRST NAME"].ToString()))
                        {
                            if (string.IsNullOrEmpty(row["BOWLER MIDDLE NAME"].ToString()))
                            {
                                bowler = row["BOWLER FIRST NAME"].ToString() + " " + row["BOWLER LAST NAME"].ToString();
                            }
                            else
                            {
                                bowler = row["BOWLER FIRST NAME"].ToString() + " " + row["BOWLER MIDDLE NAME"].ToString() + " " + row["BOWLER LAST NAME"].ToString();
                            }
                        }
                        else
                        {
                            bowler = "";
                        }

                        //Fielder Name
                        if (!string.IsNullOrEmpty(row["FIELDER FIRST NAME"].ToString()))
                        {
                            if (string.IsNullOrEmpty(row["FIELDER MIDDLE NAME"].ToString()))
                            {
                                fielder = row["FIELDER FIRST NAME"].ToString() + " " + row["FIELDER LAST NAME"].ToString();
                            }
                            else
                            {
                                fielder = row["FIELDER FIRST NAME"].ToString() + " " + row["FIELDER MIDDLE NAME"].ToString() + " " + row["FIELDER LAST NAME"].ToString();
                            }
                        }
                        else
                        {
                            fielder = "";
                        }

                        //final Bowler Changes
                        if (!string.IsNullOrEmpty(row["WICKET TYPE"].ToString()))
                        {
                            //CAUGHT
                            if (row["WICKET TYPE"].ToString() == "1" && !string.IsNullOrEmpty(bowler.ToString().Trim()) && !string.IsNullOrEmpty(fielder.ToString().Trim()))
                            {
                                final = "c " + fielder + " b " + bowler;
                            }
                            //BOWLED
                            else if (row["WICKET TYPE"].ToString() == "2" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "b " + bowler;
                            }
                            //RUN-OUT
                            else if (row["WICKET TYPE"].ToString() == "3" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "run out (" + bowler + ")";
                            }
                            //LEG-BEFORE-WICKET
                            else if (row["WICKET TYPE"].ToString() == "4" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "lbw b " + bowler;
                            }
                            //STUMPED
                            else if (row["WICKET TYPE"].ToString() == "5" && !string.IsNullOrEmpty(bowler.ToString().Trim()) && !string.IsNullOrEmpty(fielder.ToString().Trim()))
                            {
                                final = "st " + fielder + " b " + bowler;
                            }
                            //HIT-WICKET
                            else if (row["WICKET TYPE"].ToString() == "6" && !string.IsNullOrEmpty(bowler.ToString().Trim()))
                            {
                                final = "hit wicket b " + bowler;
                            }
                        }
                        else if (string.IsNullOrEmpty(row["WICKET TYPE"].ToString()) && string.IsNullOrEmpty(bowler.ToString().Trim()))
                        {
                            final = "";
                        }


                        bat = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BatsmanName = name.ToString().Trim(),
                            BowlerName = final.ToString().Trim(),
                            TotalRuns = row["RUNS"] != DBNull.Value ? row["RUNS"].ToString() : "0",
                            TotalBalls = row["BALLS"] != DBNull.Value ? row["BALLS"].ToString() : "0",
                            FOUR = row["FOUR"] != DBNull.Value ? row["FOUR"].ToString() : "0",
                            SIX = row["SIX"] != DBNull.Value ? row["SIX"].ToString() : "0",
                            StrikeRate = row["STRIKE RATE"] != DBNull.Value ? (decimal?)Math.Round(Convert.ToDecimal(row["STRIKE RATE"]), 5) : 0
                        };

                        if (bat.BytePicture != null)
                        {
                            bat.BatsmanPicture = Convert.ToBase64String(bat.BytePicture);
                        }

                        details.Add(bat);
                        name = null;
                        bowler = null;
                        fielder = null;
                        final = null;
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    bat = null;
                    name = null;
                    bowler = null;
                    fielder = null;
                    final = null;
                }
            }
        }
        public List<BOWLING_VO> BOWLING_SCOREBOARD_1(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLING_SCOREBOARD_1(ID))
            {
                List<BOWLING_VO> list = new List<BOWLING_VO>();
                BOWLING_VO vo;
                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BOWLING_VO()
                        {
                            BytesPicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BowlerName = name.ToString().Trim(),
                            BallCount = row["BALL NUMBER"] != DBNull.Value ? Convert.ToInt32(row["BALL NUMBER"]) : (int?)0,
                            MaidenOvers = Convert.ToInt32(row["MAIDEN OVERS"]),
                            RunsConceded = row["RUNS CONCEDED"] != DBNull.Value ? Convert.ToInt32(row["RUNS CONCEDED"]) : (int?)0,
                            WicketsTaken = row["WICKETS TAKEN"] != DBNull.Value ? Convert.ToInt32(row["WICKETS TAKEN"]) : (int?)0,
                            EconomyRate = (row["BALL NUMBER"] != DBNull.Value && Convert.ToInt32(row["BALL NUMBER"]) > 0)? (row["RUNS CONCEDED"] != DBNull.Value ? (float)Convert.ToInt32(row["RUNS CONCEDED"]) / (Convert.ToInt32(row["BALL NUMBER"]) / 6.0f) : 0): 0
                        };

                        if (vo.BytesPicture != null)
                        {
                            vo.BowlerPicture = Convert.ToBase64String(vo.BytesPicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                }
            }
        }
        public List<BOWLING_VO> BOWLING_SCOREBOARD_2(string ID)
        {
            dl = new Cricket_DL();
            using (DataTable table = dl.BOWLING_SCOREBOARD_2(ID))
            {
                List<BOWLING_VO> list = new List<BOWLING_VO>();
                BOWLING_VO vo;
                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BOWLING_VO()
                        {
                            BytesPicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BowlerName = name.ToString().Trim(),
                            BallCount = row["BALL NUMBER"] != DBNull.Value ? Convert.ToInt32(row["BALL NUMBER"]) : (int?)null,
                            MaidenOvers = Convert.ToInt32(row["MAIDEN OVERS"]),
                            RunsConceded = row["RUNS CONCEDED"] != DBNull.Value ? Convert.ToInt32(row["RUNS CONCEDED"]) : (int?)null,
                            WicketsTaken = row["WICKETS TAKEN"] != DBNull.Value ? Convert.ToInt32(row["WICKETS TAKEN"]) : (int?)null,
                            EconomyRate = (row["BALL NUMBER"] != DBNull.Value && Convert.ToInt32(row["BALL NUMBER"]) > 0) ? (row["RUNS CONCEDED"] != DBNull.Value ? (float)Convert.ToInt32(row["RUNS CONCEDED"]) / (Convert.ToInt32(row["BALL NUMBER"]) / 6.0f) : 0) : 0
                        };

                        if (vo.BytesPicture != null)
                        {
                            vo.BowlerPicture = Convert.ToBase64String(vo.BytesPicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                }
            }
        }


        public List<BATTING_VO> HALF_SCOREBOARD(string ID)
        {
            dl = new Cricket_DL();

            using (DataTable table = dl.HALF_SCOREBOARD(ID))
            {
                List<BATTING_VO> details = new List<BATTING_VO>();

                BATTING_VO bat;
                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        bat = new BATTING_VO()
                        {
                            BytePicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BatsmanName = name.ToString(),
                            TotalRuns = row["RUNS"].ToString(),
                            TotalBalls = row["BALLS"] != DBNull.Value ? row["BALLS"].ToString() : null,
                            FOUR = row["FOUR"] != DBNull.Value ? row["FOUR"].ToString() : null,
                            SIX = row["SIX"] != DBNull.Value ? row["SIX"].ToString() : null,
                            StrikeRate = row["STRIKE RATE"] != DBNull.Value ? (decimal?)Math.Round(Convert.ToDecimal(row["STRIKE RATE"]), 3) : null
                        };

                        if (bat.BytePicture != null)
                        {
                            bat.BatsmanPicture = Convert.ToBase64String(bat.BytePicture);
                        }

                        details.Add(bat);
                    }
                    return details;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    details = null;
                    name = null;
                    bat = null;
                }
            }
        }
        public List<BOWLING_VO> HALF_BOWLING(string ID)
        {
            dl = new Cricket_DL();

            using(DataTable table = dl.HALF_BOWLING(ID))
            {
                List<BOWLING_VO> list = new List<BOWLING_VO>();
                BOWLING_VO vo;

                object name;

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (string.IsNullOrEmpty(row["MIDDLE NAME"].ToString()))
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }
                        else
                        {
                            name = row["FIRST NAME"].ToString() + " " + row["MIDDLE NAME"].ToString() + " " + row["LAST NAME"].ToString();
                        }

                        vo = new BOWLING_VO()
                        {
                            BytesPicture = row["PROFILE PICTURE"] != DBNull.Value ? (byte[])row["PROFILE PICTURE"] : null,
                            BowlerName = name.ToString(),
                            BallCount = Convert.ToInt32(row["BALL NUMBER"]),
                            RunsConceded = Convert.ToInt32(row["RUNS CONCEDED"]),
                            WicketsTaken = Convert.ToInt32(row["WICKETS TAKEN"]),
                            EconomyRate = Convert.ToInt32(row["BALL NUMBER"]) > 0 ? (float)Convert.ToInt32(row["RUNS CONCEDED"]) / (Convert.ToInt32(row["BALL NUMBER"]) / 6.0f): 0
                        };

                        if (vo.BytesPicture != null)
                        {
                            vo.BowlerPicture = Convert.ToBase64String(vo.BytesPicture);
                        }

                        list.Add(vo);
                    }
                    return list;
                }
                finally
                {
                    dl = null;
                    table.Clear();
                    table.Dispose();
                    list = null;
                    vo = null;
                    name = null;
                }
            }
        }
        public List<string> COMMENTARY(string ID)
        {
            if(dl != null)
            {
                return dl.COMMENTARY(ID);
            }
            else
            {
                dl = new Cricket_DL();
                return dl.COMMENTARY(ID);
            }
        }


        //Live Updation


        /// <summary>
        /// Updating Ball Count if the Player Scored 0 Runs
        /// </summary>
        /// <param name="VO"></param>
        public void UpdatingBallCount(ScoreBoard_VO VO)
        {
            dl = new Cricket_DL();
            dl.UpdatingBallCount(VO);

            if(dl != null)
            {
                dl = null;
            }
        }
        /// <summary>
        /// Inserting New Ball record
        /// </summary>
        /// <param name="VO"></param>
        public void InsertEachBallNewRecord(ScoreBoard_VO VO)
        {
            dl = new Cricket_DL();
            dl.InsertEachBallNewRecord(VO);

            if(dl != null)
            {
                    dl = null;
            }
        }
        /// <summary>
        /// Swapping Striker & Non Striker Positions after each over
        /// </summary>
        /// <param name="Vo"></param>
        public void ChangingStrikerNonStriker(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.ChangingStrikerNonStriker(Vo);

            if (dl != null)
            {
                dl = null;
            }
        }

        /// <summary>
        /// Updating 4 Runs
        /// </summary>
        /// <param name="Vo"></param>
        public void Updating4RunsCount(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.Updating4RunsCount(Vo);

            if (dl != null)
            {
                 dl = null;
            }
        }
        /// <summary>
        /// Updating 6 Runs
        /// </summary>
        /// <param name="Vo"></param>
        public void Updating6RunsCount(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.Updating6RunsCount(Vo);

            if( dl != null)
            {
                dl = null;
            }
        }
        /// <summary>
        /// Handling Special Balls Case
        /// </summary>
        /// <param name="Vo"></param>
        public void SpecialBallHandling(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.SpecialBallHandling(Vo);

            if ( dl != null )
            {
                dl = null;
            }
        }
        /// <summary>
        /// Adding New Striker if the previous gets OUT
        /// </summary>
        /// <param name="Vo"></param>
        public void NewStrikerAdding(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NewStrikerAdding(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }




        public void NEWBOWLER_STRIKER(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NEWBOWLER_STRIKER(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }
        public void NEWBOWLER_BOWLER(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NEWBOWLER_BOWLER(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }
        public void NEWBOWLER_RUNS_CASE1(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NEWBOWLER_RUNS_CASE1(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }

        public void NEWBOWLER_EXTRACASE_STRIKER_BOWLER(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NEWBOWLER_EXTRACASE_STRIKER_BOWLER(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }
        public void NEWBOWLER_EXTRAS_CASE2(ScoreBoard_VO Vo)
        {
            dl = new Cricket_DL();
            dl.NEWBOWLER_EXTRAS_CASE2(Vo);

            if(dl != null)
            {
                dl = null;
            }
        }

        public void NEWBOWLER_NEWSTRIKER_CASE3_PART1(ScoreBoard_VO Vo)
        {
            if(dl != null)
            {
                dl.NEWBOWLER_NEWSTRIKER_CASE3_PART1(Vo);
            }
            else
            {
                dl = new Cricket_DL();
                dl.NEWBOWLER_NEWSTRIKER_CASE3_PART1(Vo);
            }
        }
        public void NEWBOWLER_NEWSTRIKER_CASE3_PART2(ScoreBoard_VO Vo)
        {
            if (dl != null)
            {
                dl.NEWBOWLER_NEWSTRIKER_CASE3_PART2(Vo);
            }
            else
            {
                dl = new Cricket_DL();
                dl.NEWBOWLER_NEWSTRIKER_CASE3_PART2(Vo);
            }
        }

        public void COMMENTARY_RUNS_CASE1(ScoreBoard_VO Vo)
        {
            if(dl != null)
            {
                dl.COMMENTARY_RUNS_CASE1(Vo);
            }
            else
            {
                dl = new Cricket_DL();
                dl.COMMENTARY_RUNS_CASE1(Vo);
            }
        }
    }
}