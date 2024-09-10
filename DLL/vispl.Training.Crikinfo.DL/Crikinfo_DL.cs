using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Vispl.Training.Crickinfo.VO;


namespace Vispl.Training.Crickinfo.DL
{
    /// <summary>
    /// Data Layer
    /// </summary>
    public class Cricket_DL: Interface_Cricket_DL,IDisposable
    {

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                connectionString = null;
            }
        }
        ~Cricket_DL()
        {
            Dispose(false);
        }

        private static string FlatFile()
        {
            using (Login_VO vo = new Login_VO())
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(vo.FlatFile);
                XmlNode usersNode = doc.SelectSingleNode("/users");

                if (usersNode != null)
                {
                    for (int i = 0; i < usersNode.ChildNodes.Count; i++)
                    {
                        XmlNode newNode = usersNode.ChildNodes[i];

                        if (newNode.Name == "connectionStrings")
                        {
                            XmlNode childNode = newNode.SelectSingleNode("connectionString");
                            string connection = childNode.InnerText;
                            vo.Dispose();
                            return connection;
                        }
                    }
                }
            }
            return null;
        }
        private string connectionString;


        public Cricket_DL()
        {
            connectionString = FlatFile();
        }
        

        /// <summary>
        /// Verifying Login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object isValidLogin(string user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT [PASSWORD] FROM [Logins] WHERE [USERNAME] = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Username", user);
                    object value = command.ExecuteScalar();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    return value;
                }
            }
        }


        /// <summary>
        /// Getting Count
        /// </summary>
        /// <returns></returns>
        public object TotalPlayers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [CricketTable]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object players = command.ExecuteScalar();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    return players;
                }
            }
        }


        /// <summary>
        /// Total Teams Count
        /// </summary>
        /// <returns></returns>
        public object TotalTeams()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [TEAM DETAILS]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object teams = command.ExecuteScalar();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    return teams;
                }
            }
        }


        /// <summary>
        /// Total Macth Count
        /// </summary>
        /// <returns></returns>
        public object TotalMatches()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [MatchDetails]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object matches = command.ExecuteScalar();

                    connection.Close();
                    connection.Dispose();
                    query = null;
                    command.Dispose();
                    return matches;
                }
            }

        }


        /// <summary>
        /// Adding Details
        /// </summary>
        /// <param name="vo"></param>
        public void addingPlayerDetails(Player_VO vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT into [CricketTable] ([PROFILE PICTURE], [PLAYER ID], [JERSEY NUMBER], [FIRST NAME], [MIDDLE NAME], [LAST NAME], [DOB], [AGE], [BIRTH PLACE], [CAPTAIN], [SUBSTITUTE], [FRANCHISE], [MATCHES PLAYED], [RUNS SCORED], [WICKETS TAKEN], [CENTURIES], [HALF CENTURIES], [DEBUT DATE], [ICC RANKING], [COUNTRY], [POSITION])
                VALUES(@Picture, @Player_ID, @JerseyNumber, @FirstName, @MiddleName, @LastName, @DOB, @Age, @BirthPlace, @Captain, @Substitute, @Franchise, @MatchesPlayed, @RunsScored, @WicketsTaken, @Centuries, @HalfCenturies, @DebutDate, @ICCRanking, @Country, @Position)
                SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string formattedDate = vo.DOB?.ToString("yyyy-MM-dd");
                    string formattedDebutDate = vo.DebutDate?.ToString("yyyy-MM-dd");

                    command.Parameters.AddWithValue("@Player_ID", (object)vo.Franchise ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Franchise", (object)vo.Franchise ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DOB", formattedDate);
                    command.Parameters.AddWithValue("@DebutDate", formattedDebutDate);
                    command.Parameters.AddWithValue("@Picture", (object)vo.ProfilePicture ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JerseyNumber", vo.Jersey.Trim());
                    command.Parameters.AddWithValue("@FirstName", vo.FirstName.Trim());
                    command.Parameters.AddWithValue("@MiddleName", (object)vo.MiddleName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", vo.LastName.Trim());
                    command.Parameters.AddWithValue("@Age", vo.Age);
                    command.Parameters.AddWithValue("@BirthPlace", vo.BirthPlace.Trim());
                    command.Parameters.AddWithValue("@Captain", vo.IsCaptain);
                    command.Parameters.AddWithValue("@Substitute", vo.IsSubstitute);
                    command.Parameters.AddWithValue("@MatchesPlayed", vo.MatchesPlayed);
                    command.Parameters.AddWithValue("@RunsScored", vo.RunsScored);
                    command.Parameters.AddWithValue("@WicketsTaken", vo.WicketsTaken);
                    command.Parameters.AddWithValue("@Centuries", vo.Centuries);
                    command.Parameters.AddWithValue("@HalfCenturies", vo.HalfCenturies);
                    command.Parameters.AddWithValue("@ICCRanking", vo.ICCRanking);
                    command.Parameters.AddWithValue("@Country", vo.Country);
                    command.Parameters.AddWithValue("@Position", vo.Position);

                    connection.Open();
                    vo.Player_ID = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Parameters.Clear();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding Player in TEAM PLAYERS Table seprately
        /// </summary>
        /// <param name="vo"></param>
        public void UpdatingTeamWithPlayers(Player_VO vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [TEAM PLAYERS] ([TEAM ID], [PLAYER ID])
                                VALUES (@TeamId, @PlayerId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@TeamId", Convert.ToInt32(vo.Franchise));
                    command.Parameters.AddWithValue("@PlayerId", vo.Player_ID);
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    
                }
            }
        }


        /// <summary>
        /// Adding Team Details
        /// </summary>
        /// <param name="vo"></param>
        public void addingTeamDetails(Team_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [TEAM DETAILS] ([TEAM_ICON], [TEAM NAME], [TEAM SHORT NAME], [TEAM CAPTAIN], [TEAM VICE CAPTAIN], [TEAM WICKET KEEPER])
                            VALUES(@TeamIcon, @TeamName, @TeamShortName, @TeamCaptain, @TeamViceCaptain, @TeamWicketKeeper)
                            SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@TeamIcon", (object)vo.TeamIcon ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TeamName", vo.TeamName.Trim());
                    command.Parameters.AddWithValue("@TeamShortName", vo.TeamShortName.Trim());
                    command.Parameters.AddWithValue("@TeamCaptain", (object)vo.TeamCaptain ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TeamViceCaptain", (object)vo.TeamViceCaptain ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TeamWicketKeeper", (object)vo.TeamKeeper ?? DBNull.Value);
                    connection.Open();
                    vo.TeamID = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding Players to the TEAM PLAYERS table seprately
        /// </summary>
        /// <param name="vo"></param>
        public void addingTeamPlayers(Team_VO vo)
        {
            using( SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Insert Into [TEAM PLAYERS] ([TEAM ID], [PLAYER ID]) 
                                    VALUES(@TeamID, @PlayerID)";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    connection.Open();

                    for (int i = 0; i < vo.TeamMembers.Count; i++)
                    {
                        command.Parameters.AddWithValue("@TeamID", vo.TeamID);
                        command.Parameters.AddWithValue("@PlayerID", vo.TeamMembers[i]);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Updating Player Details
        /// </summary>
        /// <param name="vo"></param>
        public void UpdatingFranchise(Team_VO vo)
        {
            if (vo.TeamMembers.Count > 0 && vo.TeamMembers != null && vo.TeamID != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE [CricketTable] 
                                    SET [PLAYER ID] = @TeamId, [FRANCHISE] = @TeamId
                                    WHERE [ID] = @PlayerId;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        for (int i = 0; i < vo.TeamMembers.Count; i++)
                        {
                            command.Parameters.AddWithValue("@TeamId", vo.TeamID);
                            command.Parameters.AddWithValue("@PlayerId", vo.TeamMembers[i]);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query = null;
                    }
                }
            }
        }


        /// <summary>
        /// Adding Match Details
        /// </summary>
        /// <param name="vo"></param>
        public void addingMatchDetails(Match_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
               
                string query = @"INSERT into [MatchDetails] ([Team_A], [Team_B], [MATCH_VENUE], [MATCH_TYPE], [MATCH_SCHEDULE])
                                VALUES(@TeamA, @TeamB, @MatchVenue, @MatchType, @Time)";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    if(vo.MatchSchedule != null && vo.MatchScheduleOffset != null)
                    {
                        DateTimeOffset matchDate;
                        string offsetString = vo.MatchScheduleOffset;
                        if (offsetString.StartsWith("+"))
                        {
                            offsetString = offsetString.Substring(1);
                        }

                        TimeSpan offsetValue = TimeSpan.Parse(offsetString);
                        matchDate = new DateTimeOffset(vo.MatchSchedule.Value, offsetValue);
                        command.Parameters.AddWithValue("@Time", matchDate);
                    }

                    command.Parameters.AddWithValue("@TeamA", (object)vo.Team_A);
                    command.Parameters.AddWithValue("@TeamB", (object)vo.Team_B);
                    command.Parameters.AddWithValue("@MatchVenue", (object)vo.MatchVenue);
                    command.Parameters.AddWithValue("@MatchType", (object)vo.MatchType);

                    connection.Open();
                    command.ExecuteNonQuery();


                    connection.Close();
                    connection.Dispose();
                    command.Parameters.Clear();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding TOSS Details
        /// </summary>
        /// <param name="vo"></param>
        public void addingTossDetails(Toss_Decision_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT into [TOSS] ([MATCH_ID], [TOSS_WON_BY], [TOSS_LOST_BY], [DECISION])
                                VALUES (@MatchID, @Won, @Lost, @Decision)
                                SELECT SCOPE_IDENTITY();";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@MatchID", vo.MatchID);
                    command.Parameters.AddWithValue("@Won", vo.TossWonBy);
                    command.Parameters.AddWithValue("@Lost", vo.TossLostBy);
                    command.Parameters.AddWithValue("@Decision", vo.Decision);

                    connection.Open();
                    vo.TossID = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding Batting & Bolwing Playing 11 to Batting Table
        /// </summary>
        /// <param name="vo"></param>
        public void addingBattingData(SelectPlayers_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query1 = @"INSERT INTO [BATTING] ([MATCH ID], [TEAM ID], [STRIKER PLAYER ID])
                                VALUES (@Match, @Team, @Striker);";
                string query2 = @"INSERT INTO [BATTING] ([MATCH ID], [TEAM ID], [STRIKER PLAYER ID])
                                VALUES (@Match, @Team, @Striker)";

                connection.Open();

                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    for(int i=0; i < vo.BattingPlayers.Count; i++)
                    {
                        command.Parameters.AddWithValue("@Match", vo.MatchID);
                        command.Parameters.AddWithValue("@Team", vo.BattingTeamID);
                        command.Parameters.AddWithValue("@Striker", vo.BattingPlayers[i]);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    command.Dispose();
                }
                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    for(int i = 0; i < vo.BowlingPLayers.Count; i++)
                    {
                        command.Parameters.AddWithValue("@Match", vo.MatchID);
                        command.Parameters.AddWithValue("@Team", vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@Striker", vo.BowlingPLayers[i]);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    command.Dispose();
                }

                connection.Close();
                connection.Dispose();
                query1 = null;
                query2 = null;
            }
        }


        /// <summary>
        /// Adding Bowling Data
        /// </summary>
        /// <param name="vo"></param>
        public void addingBowlingData(SelectPlayers_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID], [CURRENTLY BOWLING])
                                VALUES (@MatchId, @teamId, @BowlerId, 1)";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@MatchId",vo.MatchID);
                    command.Parameters.AddWithValue("@teamId",vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@BowlerId",vo.Bowler);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding Each Ball Data
        /// </summary>
        /// <param name="vo"></param>
        public void addingEachBallData(SelectPlayers_VO vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [EACH BALL] ([MATCH ID], [TEAM ID], [BOWLER ID], [STRIKER ID])
                                VALUES (@MatchId, @teamId, @BowlerId, @Striker)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", vo.MatchID);
                    command.Parameters.AddWithValue("@teamId", vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@BowlerId", vo.Bowler);
                    command.Parameters.AddWithValue("@Striker", vo.Striker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Updating Captains
        /// </summary>
        /// <param name="vo"></param>
        public void UpdatingCaptains(SelectPlayers_VO vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query1 = @"UPDATE [FINAL DECISION]
                                SET [BATTING TEAM CAPTAIN] = @BattCap, [BOWLING TEAM CAPTAIN] = @BowlCap
                                WHERE [MATCH ID] = @Id AND [BATTING TEAM] = @BatId AND [BOWLING TEAM] = @BowlId";

                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@BattCap", vo.BattingTeamCaptain);
                    command.Parameters.AddWithValue("@BowlCap", vo.BallingTeamCaptain);
                    command.Parameters.AddWithValue("@Id", vo.MatchID);
                    command.Parameters.AddWithValue("@BatId", vo.BattingTeamID);
                    command.Parameters.AddWithValue("@BowlId", vo.BowlingTeamID);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query1 = null;
                }
            }
        }


        /// <summary>
        /// Updating Non Striker
        /// </summary>
        /// <param name="vo"></param>
        public void UpdatingNonStriker(SelectPlayers_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BATTING]
                                SET [NON STRIKER PLAYER ID] = @NonStrikerId
                                WHERE [MATCH ID] = @MatchId AND [TEAM ID] = @TeamId AND [STRIKER PLAYER ID] = @StrikerId AND [OUT] IS NULL";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@NonStrikerId", vo.NonStriker);
                    command.Parameters.AddWithValue("StrikerId", vo.Striker);
                    command.Parameters.AddWithValue("MatchId", vo.MatchID);
                    command.Parameters.AddWithValue("TeamId", vo.BattingTeamID);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Updating Match Status
        /// </summary>
        /// <param name="vo"></param>
        public void UpdatingMatchStatus(SelectPlayers_VO vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [MatchDetails]
                                SET [MATCH_STATUS] = @Status
                                WHERE [ID] = @Id";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Status", "Live");
                    command.Parameters.AddWithValue("@Id", vo.MatchID);
                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding All Batting & Bowling Team Final Details
        /// </summary>
        /// <param name="vo"></param>
        public int addingFinalDecison(Toss_Decision_VO vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Insert Into [FINAL DECISION] ([MATCH ID], [TOSS ID], [BATTING TEAM], [BOWLING TEAM])
                                VALUES (@Match, @Toss, @Batt, @Bowl)
                                SELECT SCOPE_IDENTITY();";
                int? battingTeam = null;
                int? bowlingTeam = null;
                int value;

                if (vo.Decision == "0")
                {
                    battingTeam = vo.TossWonBy;
                    bowlingTeam = vo.TossLostBy;
                }
                else if (vo.Decision == "1")
                {
                    bowlingTeam = vo.TossWonBy;
                    battingTeam = vo.TossLostBy;
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Match", vo.MatchID);
                    command.Parameters.AddWithValue("@Toss", vo.TossID);
                    command.Parameters.AddWithValue("@Batt", battingTeam);
                    command.Parameters.AddWithValue("@Bowl", bowlingTeam);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    battingTeam = null;
                    bowlingTeam = null;

                    return value;
                }
            }
        }


        /// <summary>
        /// Player Names List
        /// </summary>
        /// <param name="vo"></param>
        public DataTable Players()
        {
            string query = @"SELECT [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] FROM [CricketTable] WHERE [PLAYER ID] is NULL";
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        table.Dispose();
                        adapter.Dispose();
                        return table;
                    }
                }
                
            }
        }


        /// <summary>
        /// Teams List
        /// </summary>
        /// <returns></returns>
        public DataTable Teams()
        {
            string query = @"SELECT [TEAM ID], [TEAM NAME] FROM [TEAM DETAILS] ";
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        table.Dispose();
                        adapter.Dispose();
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Player Details for View
        /// </summary>
        public DataTable GetPlayerData()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        c.[PROFILE PICTURE],
                                        c.[JERSEY NUMBER],
                                        c.[FIRST NAME],
                                        c.[MIDDLE NAME],
                                        c.[LAST NAME],
                                        c.[DOB],
                                        c.[AGE],
                                        c.[BIRTH PLACE],
                                        c.[CAPTAIN],
                                        c.[SUBSTITUTE],
                                        ISNULL(cd.[TEAM NAME], ' - ') as [FRANCHISE],
                                        c.[MATCHES PLAYED],
                                        c.[RUNS SCORED],
                                        c.[WICKETS TAKEN],
                                        c.[CENTURIES],
                                        c.[HALF CENTURIES],
                                        c.[DEBUT DATE],
                                        c.[ICC RANKING],
                                        ct.[country] AS [COUNTRY],
                                        c.[POSITION]
                                    FROM [CricketTable] c
                                    INNER JOIN [Countries] ct
                                    ON c.[COUNTRY] = ct.[ID]
                                    LEFT JOIN [TEAM DETAILS] cd ON c.[FRANCHISE] = cd.[TEAM ID]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        details.Dispose();
                        query = null;
                        return details;   
                    }
                }
            }
        }


        /// <summary>
        /// Team Details For View
        /// </summary>
        /// <returns></returns>
        public DataTable GetTeamData()
        {
            using(DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        td.[TEAM_ICON],
                                        ct.[FIRST NAME] as [FIRST NAME],
                                        ct.[MIDDLE NAME] as [MIDDLE NAME],
                                        ct.[LAST NAME] as [LAST NAME],
                                        td.[TEAM NAME],
                                        td.[TEAM SHORT NAME],
                                        td.[TEAM CAPTAIN],
                                        td.[TEAM VICE CAPTAIN],
                                        td.[TEAM WICKET KEEPER]
                                    FROM 
                                        [TEAM DETAILS] td
                                    LEFT JOIN 
                                        [TEAM PLAYERS] tp ON td.[TEAM ID] = tp.[TEAM ID]
                                    LEFT JOIN 
                                        [CricketTable] ct ON tp.[PLAYER ID] = ct.[ID];";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Match Details For View
        /// </summary>
        /// <returns></returns>
        public DataTable GetMatchData()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select m.[ID], ct.[TEAM NAME] as [TEAM_A], cd.[TEAM NAME] as [TEAM_B], mv.[VENUE] as [MATCH_VENUE], mt.[TYPE] as [MATCH_TYPE], m.[MATCH_SCHEDULE]
                                    From MatchDetails m
                                    Inner Join [TEAM DETAILS] ct
                                    on m.[TEAM_A] = ct.[TEAM ID]
                                    Inner Join [TEAM DETAILS] cd
                                    on m.[TEAM_B] = cd.[TEAM ID]
                                    Inner Join [MATCH VENUE] mv
                                    on m.[MATCH_VENUE] = mv.ID
                                    Inner Join [MATCH TYPE] mt
                                    on m.[MATCH_TYPE] = mt.ID";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        query = null;
                        adapter.Dispose();
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Filtered Match Data for View
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredData(MatchFilter_VO VO)
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT teamA.[TEAM NAME] as [TEAM_A], teamB.[TEAM NAME] as [TEAM_B], venue.[VENUE] as [MATCH_VENUE], typ.[TYPE] as [MATCH_TYPE], matc.[MATCH_STATUS] as [MATCH_STATUS], matc.[MATCH_SCHEDULE] as [MATCH_SCHEDULE] 
                                    FROM [MatchDetails] matc
                                    Inner Join [MATCH VENUE] venue on matc.[MATCH_VENUE] = venue.[ID]
                                    Inner Join [MATCH TYPE] typ on matc.[MATCH_TYPE] = typ.ID
                                    Inner Join [TEAM DETAILS] teamA on matc.[TEAM_A] = teamA.[TEAM ID]
                                    Inner Join [TEAM DETAILS] teamB on matc.[TEAM_B] = teamB.[TEAM ID]
                                    WHERE [MATCH_SCHEDULE] 
                                    BETWEEN @MatchStartDate AND @MatchEndDate";

                    DateTimeOffset matchStartDate;
                    DateTimeOffset matchEndDate;

                    string offsetString1 = VO.MatchFromOffset;
                    string offsetString2 = VO.MatchToOffset;

                    if (offsetString1.StartsWith("+"))
                    {
                        offsetString1 = offsetString1.Substring(1);
                    }
                    if (offsetString2.StartsWith("+"))
                    {
                        offsetString2 = offsetString2.Substring(1);
                    }

                    TimeSpan offsetValue1 = TimeSpan.Parse(offsetString1);
                    TimeSpan offsetValue2 = TimeSpan.Parse(offsetString2);

                    matchStartDate = new DateTimeOffset(VO.MatchFrom.Value, offsetValue1);
                    matchEndDate = new DateTimeOffset(VO.MatchTo.Value, offsetValue2);

                    
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@MatchStartDate", matchStartDate);
                        adapter.SelectCommand.Parameters.AddWithValue("@MatchEndDate", matchEndDate);

                        connection.Open();
                        adapter.Fill(details);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        details.Dispose();
                        query = null;
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Countries
        /// </summary>
        /// <returns></returns>
        public DataTable Countries()
        {
            using(DataTable table = new DataTable())
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [country] FROM [Countries]";

                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        query = null;
                        adapter.Dispose();
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Getting Match Necessary Fields
        /// </summary>
        /// <returns></returns>
        public DataTable MatchResult()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [RESULT] From [MATCH RESULT]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Getting Match Status
        /// </summary>
        /// <returns></returns>
        public DataTable MatchStatus()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [STATUS] FROM [MATCH STATUS]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        query = null;
                        adapter.Dispose();
                        return details;
                    }
                }
            }
        }


        /// <summary>
        ///Getting Match Type 
        /// </summary>
        /// <returns></returns>
        public DataTable MatchType()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [TYPE] FROM [MATCH TYPE]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        query = null;
                        adapter.Dispose();
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Getting Match Venue
        /// </summary>
        /// <returns></returns>
        public DataTable MatchVenue()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [VENUE] FROM [MATCH VENUE]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Getting Time Zone Offset Values
        /// </summary>
        /// <returns></returns>
        public DataTable TimeZoneOffset()
        {
            using (DataTable details = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ZONE], [OFFSET] FROM [TIME ZONES]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        connection.Open();
                        adapter.Fill(details);

                        details.Dispose();
                        connection.Close();
                        connection.Dispose();
                        query = null;
                        adapter.Dispose();
                        return details;
                    }
                }
            }
        }


        /// <summary>
        /// Getting Types of OUT
        /// </summary>
        /// <returns></returns>
        public DataTable Out()
        {
            using(DataTable table = new DataTable())
            {
                using (DataTable details = new DataTable())
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = @"SELECT [ID], [out] FROM [OUT]";

                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                        {
                            connection.Open();
                            adapter.Fill(details);

                            details.Dispose();
                            connection.Close();
                            connection.Dispose();
                            adapter.Dispose();
                            query = null;
                            return details;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Details For Particular Match
        /// </summary>
        /// <returns></returns>
        public DataTable MatchData(string ID)
        {
            using(DataTable table = new DataTable())
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select mv.[VENUE] as [MATCH_VENUE], mt.[TYPE] as [MATCH_TYPE], td.[TEAM NAME] as [TEAM_A], tt.[TEAM NAME] as [TEAM_B], m.[MATCH_SCHEDULE]
                                    FROM [MatchDetails] m
                                    Inner Join [TEAM DETAILS] td
                                    on m.[TEAM_A] = td.[TEAM ID]
                                    Inner Join [TEAM DETAILS] tt
                                    on m.[TEAM_B] = tt.[TEAM ID]
                                    Inner Join [MATCH VENUE] mv
                                    on m.[MATCH_VENUE] = mv.ID
                                    Inner Join [MATCH TYPE] mt
                                    on m.[MATCH_TYPE] = mt.ID
                                    WHERE m.[ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Finding Team IDS for the Live Match
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (object TeamA, object TeamB) TeamData(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [TEAM_A], [TEAM_B] From [MatchDetails] WHERE [ID] = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                            connection.Open();
                            adapter.Fill(table);

                            connection.Close();
                            connection.Dispose();
                            command.Dispose();
                            adapter.Dispose();

                            DataRow row = table.Rows[0];
                            object teamA = row["TEAM_A"];
                            object teamB = row["TEAM_B"];

                            table.Clear();
                            table.Dispose();
                            query = null;


                            if(teamA != null && teamB != null)
                            {
                                return (teamA, teamB);
                            }
                            else
                            {
                                return (null, null);
                            }
                        }
                    }

                }
            }
        }


        /// <summary>
        /// Team Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TeamA_LiveData(string ID)
        {
            using(DataTable table = new DataTable())
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (TeamA, TeamB) = TeamData(ID);
                    string id = TeamA.ToString();

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable TeamB_LiveData(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (TeamA, TeamB) = TeamData(ID);
                    string id = TeamB.ToString();

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Finding Players of both teams
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TeamA_Players(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [PROFILE PICTURE], [ID], [JERSEY NUMBER], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id";

                    var (TeamA, TeamB) = TeamData(ID);
                    string id = TeamA.ToString();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable TeamB_Players(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [PROFILE PICTURE], [ID], [JERSEY NUMBER], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id";

                    var (TeamA, TeamB) = TeamData(ID);
                    string id = TeamB.ToString();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Generating MatchID, BattingTEAM, BowlingTEAM
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? BattingTeam, int? BowlingTeam, int? MatchID) TossData(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [MATCH ID], [BATTING TEAM], [BOWLING TEAM] FROM [FINAL DECISION] Where [ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();

                        DataRow row = table.Rows[0];
                        int? match = Convert.ToInt32(row["MATCH ID"]);
                        int? bater = Convert.ToInt32(row["BATTING TEAM"]);
                        int? bowler = Convert.ToInt32(row["BOWLING TEAM"]);

                        table.Clear();
                        table.Dispose();
                        query = null;

                        if (match != null && bater != null && bowler != null)
                        {
                            return (match, bater, bowler);
                        }
                        else
                        {
                            return (null, null, null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Match Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable TOSS_MATCH_DATA(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"Select mv.[VENUE] as [MATCH_VENUE], mt.[TYPE] as [MATCH_TYPE], td.[TEAM NAME] as [TEAM_A], tt.[TEAM NAME] as [TEAM_B], m.[MATCH_SCHEDULE], m.[ID] as [ID]
                                    FROM [MatchDetails] m
                                    Inner Join [TEAM DETAILS] td
                                    on m.[TEAM_A] = td.[TEAM ID]
                                    Inner Join [TEAM DETAILS] tt
                                    on m.[TEAM_B] = tt.[TEAM ID]
                                    Inner Join [MATCH VENUE] mv
                                    on m.[MATCH_VENUE] = mv.ID
                                    Inner Join [MATCH TYPE] mt
                                    on m.[MATCH_TYPE] = mt.ID
                                    WHERE m.[ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Toss Finalised Batting & Bowling Teams Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable BATTING_TEAM(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BattingID;

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM NAME], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_TEAM(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BowlingID;

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM NAME], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Toss Finalised Batting & Bowling Players
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable BATTING_PLAYERS (string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id";

                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BattingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;
                        BattingID = null;
                        BowlingID = null;

                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_PLAYERS(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id;";

                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BowlingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;

                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Only Batting Players
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable BATSMAN(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id AND [POSITION] Like '%BATSMAN%';";

                    var (MatchId, BattingID, BowlingID) = TossData(ID);
                    int? id = BattingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;

                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Only Captains Dropdown
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable BATTING_CAPTAIN(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id AND [CAPTAIN] LIKE '%YES%'";

                    var (MatchId, BattingID, BowlingID) = TossData(ID);
                    int? id = BattingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;

                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_CAPTAIN(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id AND [CAPTAIN] LIKE '%YES%'";

                    var (MatchId, BattingID, BowlingID) = TossData(ID);
                    int? id = BowlingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;

                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Finding Batting Team & Bowling Team using MatchID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? BattingID, int? BowlingID) BattingBowlingIDS(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [BATTING TEAM], [BOWLING TEAM] FROM [FINAL DECISION] Where [MATCH ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();

                        DataRow row = table.Rows[0];
                        int? bater = Convert.ToInt32(row["BATTING TEAM"]);
                        int? bowler = Convert.ToInt32(row["BOWLING TEAM"]);

                        table.Clear();
                        table.Dispose();
                        query = null;

                        if (bater != null && bowler != null)
                        {
                            return (bater, bowler);
                        }
                        else
                        {
                            return (null, null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Players That can Bat
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable NOT_OUT_BATTING_PLAYERS(string ID)
        {
            using(DataTable table = new DataTable())
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                                    SELECT player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME], player.[ID]
                                    FROM [BATTING] AS b
                                    JOIN [CricketTable] AS player ON player.[ID] = b.[STRIKER PLAYER ID]
                                    WHERE b.[OUT] IS NULL AND b.[NON STRIKER PLAYER ID] IS NULL AND [TEAM ID] = @Id AND [MATCH ID] = @match;";

                    var(bat,bowl) = BattingBowlingIDS(ID);
                    int? battingID = bat;

                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", battingID);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;

                        return table;
                    }
                }
            }
        }
        public DataTable LIVE_MATCH_BOWLERS(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select [ID], [FIRST NAME], [MIDDLE NAME], [LAST NAME] From [CricketTable] Where [PLAYER ID] = @Id AND [POSITION] Like '%BALLER%';";

                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BowlingID;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        id = null;

                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// Toss Finalised Batting & Bowling Teams
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable BATTING_TEAM_LIVE(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BattingID;

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM NAME], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_TEAM_LIVE(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BowlingID;

                    string query = @"SELECT [TEAM ID], [TEAM_ICON], [TEAM NAME], [TEAM SHORT NAME] FROM [TEAM DETAILS] Where [TEAM ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }


        /// <summary>
        /// STRIKER, NON STRIKER, TEAM ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? Striker, int? NonStriker, int? BattingTeamID) LiveMatchDetailsForBattingTeam(string ID)
        {
            using(DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [TEAM ID], [STRIKER PLAYER ID], [NON STRIKER PLAYER ID] FROM [BATTING]
                                WHERE [NON STRIKER PLAYER ID] IS NOT NULL
                                AND [MATCH ID] = @Id
                                AND OUT IS NULL";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);
                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();

                        DataRow row = table.Rows[0];
                        int? striker = Convert.ToInt32(row["STRIKER PLAYER ID"]);
                        int? nonStriker = Convert.ToInt32(row["NON STRIKER PLAYER ID"]);
                        int? battingTeamID = Convert.ToInt32(row["TEAM ID"]);

                        table.Clear();
                        table.Dispose();
                        query = null;

                        if(striker != null && nonStriker != null &&  battingTeamID != null)
                        {
                            return (striker, nonStriker, battingTeamID);
                        }
                        else
                        {
                            return (null, null, null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// BOWLER, BOWLING TEAM ID, CURRENT BALL NUMBER
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? Bowler, int? BowlingTeam, int? CurrentBall) LiveMatchDetailsForBowlingTeam(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT TOP 1 [BOWLER ID], [TEAM ID], [BALL NUMBER] FROM [BOWLING]
                                    WHERE [MATCH ID] = @Id AND [CURRENTLY BOWLING] = 1
                                    ORDER BY [BALL NUMBER] DESC";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();

                        DataRow row = table.Rows[0];
                        int? bowler = Convert.ToInt32(row["BOWLER ID"]);
                        int?  bowlingTeamID = Convert.ToInt32(row["TEAM ID"]);
                        int? ballNumber = Convert.ToInt32(row["BALL NUMBER"]);

                        table.Clear();
                        table.Dispose();
                        query = null;

                        if(bowler != null && bowlingTeamID != null && ballNumber != null)
                        {
                            return(bowler, bowlingTeamID, ballNumber);
                        }
                        else
                        {
                            return (null, null, null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// For Keeping the each value unique
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int? IsLive(string ID)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 1 FROM [FINAL DECISION] WHERE [MATCH ID] = @Id";
                int? value = null;
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Id", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;

                    return value;
                }
            }
        }


        /// <summary>
        /// Count of Extras
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        SUM(CASE WHEN [EXTRA TYPE] = 1 THEN [EXTRA RUNS] ELSE 0 END) AS NO_BALL,
                                        SUM(CASE WHEN [EXTRA TYPE] = 2 THEN [EXTRA RUNS] ELSE 0 END) AS WIDE_BALL,
                                        SUM(CASE WHEN [EXTRA TYPE] = 3 THEN [EXTRA RUNS] ELSE 0 END) AS BYE,
                                        SUM(CASE WHEN [EXTRA TYPE] = 4 THEN [EXTRA RUNS] ELSE 0 END) AS LEG_BYE,
                                        SUM(CASE WHEN [EXTRA TYPE] = 5 THEN [EXTRA RUNS] ELSE 0 END) AS PENALTY
                                    FROM
                                        [EACH BALL]
	                                    Where [MATCH ID] = @match AND [TEAM ID] = @team;";

                    var (bat, bowl) = BattingBowlingIDS(ID);

                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        adapter.SelectCommand.Parameters.AddWithValue("@team", bowl);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        bat = null;
                        bowl = null;

                        DataRow row = table.Rows[0];
                        int? noBall = row["NO_BALL"] != DBNull.Value ? Convert.ToInt32(row["NO_BALL"]) : 0;
                        int? wideBall = row["WIDE_BALL"] != DBNull.Value ? Convert.ToInt32(row["WIDE_BALL"]) : 0;
                        int? bye = row["BYE"] != DBNull.Value ? Convert.ToInt32(row["BYE"]) : 0;
                        int? legBye = row["LEG_BYE"] != DBNull.Value ? Convert.ToInt32(row["LEG_BYE"]) : 0;
                        int? penalty = row["PENALTY"] != DBNull.Value ? Convert.ToInt32(row["PENALTY"]) : 0;


                        table.Clear();
                        table.Dispose();

                        return (noBall, wideBall, bye, legBye, penalty);
                    }
                }
            }
        }
        public (int? noball, int? wide, int? bye, int? legBye, int? penalty) ExtrasCount_2(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                            SUM(CASE WHEN [EXTRA TYPE] = 1 THEN [EXTRA RUNS] ELSE 0 END) AS NO_BALL,
                                            SUM(CASE WHEN [EXTRA TYPE] = 2 THEN [EXTRA RUNS] ELSE 0 END) AS WIDE_BALL,
                                            SUM(CASE WHEN [EXTRA TYPE] = 3 THEN [EXTRA RUNS] ELSE 0 END) AS BYE,
                                            SUM(CASE WHEN [EXTRA TYPE] = 4 THEN [EXTRA RUNS] ELSE 0 END) AS LEG_BYE,
                                            SUM(CASE WHEN [EXTRA TYPE] = 5 THEN [EXTRA RUNS] ELSE 0 END) AS PENALTY
                                        FROM 
                                            [EACH BALL]
	                                        Where [MATCH ID] = @match AND [TEAM ID] = @team;";

                    var (bat, bowl) = BattingBowlingIDS(ID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        adapter.SelectCommand.Parameters.AddWithValue("@team", bat);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        bat = null;
                        bowl = null;

                        DataRow row = table.Rows[0];
                        int? noBall = row["NO_BALL"] != DBNull.Value ? Convert.ToInt32(row["NO_BALL"]) : 0;
                        int? wideBall = row["WIDE_BALL"] != DBNull.Value ? Convert.ToInt32(row["WIDE_BALL"]) : 0;
                        int? bye = row["BYE"] != DBNull.Value ? Convert.ToInt32(row["BYE"]) : 0;
                        int? legBye = row["LEG_BYE"] != DBNull.Value ? Convert.ToInt32(row["LEG_BYE"]) : 0;
                        int? penalty = row["PENALTY"] != DBNull.Value ? Convert.ToInt32(row["PENALTY"]) : 0;

                        table.Clear();
                        table.Dispose();

                        return (noBall, wideBall, bye, legBye, penalty);
                    }
                }
            }
        }


        public DataTable BATTING_SCOREBOARD_1(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    string query = @"SELECT 
                                        player.[PROFILE PICTURE], 
                                        player.[FIRST NAME] as [BATSMAN FIRST NAME], 
                                        player.[MIDDLE NAME] as [BATSMAN MIDDLE NAME], 
                                        player.[LAST NAME] as [BATSMAN LAST NAME], 
                                        bat.[RUNS], 
                                        bat.[BALLS], 
                                        bat.[FOUR], 
                                        bat.[SIX], 
                                        bat.[STRIKE RATE],
                                        wicket_type.[wicketTypeId] AS [WICKET TYPE],
                                        bowler.[FIRST NAME] AS [BOWLER FIRST NAME],
                                        bowler.[MIDDLE NAME] AS [BOWLER MIDDLE NAME], 
                                        bowler.[LAST NAME] AS [BOWLER LAST NAME],
                                        fielder.[FIRST NAME] AS [FIELDER FIRST NAME], 
                                        fielder.[MIDDLE NAME] AS [FIELDER MIDDLE NAME], 
                                        fielder.[LAST NAME] AS [FIELDER LAST NAME]
                                    FROM 
                                        [BATTING] AS bat
                                    INNER JOIN 
                                        [CricketTable] AS player ON bat.[STRIKER PLAYER ID] = player.[ID]
                                    LEFT JOIN 
                                        [fallOfWicket] AS wicket ON bat.[OUT] = wicket.[wicketId]
                                    LEFT JOIN 
                                        [CricketTable] AS bowler ON wicket.[bowlerId] = bowler.[ID]
                                    LEFT JOIN 
                                        [CricketTable] AS fielder ON wicket.[fielderId] = fielder.[ID]
                                    LEFT JOIN 
                                        [wicketType] AS wicket_type ON wicket.[wicketTypeId] = wicket_type.[wicketTypeId]
                                    WHERE 
                                        bat.[TEAM ID] = @Id AND bat.[MATCH ID] = @match;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", BattingID);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BATTING_SCOREBOARD_2(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        player.[PROFILE PICTURE], 
                                        player.[FIRST NAME] as [BATSMAN FIRST NAME], 
                                        player.[MIDDLE NAME] as [BATSMAN MIDDLE NAME], 
                                        player.[LAST NAME] as [BATSMAN LAST NAME], 
                                        bat.[RUNS], 
                                        bat.[BALLS], 
                                        bat.[FOUR], 
                                        bat.[SIX], 
                                        bat.[STRIKE RATE],
                                        wicket_type.[wicketTypeId] AS [WICKET TYPE],
                                        bowler.[FIRST NAME] AS [BOWLER FIRST NAME],
                                        bowler.[MIDDLE NAME] AS [BOWLER MIDDLE NAME], 
                                        bowler.[LAST NAME] AS [BOWLER LAST NAME],
                                        fielder.[FIRST NAME] AS [FIELDER FIRST NAME], 
                                        fielder.[MIDDLE NAME] AS [FIELDER MIDDLE NAME], 
                                        fielder.[LAST NAME] AS [FIELDER LAST NAME]
                                    FROM 
                                        [BATTING] AS bat
                                    INNER JOIN 
                                        [CricketTable] AS player ON bat.[STRIKER PLAYER ID] = player.[ID]
                                    LEFT JOIN 
                                        [fallOfWicket] AS wicket ON bat.[OUT] = wicket.[wicketId]
                                    LEFT JOIN 
                                        [CricketTable] AS bowler ON wicket.[bowlerId] = bowler.[ID]
                                    LEFT JOIN 
                                        [CricketTable] AS fielder ON wicket.[fielderId] = fielder.[ID]
                                    LEFT JOIN 
                                        [wicketType] AS wicket_type ON wicket.[wicketTypeId] = wicket_type.[wicketTypeId]
                                    WHERE 
                                        bat.[TEAM ID] = @Id AND bat.[MATCH ID] = @match;";
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", BowlingID);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_SCOREBOARD_1(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                    player.[PROFILE PICTURE], 
                                    player.[FIRST NAME], 
                                    player.[MIDDLE NAME], 
                                    player.[LAST NAME], 
                                    bowl.[BALL NUMBER], 
                                    bowl.[RUNS CONCEDED], 
                                    bowl.[WICKETS TAKEN],
                                    maidenOversQuery.MaidenOvers as [MAIDEN OVERS]
                                FROM 
                                    [BOWLING] bowl
                                INNER JOIN 
                                    [CricketTable] player ON bowl.[BOWLER ID] = player.[ID]
                                LEFT JOIN (
                                    SELECT 
                                        [BOWLER ID],
                                        COUNT(CASE WHEN MaidenOver = 1 THEN 1 END) AS MaidenOvers
                                    FROM (
                                        SELECT 
                                            [BOWLER ID],
                                            FLOOR(([BALL NUMBER] - 1) / 6) AS OverNumber,
                                            CASE WHEN COUNT(CASE WHEN [RUNS CONCEDED] = 0 THEN 1 END) = 6 THEN 1 ELSE 0 END AS MaidenOver
                                        FROM 
                                            [EmployeeData].[dbo].[EACH BALL]
                                        WHERE 
                                            [TEAM ID] = @team
                                            AND [MATCH ID] = @match
                                        GROUP BY 
                                            [BOWLER ID], FLOOR(([BALL NUMBER] - 1) / 6)
                                    ) AS MaidenOversQuery
                                    GROUP BY 
                                        [BOWLER ID]
                                ) AS maidenOversQuery ON bowl.[BOWLER ID] = maidenOversQuery.[BOWLER ID]
                                WHERE 
                                    bowl.[TEAM ID] = @team
                                    AND [MATCH ID] = @match;";
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@team", BattingID);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        BattingID = null;
                        BowlingID = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BOWLING_SCOREBOARD_2(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                    player.[PROFILE PICTURE], 
                                    player.[FIRST NAME], 
                                    player.[MIDDLE NAME], 
                                    player.[LAST NAME], 
                                    bowl.[BALL NUMBER], 
                                    bowl.[RUNS CONCEDED], 
                                    bowl.[WICKETS TAKEN],
                                    maidenOversQuery.MaidenOvers as [MAIDEN OVERS]
                                FROM 
                                    [BOWLING] bowl
                                INNER JOIN 
                                    [CricketTable] player ON bowl.[BOWLER ID] = player.[ID]
                                LEFT JOIN (
                                    SELECT 
                                        [BOWLER ID],
                                        COUNT(CASE WHEN MaidenOver = 1 THEN 1 END) AS MaidenOvers
                                    FROM (
                                        SELECT 
                                            [BOWLER ID],
                                            FLOOR(([BALL NUMBER] - 1) / 6) AS OverNumber,
                                            CASE WHEN COUNT(CASE WHEN [RUNS CONCEDED] = 0 THEN 1 END) = 6 THEN 1 ELSE 0 END AS MaidenOver
                                        FROM 
                                            [EmployeeData].[dbo].[EACH BALL]
                                        WHERE 
                                            [TEAM ID] = @team
                                            AND [MATCH ID] = @match
                                        GROUP BY 
                                            [BOWLER ID], FLOOR(([BALL NUMBER] - 1) / 6)
                                    ) AS MaidenOversQuery
                                    GROUP BY 
                                        [BOWLER ID]
                                ) AS maidenOversQuery ON bowl.[BOWLER ID] = maidenOversQuery.[BOWLER ID]
                                WHERE 
                                    bowl.[TEAM ID] = @team
                                    AND [MATCH ID] = @match;";
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@team", BowlingID);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        BattingID = null;
                        BowlingID = null;
                        return table;
                    }
                }
            }
        }


        public DataTable HALF_SCOREBOARD(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var(striker,nonStriker,bat) = LiveMatchDetailsForBattingTeam(ID);

                    string query = @"Select player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME], bat.[RUNS], bat.[BALLS], bat.[FOUR], bat.[SIX], bat.[STRIKE RATE]
                                    From [BATTING] bat
                                    Inner Join [CricketTable] player On bat.[STRIKER PLAYER ID] = player.ID
                                    WHERE bat.[TEAM ID] = @team 
                                    AND bat.[MATCH ID] = @match 
                                    AND (bat.[STRIKER PLAYER ID] = @Striker OR bat.[STRIKER PLAYER ID] = @NonStriker)";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@team", bat);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        adapter.SelectCommand.Parameters.AddWithValue("@Striker", striker);
                        adapter.SelectCommand.Parameters.AddWithValue("@NonStriker", nonStriker);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        striker = null;
                        nonStriker = null;
                        bat = null;

                        return table;
                    }
                }
            }
        }
        public DataTable HALF_BOWLING(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (bat, bowl) = BattingBowlingIDS(ID);

                    string query = @"Select player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME], bat.[BALL NUMBER], bat.[RUNS CONCEDED], bat.[WICKETS TAKEN]
                                    From [BOWLING] bat
                                    Inner Join [CricketTable] player On
                                    bat.[BOWLER ID] = player.ID
                                    WHERE bat.[TEAM ID] = @team AND [MATCH ID] = @match AND bat.[CURRENTLY BOWLING] = 1;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@team", bowl);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        table.Dispose();
                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        bat = null;
                        bowl = null;

                        return table;
                    }
                }
            }
        }


        public int? RunsCount(string ID)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT SUM([RUNS] + [EXTRAS]) AS TotalRuns
                                FROM [BATTING]
                                WHERE [TEAM ID] = @Id AND [MATCH ID] = @match;";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? id = BattingID;
                int? value = null;

                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    id = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }
        public int? OutsCount(string ID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Select COUNT (*) from [BATTING] 
                                WHERE [OUT] is NOT NULL and [TEAM ID] = @Id AND [MATCH ID] = @match";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? id = BattingID;
                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    id = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }
        public int? BallCount(string ID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 [BALL NUMBER]
                                FROM [EACH BALL]
                                WHERE [TEAM ID] = @Id AND [MATCH ID] = @match
                                ORDER BY [BALL NUMBER] DESC;";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? id = BowlingID;
                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    id = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }
        public int? RunsCount_2(string ID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT SUM([RUNS] + [EXTRAS]) AS TotalRuns
                                FROM [BATTING]
                                WHERE [TEAM ID] = @Id AND [MATCH ID] = @match;";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", BowlingID);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }
        public int? OutsCount_2(string ID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Select COUNT (*) from [BATTING] 
                                WHERE [OUT] is NOT NULL and [TEAM ID] = @Id AND [MATCH ID] = @match";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", BowlingID);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }
        public int? BallCount_2(string ID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 [BALL NUMBER]
                                FROM [EACH BALL]
                                WHERE [TEAM ID] = @Id AND [MATCH ID] = @match
                                ORDER BY [BALL NUMBER] DESC;";

                var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", BattingID);
                    command.Parameters.AddWithValue("@match", ID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    BattingID = null;
                    BowlingID = null;

                    return value;
                }
            }
        }

        public DataTable STRIKER(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"Select player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME], bat.[RUNS] 
                                        From [CricketTable] as player
                                        Inner Join [BATTING] as bat On bat.[STRIKER PLAYER ID] = player.[ID]
                                        Where bat.[STRIKER PLAYER ID] IS NOT NULL and bat.[NON STRIKER PLAYER ID] IS NOT NULL AND bat.[OUT] is NULL and bat.[MATCH ID] = @Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        return table;
                    }
                }
            }
        }
        public DataTable NON_STRIKER(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"Select player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME], bat.[RUNS]
                                        From [CricketTable] as player
                                        Inner Join [BATTING] as bat On bat.[STRIKER PLAYER ID] = player.[ID]
                                        Where bat.[STRIKER PLAYER ID] = @non and bat.[NON STRIKER PLAYER ID] IS NULL AND bat.[OUT] is NULL and bat.[MATCH ID] = @Id";

                    var (striker, non, team) = LiveMatchDetailsForBattingTeam(ID);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@non", non);
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        striker = null;
                        non = null;
                        team = null;
                        return table;
                    }
                }
            }
        }
        public DataTable BOWLER(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BowlingID;

                    string query = @"Select TOP 1 player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME] 
                                        From [CricketTable] as player
                                        Inner Join [BOWLING] as bat On bat.[BOWLER ID] = player.[ID]
                                        Where bat.[MATCH ID] = @match AND bat.[TEAM ID] = @team AND bat.[CURRENTLY BOWLING] = 1
                                        Order by [BALL NUMBER] DESC";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        adapter.SelectCommand.Parameters.AddWithValue("@team", id);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        BattingID = null;
                        BowlingID = null;
                        id = null;
                        return table;
                    }
                }
            }
        }
        public List<string> COMMENTARY(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [COMMENT] FROM [COMMENTARY]
                                    WHERE [MATCH ID] = @match ORDER BY [ID] DESC";

                    using(SqlDataAdapter adapter = new SqlDataAdapter(query,connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);
                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                    }

                    string comment;
                    List<string> list = new List<string>();


                    foreach (DataRow row in table.Rows)
                    {
                        comment = row["COMMENT"].ToString();

                        list.Add(comment);
                    }

                    table.Clear();
                    table.Dispose();
                    comment = null;

                    return list;
                }
            }
        }

        /*public List<BATTING_VO> Striker(string ID)
        {
            using (DataTable table = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var (BattingID, BowlingID) = BattingBowlingIDS(ID);
                    int? id = BattingID;

                    string query = @"Select player.[PROFILE PICTURE], player.[FIRST NAME], player.[MIDDLE NAME], player.[LAST NAME]
                                    From [BATTING] bat
                                    Inner Join [CricketTable] player On
                                    bat.[STRIKER PLAYER ID] = player.ID
                                    WHERE bat.[TEAM ID] = @Id AND [MATCH ID] = @match AND [NON STRIKER PLAYER ID] IS NOT NULL AND [STRIKER PLAYER ID] is Not Null";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                        adapter.SelectCommand.Parameters.AddWithValue("@match", ID);

                        connection.Open();
                        adapter.Fill(table);

                        connection.Close();
                        connection.Dispose();
                        adapter.Dispose();
                        query = null;
                        
                    }
                }
            }
        }*/




        //Updating Live Score


        /// <summary>
        /// Updating Ball Count for 0 Runs
        /// </summary>
        /// <param name="Vo"></param>
        public void UpdatingBallCount(ScoreBoard_VO Vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BATTING]
                                SET [BALLS] = ISNULL([BALLS], 0) + 1, 
                                [RUNS] = ISNULL([RUNS],0) + @Runs
                                WHERE [MATCH ID] = @MatchId
                                AND [TEAM ID] = @TeamId
                                AND [STRIKER PLAYER ID] = @StrikerId
                                AND [NON STRIKER PLAYER ID] = @NonStrikerId;



                                UPDATE [BOWLING]
                                SET [BALL NUMBER] = ISNULL([BALL NUMBER], 0) + 1,
                                [RUNS CONCEDED] = ISNULL([RUNS CONCEDED], 0) + @Runs
                                WHERE [MATCH ID] = @MatchId
                                AND [TEAM ID] = @BowlingTeamId
                                AND [BOWLER ID] = @BowlerId;
";

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@MatchId", Vo.MatchID);
                    command.Parameters.AddWithValue("@Runs", Convert.ToInt32(Vo.Runs));
                    command.Parameters.AddWithValue("@TeamId", Vo.BattingTeamID);
                    command.Parameters.AddWithValue("@BowlingTeamId", Vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@StrikerId", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStrikerId", Vo.NonStriker);
                    command.Parameters.AddWithValue("@BowlerId", Vo.Bowler);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }
        /// <summary>
        /// Each Ball record
        /// </summary>
        /// <param name="VO"></param>
        public void InsertEachBallNewRecord(ScoreBoard_VO VO)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [RUNS CONCEDED])
                                VALUES (@Bowler, @Striker, @Match, @Team, @BallNumber, @Runs)";

                int? currentBall = CurrentBall(VO) + 1;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Bowler", VO.Bowler);
                    command.Parameters.AddWithValue("@Striker", VO.Striker);
                    command.Parameters.AddWithValue("@Match", VO.MatchID);
                    command.Parameters.AddWithValue("@Team", VO.BowlingTeamID);
                    command.Parameters.AddWithValue("@BallNumber", currentBall);
                    command.Parameters.AddWithValue("@Runs", VO.Runs);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    currentBall = null;
                }
            }
        }
        /// <summary>
        /// Finding Ball Number
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        public int? CurrentBall(ScoreBoard_VO VO)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 [BALL NUMBER] 
                                 FROM [EACH BALL] 
                                 WHERE [MATCH ID] = @MatchId AND [TEAM ID] = @TeamId
                                 ORDER BY [BALL NUMBER] DESC";

                int? value = null;
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", VO.MatchID);
                    command.Parameters.AddWithValue("@TeamId", VO.BowlingTeamID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;

                    return value;
                }
            }
        }
        /// <summary>
        /// Finding the Previous Ball
        /// </summary>
        /// <param name="Vo"></param>
        /// <returns></returns>
        public int? PreviousBall(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT [BALL NUMBER]
                                FROM (
                                    SELECT [BALL NUMBER], 
                                           ROW_NUMBER() OVER (ORDER BY [BALL NUMBER] DESC) AS RowNum
                                    FROM [EACH BALL]
                                    WHERE [MATCH ID] = @MatchId AND [TEAM ID] = @TeamId
                                ) AS sub
                                WHERE RowNum = 2;";

                int? value = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", Vo.MatchID);
                    command.Parameters.AddWithValue("@TeamId", Vo.BowlingTeamID);

                    connection.Open();
                    value = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;

                    return value;
                }
            }
        }


        /// <summary>
        /// NO-BALL Case-1
        /// If RUns Scored on No-Ball
        /// </summary>
        /// <param name="Vo"></param>
        public void NoBall_Case1(ScoreBoard_VO Vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [EXTRAS] ([MATCH ID], [STRIKER ID], [NON STRIKER ID], [BALL NUMBER], [IS NOBALL]
                                VALUES (@match, @striker, @nonStriker, @ball, 1));

                                Update Into [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID], [RUNS CONCEDED])";

                int? ball = CurrentBall(Vo) + 1;

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@match", Vo.Striker);
                    command.Parameters.AddWithValue("@match", Vo.NonStriker);
                    command.Parameters.AddWithValue("@match", ball);

                    connection.Open();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    ball = null;
                }
            }
        }


        /// <summary>
        /// Adding Commentary
        /// </summary>
        /// <param name="Vo"></param>
        public void COMMENTARY_RUNS_CASE1(ScoreBoard_VO Vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [COMMENTARY] ([MATCH ID], [BALL], [COMMENT])
                                VALUES (@match, @ball, @comment)";

                int? current = CurrentBall(Vo) + 1;

                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@ball", current);
                    command.Parameters.AddWithValue("@comment", Vo.Comment);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    current = null;
                }
            }
        }


        /// <summary>
        /// Updating 4 Runs Count for the Scorer
        /// </summary>
        /// <param name="Vo"></param>
        public void Updating4RunsCount(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BATTING]
                                SET [FOUR] = ISNULL([FOUR], 0) + 1
                                WHERE [MATCH ID] = @MatchId
                                AND [TEAM ID] = @TeamId
                                AND [STRIKER PLAYER ID] = @StrikerId
                                AND [NON STRIKER PLAYER ID] = @NonStrikerId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", Vo.MatchID);
                    command.Parameters.AddWithValue("@TeamId", Vo.BattingTeamID);
                    command.Parameters.AddWithValue("@StrikerId", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStrikerId", Vo.NonStriker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }
        /// <summary>
        /// Updating 6 Runs Count for the Striker
        /// </summary>
        /// <param name="Vo"></param>
        public void Updating6RunsCount(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BATTING]
                                SET [SIX] = ISNULL([SIX], 0) + 1
                                WHERE [MATCH ID] = @MatchId
                                AND [TEAM ID] = @TeamId
                                AND [STRIKER PLAYER ID] = @StrikerId
                                AND [NON STRIKER PLAYER ID] = @NonStrikerId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MatchId", Vo.MatchID);
                    command.Parameters.AddWithValue("@TeamId", Vo.BattingTeamID);
                    command.Parameters.AddWithValue("@StrikerId", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStrikerId", Vo.NonStriker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Changing Striker & Non-Striker Positions at the end of the over
        /// </summary>
        /// <param name="Vo"></param>
        public void ChangingStrikerNonStriker(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Update [BATTING] 
                                Set [NON STRIKER PLAYER ID]  = NULL
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;

                                Update [BATTING]
                                Set [NON STRIKER PLAYER ID] = @Striker
                                Where [STRIKER PLAYER ID] = @NonStriker AND [NON STRIKER PLAYER ID] is NULL AND [MATCH ID] = @match;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }
        public void AddingNewBowlerandBallDetails(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Insert into [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID])
                                VALUES (@match, @team, @bowler);

                                Insert Into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER])
                                Values (@bowler, @striker, @match, @team, @ball);";

                var (Bat, Bowl) = BattingBowlingIDS(Vo.MatchID.ToString());
                int? currentBall = CurrentBall(Vo) + 1;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@bowler", Vo.NewBowler);
                    command.Parameters.AddWithValue("@team", Bowl);
                    command.Parameters.AddWithValue("@ball", currentBall);
                    command.Parameters.AddWithValue("@striker", Vo.Striker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    Bat = null;
                    Bowl = null;
                }
            }
        }


        /// <summary>
        /// For Handing Extras
        /// </summary>
        /// <param name="Vo"></param>
        public void SpecialBallHandling(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Update [BATTING]
                                Set [EXTRAS] = [EXTRAS] + @Runs + 1
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;

                                Update [BOWLING] 
                                Set [EXTRA RUNS] = [EXTRA RUNS] + 1 + @Runs
                                Where [BOWLER ID] = @Bowler AND [MATCH ID] = @match;

                                Insert into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [RUNS CONCEDED], [BALL NUMBER], [EXTRA TYPE], [EXTRA RUNS])
                                Values (@Bowler, @Striker, @match, @team, @Runs, @balls, @Extra, @extraRuns)";

                int? current = CurrentBall(Vo);

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Runs", Convert.ToInt32(Vo.Runs));
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                    command.Parameters.AddWithValue("@Bowler", Vo.Bowler);
                    command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@Extra", Vo.SpecialBall);
                    command.Parameters.AddWithValue("@balls", current);
                    command.Parameters.AddWithValue("@extraRuns", (Convert.ToInt32(Vo.Runs) + 1));

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// For Handing Out Players
        /// </summary>
        /// <param name="Vo"></param>
        public void NewStrikerAdding(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Update [BATTING]
                                Set [OUT] = @Bowler, [NON STRIKER PLAYER ID] = NULL, [BALLS] = ISNULL([BALLS], 0) + 1
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;

                                Update [BOWLING]
                                Set [WICKETS TAKEN] = [WICKETS TAKEN] + 1
                                Where [BOWLER ID] = @Bowler AND [MATCH ID] = @match AND [TEAM ID] = @team;

                                Update [BATTING]
                                Set [NON STRIKER PLAYER ID] = @NonStriker
                                Where [STRIKER PLAYER ID] = @NewStriker AND [OUT] IS NULL AND [MATCH ID] = @match;

                                Insert into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [WICKET TYPE])
                                Values (@Bowler, @Striker, @match, @team, @ball, @wicket)";

                int? currentBall = CurrentBall(Vo) + 1;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Bowler", Vo.Bowler);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@NewStriker", Vo.NewStriker);
                    command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@ball", currentBall);
                    command.Parameters.AddWithValue("@wicket", Vo.Out);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// To Check For New Bowler Handling
        /// </summary>
        /// <param name="Vo"></param>
        /// <returns></returns>
        public int IS_BOWLER_PRESENT(ScoreBoard_VO Vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [BOWLING]
                                WHERE [BOWLER ID] = @Bowler AND [MATCH ID] = @match";
                int result;

                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Bowler",Vo.NewBowler);
                    command.Parameters.AddWithValue("@match",Vo.MatchID);

                    connection.Open();
                    object rows = command.ExecuteScalar();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;

                    result = rows != null ? Convert.ToInt32(rows) : 0;
                    return result;
                }
            }
        }

        

        /// <summary>
        /// For Adding Runs
        /// </summary>
        /// <param name="Vo"></param>
        public void NEWBOWLER_STRIKER(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Update [BATTING]
                                SET [BALLS] = ISNULL([BALLS], 0) + 1, [RUNS] = ISNULL([RUNS], 0) + @Runs
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Runs", Vo.Runs);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                    command.Parameters.AddWithValue("@match", Vo.MatchID);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }
        public void NEWBOWLER_BOWLER(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query1 = @"INSERT into [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID], [BALL NUMBER], [RUNS CONCEDED], [CURRENTLY BOWLING])
                                VALUES (@match, @team, @NewBowler, 1, @runs, @status);";

                string query2 = @"UPDATE [BOWLING]
                                SET [CURRENTLY BOWLING] = 1, [BALL NUMBER] = [BALL NUMBER] + 1, [RUNS CONCEDED] = [RUNS CONCEDED] + @Runs
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @NewBowler AND [CURRENTLY BOWLING] = 0 AND [TEAM ID] = @team;";

                if(IS_BOWLER_PRESENT(Vo) == 0)
                {
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@runs", Convert.ToInt32(Vo.Runs));
                        command.Parameters.AddWithValue("@status", 1);

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query1 = null;
                    }
                }
                else if(IS_BOWLER_PRESENT(Vo) == 1)
                {
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@runs", Convert.ToInt32(Vo.Runs));
                        command.Parameters.AddWithValue("@status", 1);

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query2 = null;
                    }
                }
            }
        }
        public void NEWBOWLER_EXTRACASE_STRIKER_BOWLER(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query1 = @"Update [BATTING]
                                SET [EXTRAS] = [EXTRAS] + @Runs + 1
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;

                                INSERT into [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID], [EXTRA RUNS], [CURRENTLY BOWLING])
                                VALUES (@match, @team, @NewBowler, @extraRuns, 1);";

                string query2 = @"Update [BATTING]
                                SET [EXTRAS] = [EXTRAS] + @Runs + 1
                                Where [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker AND [MATCH ID] = @match;

                                UPDATE [BOWLING]
                                SET [EXTRA RUNS] = [EXTRA RUNS] + @Runs + 1, [CURRENTLY BOWLING] = 1
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @NewBowler AND [CURRENTLY BOWLING] = 0 AND [TEAM ID] = @team;";

                if (IS_BOWLER_PRESENT(Vo) == 0)
                {
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@Striker", Vo.Striker);
                        command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@Runs", Convert.ToInt32(Vo.Runs));
                        command.Parameters.AddWithValue("@extraRuns", Convert.ToInt32(Vo.Runs) + 1);

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query1 = null;
                    }
                }
                else if (IS_BOWLER_PRESENT(Vo) == 1)
                {
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@Striker", Vo.Striker);
                        command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@Runs", Convert.ToInt32(Vo.Runs));

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query2 = null;
                    }
                }
            }
        }


        /// <summary>
        /// Adding New Bowler in Case of Runs only
        /// </summary>
        /// <param name="Vo"></param>
        public void NEWBOWLER_RUNS_CASE1(ScoreBoard_VO Vo)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BOWLING]
                                SET [CURRENTLY BOWLING] = 0
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @Bowler AND [TEAM ID] = @team;

                                INSERT into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [RUNS CONCEDED])
                                VALUES (@NewBowler, @Striker, @match, @team, @ball, @runs);";

                int? current = CurrentBall(Vo) + 1;

                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@Bowler", Vo.Bowler);
                    command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                    command.Parameters.AddWithValue("@status", 1);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@runs", Convert.ToInt32(Vo.Runs));
                    command.Parameters.AddWithValue("@ball", current);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// Adding New Bowler for EXTRAS case
        /// </summary>
        /// <param name="Vo"></param>
        public void NEWBOWLER_EXTRAS_CASE2(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BOWLING]
                                SET [CURRENTLY BOWLING] = 0
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @Bowler AND [TEAM ID] = @team;

                                INSERT into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [EXTRA TYPE], [EXTRA RUNS])
                                VALUES (@NewBowler, @Striker, @match, @team, @ball, @type, @extraRuns);";

                int? current = CurrentBall(Vo);

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@Bowler", Vo.Bowler);
                    command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                    command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@ball", current);
                    command.Parameters.AddWithValue("@type", Vo.SpecialBall);
                    command.Parameters.AddWithValue("@extraRuns", (Convert.ToInt32(Vo.Runs) + 1));

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                    current = null;
                }
            }
        }


        /// <summary>
        /// Setting Previous Striker to OUT
        /// </summary>
        /// <param name="Vo"></param>
        public void NEWBOWLER_NEWSTRIKER_CASE3_PART1(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [BATTING]
                                SET [OUT] = @NewBowler, [NON STRIKER PLAYER ID] = NULL, [BALLS] = ISNULL([BALLS], 0) + 1
                                WHERE [MATCH ID] = @match AND [STRIKER PLAYER ID] = @Striker AND [NON STRIKER PLAYER ID] = @NonStriker;

                                UPDATE [BATTING]
                                SET [NON STRIKER PLAYER ID] = @NonStriker
                                WHERE [MATCH ID] = @match AND [STRIKER PLAYER ID] = @NewStriker AND [NON STRIKER PLAYER ID] IS NULL;";

                int? current = CurrentBall(Vo) + 1;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                    command.Parameters.AddWithValue("@match", Vo.MatchID);
                    command.Parameters.AddWithValue("@Striker", Vo.Striker);
                    command.Parameters.AddWithValue("@NonStriker", Vo.NonStriker);
                    command.Parameters.AddWithValue("@NewStriker", Vo.NewStriker);

                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    query = null;
                }
            }
        }


        /// <summary>
        /// For Updating New Striker
        /// </summary>
        /// <param name="Vo"></param>
        public void NEWBOWLER_NEWSTRIKER_CASE3_PART2(ScoreBoard_VO Vo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query1 = @"UPDATE [BOWLING]
                                SET [CURRENTLY BOWLING] = 0
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @OldBowler AND [TEAM ID] = @team;

                                INSERT into [BOWLING] ([MATCH ID], [TEAM ID], [BOWLER ID], [BALL NUMBER], [CURRENTLY BOWLING])
                                VALUES (@match, @team, @NewBowler, 1 , 1);

                                INSERT into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [WICKET TYPE])
                                VALUES (@NewBowler, @OldStriker, @match, @team, @ball, @type);";

                string query2 = @"UPDATE [BOWLING]
                                SET [CURRENTLY BOWLING] = 0
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @OldBowler AND [TEAM ID] = @team;

                                UPDATE [BOWLING]
                                SET [BALL NUMBER] = [BALL NUMBER] + 1, [CURRENTLY BOWLING] = 1, [WICKETS TAKEN] = ISNULL([WICKETS TAKEN], 0) + 1
                                WHERE [MATCH ID] = @match AND [BOWLER ID] = @NewBowler AND [TEAM ID] = @team;

                                INSERT into [EACH BALL] ([BOWLER ID], [STRIKER ID], [MATCH ID], [TEAM ID], [BALL NUMBER], [WICKET TYPE])
                                VALUES (@NewBowler, @OldStriker, @match, @team, @ball, @type);";

                int? current = CurrentBall(Vo) + 1;

                if(IS_BOWLER_PRESENT(Vo) == 0)
                {
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@OldBowler", Vo.Bowler);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@OldStriker", Vo.Striker);
                        command.Parameters.AddWithValue("@ball", current);
                        command.Parameters.AddWithValue("@type", Vo.Out);

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query1 = null;
                        query2 = null;
                    }
                }
                else if(IS_BOWLER_PRESENT(Vo) == 1)
                {
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@match", Vo.MatchID);
                        command.Parameters.AddWithValue("@OldBowler", Vo.Bowler);
                        command.Parameters.AddWithValue("@team", Vo.BowlingTeamID);
                        command.Parameters.AddWithValue("@NewBowler", Vo.NewBowler);
                        command.Parameters.AddWithValue("@OldStriker", Vo.Striker);
                        command.Parameters.AddWithValue("@ball", current);
                        command.Parameters.AddWithValue("@type", Vo.Out);

                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                        connection.Dispose();
                        command.Dispose();
                        query1 = null;
                        query2 = null;
                    }
                }

                
            }
        }
    }
}