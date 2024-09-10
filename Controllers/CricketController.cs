using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Vispl.Training.Crickinfo.BM;
using Vispl.Training.Crickinfo.VO;

namespace Vispl.Training.Crickinfo.UI
{
    /// <summary>
    /// Cricket Scoreboard Controller
    /// </summary>
    [HandleError]
    public class CricketController : Controller
    {
        private ITeam_VO team;
        private ILogin_VO login;
        private IMatch_VO match;
        private IPlayer_VO player;
        private IMatchFilter_VO filter;
        private IToss_Decision_VO toss;
        private Interface_Cricket_BM bm;
        private ScoreBoard_VO scoreBoard;
        private ISelectPlayers_VO selectPlayers;


        /// <summary>
        /// GET: Login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                login = new Login_VO();
                return View(login);
            }
            finally
            {
                ViewData.Clear();
                ViewData = null;
                TempData.Clear();
                TempData = null;
                Session.Clear();

                Disposing();
            }
        }


        /// <summary>
        /// POST: Login
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Login_VO VO)
        {
            try
            {
                bm = new Cricket_BM();

                if (ModelState.IsValid)
                {
                    if (bm.savingCredentials(VO))
                    {
                        Session["User"] = VO.Username.ToLower();
                        return RedirectToAction("AdminPage", "Cricket");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Cricket");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                VO.Dispose();
                VO = null;
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                Disposing();
            }
        }


        /// <summary>
        /// User Main Page (Dashboard)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserPage()
        {
            Session["User"] = "user";

            if (Session["User"] as string == "user")
            {
                try
                {
                    bm = new Cricket_BM();

                    ViewData["PlayerDetails"] = bm.playerDetails();
                    ViewData["TeamDetails"] = bm.teamDetails();
                    ViewData["MatchDetails"] = bm.matchDetails();

                    return View();
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("Login", "Cricket");
            }
        }


        /// <summary>
        /// GET: ADMIN Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminPage()
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                try
                {
                    bm = new Cricket_BM();

                    ViewData["PlayerDetails"] = bm.playerDetails();
                    ViewData["TeamDetails"] = bm.teamDetails();
                    ViewData["MatchDetails"] = bm.matchDetails();
                    ViewData["PlayersCount"] = bm.TotalPlayers();
                    ViewData["MatchCount"] = bm.TotalMatches();
                    ViewData["TeamCount"] = bm.TotalTeams();

                    return View();
                }
                catch (Exception ex)
                {
                    DisposingAll();

                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("UserPage", "Cricket");
            }
        }


        /// <summary>
        /// GET: Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPlayer()
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                try
                {
                    bm = new Cricket_BM();
                    player = new Player_VO();

                    ViewData["Teams"] = bm.Teams();
                    ViewData["Countries"] = bm.Countries();
                    ViewData["Count"] = bm.TotalPlayers().ToString();

                    return View(player);
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    return RedirectToAction("UserPage", "Cricket");
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("Login", "Cricket");
            }
        }


        /// <summary>
        /// POST: Add PLayer
        /// </summary>
        /// <param name="VO"></param>
        /// <param name="UploadFiles"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPlayer(Player_VO VO, IEnumerable<HttpPostedFileBase> UploadFiles)
        {
            try
            {
                bm = new Cricket_BM();
                if (ModelState.IsValid)
                {
                    if (VO.Position == null)
                    {
                        if (!string.IsNullOrEmpty(VO.Batsman))
                        {
                            VO.Position = VO.Batsman.ToString();
                        }
                        if (!string.IsNullOrEmpty(VO.Baller) && VO.Position == null)
                        {
                            VO.Position = VO.Baller.ToString();
                        }
                        else if (VO.Baller != null && VO.Position != null)
                        {
                            VO.Position = VO.Position + ", " + VO.Baller.ToString();
                        }
                        if (!string.IsNullOrEmpty(VO.Fielder) && VO.Position == null)
                        {
                            VO.Position = VO.Fielder.ToString();
                        }
                        else if (VO.Position != null && VO.Fielder != null)
                        {
                            VO.Position = VO.Position + ", " + VO.Fielder.ToString();
                        }
                        if (!string.IsNullOrEmpty(VO.AllRounder) && VO.Batsman == null && VO.Baller == null && VO.Fielder == null)
                        {
                            VO.Position = VO.AllRounder.ToString();
                        }
                    }
                    foreach (var file in UploadFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                VO.ProfilePicture = binaryReader.ReadBytes(file.ContentLength);
                            }
                        }
                        else
                        {
                            VO.ProfilePicture = null;
                        }
                    }

                    bm.addingPlayerData(VO);

                    return RedirectToAction("AddPlayer", "Cricket");
                }
                else
                {
                    ViewData["Teams"] = bm.Teams();
                    ViewData["Countries"] = bm.Countries();
                    ViewData["Count"] = bm.TotalPlayers().ToString();

                    return View(VO);
                }
            }
            catch (Exception ex)
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                Disposing();
            }
        }


        /// <summary>
        /// Adding Team Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddTeam()
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                try
                {
                    bm = new Cricket_BM();
                    team = new Team_VO();

                    ViewData["Players"] = bm.Players();
                    ViewData["Count"] = bm.TotalTeams().ToString();

                    return View(team);
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("Login", "Cricket");
            }
        }


        /// <summary>
        /// Adding Team
        /// </summary>
        /// <param name="VO"></param>
        /// <param name="UploadFiles"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTeam(Team_VO VO, IEnumerable<HttpPostedFileBase> UploadFiles)
        {
            try
            {
                bm = new Cricket_BM();

                if (ModelState.IsValid)
                {
                    foreach (var file in UploadFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                VO.TeamIcon = binaryReader.ReadBytes(file.ContentLength);
                            }
                        }
                    }
                    bm.addingTeamData(VO);

                    return RedirectToAction("AddTeam", "Cricket");
                }
                else
                {
                    bm = new Cricket_BM();

                    ViewData["Players"] = bm.Players();
                    ViewData["Count"] = bm.TotalTeams().ToString();

                    return View(VO);
                }
            }
            catch (Exception ex)
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                Disposing();
            }
        }


        /// <summary>
        /// GET: AddMatch (Adding Match Schedule)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddMatch()
        {
            if (Session["User"] as string == "super" || Session["User"] as string == "admin")
            {
                try
                {
                    bm = new Cricket_BM();
                    match = new Match_VO();

                    ViewData["Teams"] = bm.Teams();
                    ViewData["Count"] = bm.TotalMatches();
                    ViewData["MatchType"] = bm.MatchType();
                    ViewData["Zones"] = bm.TimeZoneOffset();
                    ViewData["MatchVenue"] = bm.MatchVenue();

                    return View(match);
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("UserPage", "Cricket");
            }
        }


        /// <summary>
        /// POST: AddMatch (Saving Match Schedule)
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMatch(Match_VO VO)
        {
            try
            {
                bm = new Cricket_BM();

                if (ModelState.IsValid && (VO.Team_A != VO.Team_B))
                {
                    bm.addingMatchData(VO);

                    return RedirectToAction("AddMatch", "Cricket");
                }
                else
                {
                    ViewData["Teams"] = bm.Teams();
                    ViewData["Count"] = bm.TotalMatches();
                    ViewData["MatchType"] = bm.MatchType();
                    ViewData["Zones"] = bm.TimeZoneOffset();
                    ViewData["MatchVenue"] = bm.MatchVenue();

                    return View(VO);
                }
            }
            catch (Exception ex)
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                Disposing();
            }
        }


        /// <summary>
        /// Gogin to Matches Scheduled for Live Page
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public ActionResult LiveMatch()
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                try
                {
                    bm = new Cricket_BM();

                    ViewData["MatchDetails"] = bm.matchDetails();
                    return View();
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("UserPage", "Cricket");
            }
        }


        /// <summary>
        /// GET: Match Details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public ActionResult NewMatch(string ID)
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                bm = new Cricket_BM();
                Session["MATCHID"] = ID;

                try
                {
                    if(bm.IsLive(ID) == 1)
                    {
                        return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = ID });
                    }
                    else
                    {
                        List<Match_VO> liveMatchData = bm.LiveMatchData(ID);
                        List<Team_VO> teamAData = bm.Live_TeamA_Data(ID);
                        List<Team_VO> teamBData = bm.Live_TeamB_Data(ID);

                        ViewData["MatchId"] = ID;
                        ViewData["TeamAName"] = liveMatchData[0].Team_A.ToString();
                        ViewData["TeamBName"] = liveMatchData[0].Team_B.ToString();
                        ViewData["MatchType"] = liveMatchData[0].MatchType.ToString();
                        ViewData["MatchVenue"] = liveMatchData[0].MatchVenue.ToString();
                        ViewData["MatchTimings"] = liveMatchData[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");

                        ViewData["TeamAIcon"] = teamAData[0].TEAM_ICON.ToString();
                        ViewData["TeamAShortName"] = teamAData[0].TeamShortName.ToString();
                        ViewData["TeamBIcon"] = teamBData[0].TEAM_ICON.ToString();
                        ViewData["TeamBShortName"] = teamBData[0].TeamShortName.ToString();
                        ViewData["Players_TeamA"] = bm.Live_TeamA_PlayersDetails(ID);
                        ViewData["Players_TeamB"] = bm.Live_TeamB_PlayersDetails(ID);

                        liveMatchData.Clear();
                        teamAData.Clear();
                        teamBData.Clear();
                        liveMatchData = null;
                        teamBData = null;
                        teamAData = null;

                        return View();
                    }
                    
                }
                catch (Exception ex)
                {
                    if (ID != null)
                    {
                        ID = null;
                    }
                    DisposingAll();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                return RedirectToAction("AdminPage", "Cricket");
            }
        }


        /// <summary>
        /// GET: Starting Toss
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public ActionResult TossDecision(string ID)
        {
            bm = new Cricket_BM();
            Session["TOSS_ID"] = ID;

            try
            {
                toss = new Toss_Decision_VO();

                List<Team_VO> teamAData = bm.Live_TeamA_Data(ID);
                List<Team_VO> teamBData = bm.Live_TeamB_Data(ID);

                ViewData["MatchID"] = ID;
                ViewData["TeamAID"] = teamAData[0].TeamID.ToString();
                ViewData["TeamBID"] = teamBData[0].TeamID.ToString();

                ViewData["TeamAIcon"] = teamAData[0].TEAM_ICON.ToString();
                ViewData["TeamBIcon"] = teamBData[0].TEAM_ICON.ToString();

                ViewData["TeamAShortName"] = teamAData[0].TeamShortName.ToString();
                ViewData["TeamBShortName"] = teamBData[0].TeamShortName.ToString();

                teamAData.Clear();
                teamAData = null;
                teamBData.Clear();
                teamBData = null;

                return View(toss);
            }
            catch (Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }

        }


        /// <summary>
        /// POST: Toss Decision
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public ActionResult TossDecision(Toss_Decision_VO VO)
        {
            bm = new Cricket_BM();
            
            try
            {
                if (ModelState.IsValid)
                {
                    bm.addingTossDetails(VO);
                    bm.FinalDecisionValue(VO);

                    Session["MATCH_ID"] = VO.MatchID;
                    return RedirectToAction("SelectPlayers", "Cricket", new { ID = VO.MatchID });
                }
                else
                {
                    string ID = Session["TOSS_ID"] as string;

                    List<Team_VO> teamAData = bm.Live_TeamA_Data(ID);
                    List<Team_VO> teamBData = bm.Live_TeamB_Data(ID);

                    ViewData["MatchID"] = ID;
                    ViewData["TeamAID"] = teamAData[0].TeamID.ToString();
                    ViewData["TeamBID"] = teamBData[0].TeamID.ToString();
                    ViewData["TeamAIcon"] = teamAData[0].TEAM_ICON.ToString();
                    ViewData["TeamBIcon"] = teamBData[0].TEAM_ICON.ToString();
                    ViewData["TeamAShortName"] = teamAData[0].TeamShortName.ToString();
                    ViewData["TeamBShortName"] = teamBData[0].TeamShortName.ToString();

                    teamAData.Clear();
                    teamAData = null;
                    teamBData.Clear();
                    teamBData = null;

                    return View(VO);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// Selecting Players
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public ActionResult SelectPlayers(string ID)
        {
            bm = new Cricket_BM();
            selectPlayers = new SelectPlayers_VO();

            try
            {
                List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                List<Match_VO> match = bm.MATCH_DATA(ID);

                ViewData["MatchID"] = match[0].MatchId;
                ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                ViewData["BattingTeamID"] = teamA[0].TeamID;
                ViewData["BowlingTeamID"] = teamB[0].TeamID;
                ViewData["TeamAName"] = teamA[0].TeamName;
                ViewData["TeamBName"] = teamB[0].TeamName;
                ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                ViewData["MatchType"] = match[0].MatchType.ToString();
                ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");

                ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                teamA.Clear();
                teamA = null;
                teamB.Clear();
                teamB = null;
                match.Clear();
                match = null;

                return View(selectPlayers);
            }
            catch(Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// POST: Sending the Players to DB
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public ActionResult SelectPlayers(SelectPlayers_VO VO)
        {
            bm = new Cricket_BM();

            try
            {
                if (ModelState.IsValid)
                {
                    /*if (VO.BowlingPLayers != null && VO.BattingPlayers != null)
                    {
                        if (VO.BowlingPLayers.Count < 11)
                        {
                            ModelState.AddModelError("BowlingPLayers", "The Playing count should be equal to 11. You selected :" + VO.BowlingPLayers.Count + " players.");
                            string ID = Session["ID"] as string;

                            List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                            List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                            List<Match_VO> match = bm.MATCH_DATA(ID);


                            ViewData["MatchID"] = match[0].MatchId;
                            ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                            ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                            ViewData["BattingTeamID"] = teamA[0].TeamID;
                            ViewData["BowlingTeamID"] = teamB[0].TeamID;
                            ViewData["TeamAName"] = teamA[0].TeamName;
                            ViewData["TeamBName"] = teamB[0].TeamName;
                            ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                            ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                            ViewData["MatchType"] = match[0].MatchType.ToString();
                            ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                            ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");
                            ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                            ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                            teamA.Clear();
                            teamA = null;
                            teamB.Clear();
                            teamB = null;
                            match.Clear();
                            match = null;

                            return View(VO);
                        }
                        else if (VO.BowlingPLayers.Count > 11)
                        {
                            ModelState.AddModelError("BowlingPLayers", "The Playing Count should be 10, you selected " + VO.BowlingPLayers.Count + "  which is greater than 11.");
                            string ID = Session["ID"] as string;

                            List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                            List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                            List<Match_VO> match = bm.MATCH_DATA(ID);


                            ViewData["MatchID"] = match[0].MatchId;
                            ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                            ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                            ViewData["BattingTeamID"] = teamA[0].TeamID;
                            ViewData["BowlingTeamID"] = teamB[0].TeamID;
                            ViewData["TeamAName"] = teamA[0].TeamName;
                            ViewData["TeamBName"] = teamB[0].TeamName;
                            ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                            ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                            ViewData["MatchType"] = match[0].MatchType.ToString();
                            ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                            ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");
                            ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                            ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                            teamA.Clear();
                            teamA = null;
                            teamB.Clear();
                            teamB = null;
                            match.Clear();
                            match = null;

                            return View(VO);
                        }
                        else if (VO.BattingPlayers.Count < 11)
                        {
                            ModelState.AddModelError("BattingPlayers", "The Playing 10 count should be greater than + " + VO.BowlingPLayers.Count + " and less than 11.");
                            string ID = Session["ID"] as string;

                            List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                            List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                            List<Match_VO> match = bm.MATCH_DATA(ID);


                            ViewData["MatchID"] = match[0].MatchId;
                            ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                            ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                            ViewData["BattingTeamID"] = teamA[0].TeamID;
                            ViewData["BowlingTeamID"] = teamB[0].TeamID;
                            ViewData["TeamAName"] = teamA[0].TeamName;
                            ViewData["TeamBName"] = teamB[0].TeamName;
                            ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                            ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                            ViewData["MatchType"] = match[0].MatchType.ToString();
                            ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                            ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");
                            ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                            ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                            teamA.Clear();
                            teamA = null;
                            teamB.Clear();
                            teamB = null;
                            match.Clear();
                            match = null;

                            return View(VO);
                        }
                        else if (VO.BattingPlayers.Count > 11)
                        {
                            ModelState.AddModelError("BattingPlayers", "The Playing Count should be 10, you selected " + VO.BowlingPLayers.Count + "  which is greater than 11.");
                            string ID = Session["ID"] as string;

                            List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                            List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                            List<Match_VO> match = bm.MATCH_DATA(ID);


                            ViewData["MatchID"] = match[0].MatchId;
                            ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                            ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                            ViewData["BattingTeamID"] = teamA[0].TeamID;
                            ViewData["BowlingTeamID"] = teamB[0].TeamID;
                            ViewData["TeamAName"] = teamA[0].TeamName;
                            ViewData["TeamBName"] = teamB[0].TeamName;
                            ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                            ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                            ViewData["MatchType"] = match[0].MatchType.ToString();
                            ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                            ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");
                            ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                            ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                            teamA.Clear();
                            teamA = null;
                            teamB.Clear();
                            teamB = null;
                            match.Clear();
                            match = null;

                            return View(VO);
                        }
                    }*/

                    bm.addingBattingBowlingData(VO);

                    return RedirectToAction("LiveScoreUpdate", "Cricket", new {ID = VO.MatchID});
                }
                else
                {
                    string ID = Session["MATCH_ID"] as string;

                    List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                    List<Team_VO> teamB = bm.BOWLING_TEAM(ID);
                    List<Match_VO> match = bm.MATCH_DATA(ID);


                    ViewData["MatchID"] = match[0].MatchId;
                    ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                    ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                    ViewData["BattingTeamID"] = teamA[0].TeamID;
                    ViewData["BowlingTeamID"] = teamB[0].TeamID;
                    ViewData["TeamAName"] = teamA[0].TeamName;
                    ViewData["TeamBName"] = teamB[0].TeamName;
                    ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                    ViewData["TeamBShortName"] = teamB[0].TeamShortName;
                    ViewData["MatchType"] = match[0].MatchType.ToString();
                    ViewData["MatchVenue"] = match[0].MatchVenue.ToString();
                    ViewData["MatchTimings"] = match[0].MatchTimings?.ToString("dd MMMM, yyyy hh:mm tt (zzz)");

                    ViewData["TeamAPlayers"] = bm.BATTING_PLAYERS(ID);
                    ViewData["TeamBPlayers"] = bm.BOWLING_PLAYERS(ID);


                    teamA.Clear();
                    teamA = null;
                    teamB.Clear();
                    teamB = null;
                    match.Clear();
                    match = null;

                    return View(VO);
                }

            }
            catch(Exception ex )
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {

            }
            
        }


        /// <summary>
        /// Live Score Update
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LiveScoreUpdate(string ID)
        {
            if (Session["User"] as string == "admin" || Session["User"] as string == "super")
            {
                Session["MATCHID"] = ID;
                bm = new Cricket_BM();

                try
                {
                    scoreBoard = new ScoreBoard_VO();

                    var (Striker, NonStriker, BattingTeamID) = bm.LiveMatchDetailsForBattingTeam(ID);
                    var (Bowler, BowlingTeamID, CurrentBall) = bm.LiveMatchDetailsForBowlingTeam(ID);
                    
                    Session["Striker"] = Striker;
                    Session["Non-Striker"] = NonStriker;
                    Session["Bowler"] = Bowler;
                    Session["BattingTeam"] = BattingTeamID;
                    Session["BowlingTeam"] = BowlingTeamID;
                    Session["BallNumber"] = bm.TotalBalls(ID);


                    int? balls = bm.TotalBalls(ID);
                    int? completedOvers = balls / 6;
                    int? remainingBalls = balls % 6;
                    float? overs = completedOvers + (remainingBalls * 0.1f);

                    List<BATTING_VO> STRIKER = bm.STRIKER(ID);
                    List<BATTING_VO> NON_STRIKER = bm.NON_STRIKER(ID);
                    List<BATTING_VO> BOWLER = bm.BOWLER(ID);

                    ViewData["STRIKER_NAME"] = STRIKER[0].BatsmanName;
                    ViewData["STRIKER_RUNS"] = STRIKER[0].TotalRuns;
                    ViewData["STRIKER_PICTURE"] = STRIKER[0].BatsmanPicture;
                    ViewData["NON_STRIKER_NAME"] = NON_STRIKER[0].BatsmanName;
                    ViewData["NON_STRIKER_RUNS"] = NON_STRIKER[0].TotalRuns;
                    ViewData["NON_STRIKER_PICTURE"] = NON_STRIKER[0].BatsmanPicture;
                    ViewData["BOWLER_PICTURE"] = BOWLER[0].BatsmanPicture;
                    ViewData["BOWLER_NAME"] = BOWLER[0].BatsmanName;


                    ViewData["Runs"] = bm.TotalRuns(ID);
                    ViewData["Outs"] = bm.TotalOuts(ID);
                    ViewData["Balls"] = overs;


                    ViewData["SCOREBOARD"] = bm.HALF_SCOREBOARD(ID);
                    ViewData["BOWLING_SCORE"] = bm.HALF_BOWLING(ID);
                    ViewData["COMMENTARY"] = bm.COMMENTARY(ID);


                    List<Team_VO> battingTeam = bm.BATTING_TEAM_LIVE(ID);
                    List<Team_VO> bowlingTeam = bm.BOWLING_TEAM_LIVE(ID);

                    {
                        ViewData["BATTING_TEAM_ID"] = battingTeam[0].TeamID.ToString();
                        ViewData["BOWLING_TEAM_ID"] = bowlingTeam[0].TeamID.ToString();
                        ViewData["BATTING_TEAM_ICON"] = battingTeam[0].TEAM_ICON.ToString();
                        ViewData["BOWLING_TEAM_ICON"] = bowlingTeam[0].TEAM_ICON.ToString();
                        ViewData["BATTING_TEAM_NAME"] = battingTeam[0].TeamName.ToString();
                        ViewData["BOWLING_TEAM_NAME"] = bowlingTeam[0].TeamName.ToString();
                        ViewData["BATTING_TEAM_SHORT_NAME"] = battingTeam[0].TeamShortName.ToString();
                        ViewData["BOWLING_TEAM_SHORT_NAME"] = bowlingTeam[0].TeamShortName.ToString();

                        ViewData["OutTypes"] = bm.Out();
                        ViewData["NotOut"] = bm.NOT_OUT_BATTING_PLAYERS(ID);
                        ViewData["Bowler"] = bm.LIVE_MATCH_BOWLERS(ID);
                    }


                    return View(scoreBoard);
                }
                catch (Exception ex)
                {
                    DisposingAll();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Disposing();
                }
            }
            else
            {
                Session.Clear();
                ViewData.Clear();
                return RedirectToAction("UserPage", "Cricket");
            }
            
        }


        /// <summary>
        /// Live Score Updation
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LiveScoreUpdate(ScoreBoard_VO VO)
        {
            try
            {
                bm = new Cricket_BM();

                if (ModelState.IsValid && Session["Striker"] != null)
                {
                    if(VO.Runs != null && VO.SpecialBall == null && VO.NewBowler == null && VO.Out == null)
                    {
                        if (VO.Runs == "0")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else if (VO.Runs == "1")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.ChangingStrikerNonStriker(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else if (VO.Runs == "2")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else if (VO.Runs == "3")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.ChangingStrikerNonStriker(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else if (VO.Runs == "4")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.Updating4RunsCount(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else if (VO.Runs == "6")
                        {
                            bm.UpdatingBallCount(VO);
                            bm.InsertEachBallNewRecord(VO);
                            bm.Updating6RunsCount(VO);
                            bm.COMMENTARY_RUNS_CASE1(VO);

                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else if (VO.Out != null && VO.NewStriker != null && VO.NewBowler == null && VO.Out == null)
                    {
                        bm.NewStrikerAdding(VO);
                        return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                    }
                    else if (VO.SpecialBall != null && VO.Runs != null && VO.NewBowler == null)
                    {
                        if (VO.SpecialBall == "1" && VO.Runs != null && VO.Out == null)
                        {
                            return View();
                        }

                        return View();
                    }
                    else if (VO.NewBowler != null)
                    {
                        if (VO.Runs != null && VO.SpecialBall == null)
                        {
                            if (VO.Runs == "0" || VO.Runs == "2")
                            {
                                bm.NEWBOWLER_STRIKER(VO);
                                bm.NEWBOWLER_BOWLER(VO);
                                bm.NEWBOWLER_RUNS_CASE1(VO);
                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                            else if (VO.Runs == "1" || VO.Runs == "3")
                            {
                                bm.NEWBOWLER_STRIKER(VO);
                                bm.NEWBOWLER_BOWLER(VO);
                                bm.NEWBOWLER_RUNS_CASE1(VO);
                                bm.ChangingStrikerNonStriker(VO);
                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                            else if (VO.Runs == "4")
                            {
                                bm.NEWBOWLER_STRIKER(VO);
                                bm.NEWBOWLER_BOWLER(VO);
                                bm.NEWBOWLER_RUNS_CASE1(VO);
                                bm.Updating4RunsCount(VO);
                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                            else if (VO.Runs == "6")
                            {
                                bm.NEWBOWLER_STRIKER(VO);
                                bm.NEWBOWLER_BOWLER(VO);
                                bm.NEWBOWLER_RUNS_CASE1(VO);
                                bm.Updating6RunsCount(VO);
                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                            else
                            {
                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                        }
                        else if (VO.SpecialBall != null && VO.Runs != null)
                        {
                            if (VO.Runs == "1" || VO.Runs == "3")
                            {
                                bm.NEWBOWLER_EXTRACASE_STRIKER_BOWLER(VO);
                                bm.NEWBOWLER_EXTRAS_CASE2(VO);
                                bm.ChangingStrikerNonStriker(VO);

                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                            else
                            {
                                bm.NEWBOWLER_EXTRACASE_STRIKER_BOWLER(VO);
                                bm.NEWBOWLER_EXTRAS_CASE2(VO);

                                return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                            }
                        }
                        else if (VO.Out != null && VO.NewStriker != null)
                        {
                            bm.NEWBOWLER_NEWSTRIKER_CASE3_PART1(VO);
                            bm.NEWBOWLER_NEWSTRIKER_CASE3_PART2(VO);
                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }
                        else
                        {
                            return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                        }

                    }
                    else
                    {
                        return RedirectToAction("LiveScoreUpdate", "Cricket", new { ID = Session["MATCHID"] as string });
                    }
                }
                else
                {
                    if (Session["MATCHID"] != null)
                    {
                        {
                            string ID = Session["MATCHID"] as string;

                            var (Striker, NonStriker, BattingTeamID) = bm.LiveMatchDetailsForBattingTeam(ID);

                            Session["Striker"] = Striker;
                            Session["Non-Striker"] = NonStriker;


                            int? balls = bm.TotalBalls(ID);
                            int? completedOvers = balls / 6;
                            int? remainingBalls = bm.TotalBalls(ID) % 6;
                            float? overs = completedOvers + (remainingBalls * 0.1f);

                            //Changing Positions if New Over Starts
                            if (completedOvers > 0 && remainingBalls == 0)
                            {
                                int? striker = Session["Striker"] as int?;
                                int? nonStriker = Session["Non-Striker"] as int?;
                                var (batTeam, bowlTeam) = bm.BattingBowlingIDS(ID);

                                ViewData["NewBowler"] = true;
                                Session["Striker"] = nonStriker;
                                Session["Non-Striker"] = striker;

                                ScoreBoard_VO score = new ScoreBoard_VO
                                {
                                    Striker = Striker,
                                    NonStriker = NonStriker,
                                    MatchID = Convert.ToInt32(ID),
                                    BowlingTeamID = bowlTeam
                                };

                                if (bm.CurrentBall(score) == bm.PreviousBall(score) && (bool)ViewData["NewBowler"])
                                {
                                    ViewData["NewBowler"] = false;
                                    Session["Striker"] = striker;
                                    Session["Non-Striker"] = nonStriker;
                                }
                                else if (bm.CurrentBall(score) != bm.PreviousBall(score) && Session["CHANGED"] != null && (bool)Session["CHANGED"] == true)
                                {

                                }
                                else
                                {
                                    bm.ChangingStrikerNonStriker(score);
                                    Session["CHANGED"] = true;
                                }
                                score = null;
                            }
                            else
                            {
                                ViewData["NewBowler"] = false;
                            }

                            var (Bowler, BowlingTeamID, CurrentBall) = bm.LiveMatchDetailsForBowlingTeam(ID);

                            Session["Bowler"] = Bowler;
                            Session["BattingTeam"] = BattingTeamID;
                            Session["BowlingTeam"] = BowlingTeamID;
                            Session["BallNumber"] = balls;

                            List<BATTING_VO> STRIKER = bm.STRIKER(ID);
                            List<BATTING_VO> NON_STRIKER = bm.NON_STRIKER(ID);
                            List<BATTING_VO> BOWLER = bm.BOWLER(ID);

                            ViewData["STRIKER_NAME"] = STRIKER[0].BatsmanName;
                            ViewData["STRIKER_RUNS"] = STRIKER[0].TotalRuns;
                            ViewData["STRIKER_PICTURE"] = STRIKER[0].BatsmanPicture;
                            ViewData["NON_STRIKER_NAME"] = NON_STRIKER[0].BatsmanName;
                            ViewData["NON_STRIKER_RUNS"] = NON_STRIKER[0].TotalRuns;
                            ViewData["NON_STRIKER_PICTURE"] = NON_STRIKER[0].BatsmanPicture;
                            ViewData["BOWLER_PICTURE"] = BOWLER[0].BatsmanPicture;
                            ViewData["BOWLER_NAME"] = BOWLER[0].BatsmanName;


                            ViewData["Runs"] = bm.TotalRuns(ID);
                            ViewData["Outs"] = bm.TotalOuts(ID);
                            ViewData["Balls"] = overs;


                            ViewData["SCOREBOARD"] = bm.HALF_SCOREBOARD(ID);
                            ViewData["BOWLING_SCORE"] = bm.HALF_BOWLING(ID);


                            List<Team_VO> battingTeam = bm.BATTING_TEAM_LIVE(ID);
                            List<Team_VO> bowlingTeam = bm.BOWLING_TEAM_LIVE(ID);

                            {
                                ViewData["BATTING_TEAM_ID"] = battingTeam[0].TeamID.ToString();
                                ViewData["BOWLING_TEAM_ID"] = bowlingTeam[0].TeamID.ToString();
                                ViewData["BATTING_TEAM_ICON"] = battingTeam[0].TEAM_ICON.ToString();
                                ViewData["BOWLING_TEAM_ICON"] = bowlingTeam[0].TEAM_ICON.ToString();
                                ViewData["BATTING_TEAM_NAME"] = battingTeam[0].TeamName.ToString();
                                ViewData["BOWLING_TEAM_NAME"] = bowlingTeam[0].TeamName.ToString();
                                ViewData["BATTING_TEAM_SHORT_NAME"] = battingTeam[0].TeamShortName.ToString();
                                ViewData["BOWLING_TEAM_SHORT_NAME"] = bowlingTeam[0].TeamShortName.ToString();

                                ViewData["OutTypes"] = bm.Out();
                                ViewData["NotOut"] = bm.NOT_OUT_BATTING_PLAYERS(ID);
                                ViewData["Bowler"] = bm.LIVE_MATCH_BOWLERS(ID);
                            }
                        }

                        if (VO.Striker != Session["Striker"] as int?)
                        {
                            ModelState.AddModelError("Striker", "Please reload the page. The record was not saved.");
                            return View(VO);
                        }
                        else if (VO.NonStriker != Session["NonStriker"] as int?)
                        {
                            ModelState.AddModelError("NonStriker", "Please reload the page. The record was not saved.");
                            return View(VO);
                        }
                        else if (VO.Bowler != Session["Bowler"] as int?)
                        {
                            ModelState.AddModelError("Bowler", "Please reload the page. The record was not saved.");
                            return View(VO);
                        }
                        else if (VO.BattingTeamID != Session["BattingTeam"] as int?)
                        {
                            ModelState.AddModelError("BattingTeamID", "Please go to Live Match first, then come again. The record was not saved.");
                            return View(VO);
                        }
                        else if (VO.BowlingTeamID != Session["BowlingTeam"] as int?)
                        {
                            ModelState.AddModelError("BowlingTeamID", "Please go to Live Match first, then come again. The record was not saved.");
                            return View(VO);
                        }
                        else if (VO.SpecialBall != null && VO.Runs == null)
                        {
                            ModelState.AddModelError("SpecialBall", "Please select the Runs For the Extras. The record was not saved.");
                            return View(VO);
                        }
                        else
                        {
                            Session.Clear();
                            return RedirectToAction("UserPage", "Cricket");
                        }
                    }
                    else
                    {
                        Session.Clear();
                        return RedirectToAction("UserPage", "Cricket");
                    }
                }
                
            }
            catch(Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        [HttpGet]
        public ActionResult Batting(string ID)
        {
            bm = new Cricket_BM();
            try
            {
                int? runs1 = bm.TotalRuns(ID);
                float? overs1 = (bm.TotalBalls(ID) / 6) + ((bm.TotalBalls(ID) % 6) * 0.1f);

                float? CRR1 = (float?)Math.Round(runs1.Value / overs1.Value, 3);
                (int? noball1, int? wide1, int? bye1, int? legBye1, int? penalty1) = bm.ExtrasCount(ID);

                List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                List<Team_VO> teamB = bm.BOWLING_TEAM(ID);

                if (teamA != null && teamB != null)
                {
                    ViewData["TeamAName"] = teamA[0].TeamName;
                    ViewData["TeamBName"] = teamB[0].TeamName;
                    ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                    ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                    ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                    ViewData["TeamBShortName"] = teamB[0].TeamShortName;

                    ViewData["ID"] = ID;
                    ViewData["Runs"] = bm.TotalRuns(ID);
                    ViewData["Outs"] = bm.TotalOuts(ID);
                    ViewData["Balls"] = overs1;
                    ViewData["CRR"] = (CRR1.HasValue && !double.IsNaN(CRR1.Value)) ? CRR1.Value : 0;
                    

                    ViewData["BATTING"] = bm.BATTING_SCOREBOARD_1(ID);
                    ViewData["BOWLING"] = bm.BOWLING_SCOREBOARD_2(ID);

                    ViewData["NO"] = noball1;
                    ViewData["WIDE"] = wide1;
                    ViewData["BYE"] = bye1;
                    ViewData["LEGBYE"] = legBye1;
                    ViewData["PENALTY"] = penalty1;
                }
                
                teamA.Clear(); teamA = null;
                teamB.Clear(); teamB = null;

                noball1 = null; wide1 = null;
                bye1 = null; legBye1 = null; penalty1 = null; 
                runs1 = null; overs1 = null; CRR1 = null; 

                return View();
            }
            catch(Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
            
        }


        [HttpGet]
        public ActionResult Bowling(string ID)
        {
            bm = new Cricket_BM();

            try
            {
                int? runs2 = bm.TotalRuns_2(ID);
                float? overs2 = (bm.TotalBalls_2(ID) / 6) + ((bm.TotalBalls_2(ID) % 6) * 0.1f);

                float? CRR2 = (float?)Math.Round(runs2.Value / overs2.Value, 3);
                (int? noball2, int? wide2, int? bye2,int? legBye2,int? penalty2) = bm.ExtrasCount_2(ID);

                List<Team_VO> teamA = bm.BATTING_TEAM(ID);
                List<Team_VO> teamB = bm.BOWLING_TEAM(ID);

                if (teamA != null && teamB != null)
                {
                    ViewData["ID"] = ID;
                    ViewData["TeamAName"] = teamA[0].TeamName;
                    ViewData["TeamBName"] = teamB[0].TeamName;
                    ViewData["TeamAIcon"] = teamA[0].TEAM_ICON;
                    ViewData["TeamBIcon"] = teamB[0].TEAM_ICON;
                    ViewData["TeamAShortName"] = teamA[0].TeamShortName;
                    ViewData["TeamBShortName"] = teamB[0].TeamShortName;


                    ViewData["BATTING"] = bm.BATTING_SCOREBOARD_2(ID);
                    ViewData["BOWLING"] = bm.BOWLING_SCOREBOARD_1(ID);


                    ViewData["Runs"] = bm.TotalRuns_2(ID);
                    ViewData["Outs"] = bm.TotalOuts_2(ID);
                    ViewData["Balls"] = overs2;
                    ViewData["CRR"] = (CRR2.HasValue && !double.IsNaN(CRR2.Value)) ? CRR2.Value : 0;


                    ViewData["NO"] = noball2;
                    ViewData["WIDE"] = wide2;
                    ViewData["BYE"] = bye2;
                    ViewData["LEGBYE"] = legBye2;
                    ViewData["PENALTY"] = penalty2;
                }

                teamA.Clear(); teamA = null;
                teamB.Clear(); teamB = null;

                runs2 = null; overs2 = null;
                CRR2 = null; noball2 = null;
                wide2 = null; bye2 = null;
                legBye2 = null; penalty2 = null;

                return View();
            }
            catch(Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
            
        }


        /// <summary>
        /// GET: View Players Details Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewPlayers()
        {
            try
            {
                bm = new Cricket_BM();

                ViewData["PlayerDetails"] = bm.playerDetails();

                return View();
            }
            catch (Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// GET: Team Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewTeams()
        {
            try
            {
                bm = new Cricket_BM();

                ViewData["TeamDetails"] = bm.teamDetails();

                return View();
            }
            catch (Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// View Match Schedule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewMatches()
        {
            try
            {
                bm = new Cricket_BM();
                ViewData["MatchDetails"] = bm.matchDetails();

                return View();
            }
            catch (Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// GET: Time Zones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MatchFilter()
        {
            try
            {
                bm = new Cricket_BM();
                filter = new MatchFilter_VO();

                ViewData["Zones"] = bm.TimeZoneOffset();

                return View(filter);
            }
            catch (Exception ex)
            {
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// POST: Match Filtering
        /// </summary>
        /// <param name="VO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public ActionResult MatchFilter(MatchFilter_VO VO)
        {
            try
            {
                bm = new Cricket_BM();

                if (ModelState.IsValid)
                {
                    Session["FilteredData"] = bm.FilteredMatchData(VO);

                    string offset1 = VO.MatchFromOffset;
                    string offset2 = VO.MatchToOffset;

                    if(offset1.Contains("+"))
                    {
                        offset1 = offset1.Substring(1);
                    }
                    if(offset2.Contains("+"))
                    {
                        offset2 = offset2.Substring(1);
                    }

                    Session["Start"] = new DateTimeOffset(VO.MatchFrom.Value, TimeSpan.Parse(offset1)).ToString("dd MMMM, yyyy hh:mm tt (zzz)");
                    Session["End"] = new DateTimeOffset(VO.MatchTo.Value, TimeSpan.Parse(offset2)).ToString("dd MMMM, yyyy hh:mm tt (zzz)");

                    offset1 = null;
                    offset2 = null;

                    return RedirectToAction("FilteredMatchResult", "Cricket");
                }
                else
                {
                    ViewData["Zones"] = bm.TimeZoneOffset();

                    return View(VO);
                }
            }
            catch(Exception ex)
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                DisposingAll();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (VO != null)
                {
                    VO.Dispose();
                    VO = null;
                }
                Disposing();
            }
        }


        /// <summary>
        /// GET: Filtererd Match Result
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public ActionResult FilteredMatchResult()
        {
            try
            {
                return View();
            }
            finally
            {
                Disposing();
            }
        }


        /// <summary>
        /// Disposing Aall the Objects
        /// </summary>
        public void Disposing()
        {
            if (login != null)
            {
                login.Dispose();
                login = null;
            }
            if(player != null)
            {
                player.Dispose();
                player = null;
            }
            if (match != null)
            {
                match.Dispose();
                match = null;
            }
            if (team != null)
            {
                team.Dispose();
                team = null;
            }
            if(filter != null)
            {
                filter.Dispose(); 
                filter = null;
            }


            if(TempData !=  null)
            {
                TempData = null;
            }
            if(ViewData != null)
            {
                ViewData = null;
            }
            if(bm != null)
            {
                bm = null;
            }
        }
        public void DisposingAll()
        {
            Disposing();
            Dispose();

            TempData.Clear();
            TempData = null;
            ViewData.Clear();
            ViewData = null;
            Session.Clear();
            Session.Abandon();
        }

       /* /// <summary>
        /// Returning The Data
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public ActionResult UrlDatasource(DataManagerRequest dm)
        {
            try
            {
                bm = new Cricket_BM();
                List<Cricket_VO> DataSource = bm.playerDetails();
                DataOperations operation = new DataOperations();

                int count = DataSource.Count();
                if (dm.Skip != 0)
                {
                    DataSource = operation.PerformSkip(DataSource, dm.Skip).ToList();
                }
                if (dm.Take != 0)
                {
                    DataSource = operation.PerformTake(DataSource, dm.Take).ToList();
                }
                return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
            }
            finally
            {
                bm = null;
                
            }
        }*/
    }
}

/*

int? balls = bm.TotalBalls(ID);
int? completedOvers = balls / 6;
int? remainingBalls = bm.TotalBalls(ID) % 6;
float? overs = completedOvers + (remainingBalls * 0.1f);

//Changing Positions if New Over Starts
if (completedOvers > 0 && remainingBalls == 0)
{
    int? striker = Session["Striker"] as int?;
    int? nonStriker = Session["Non-Striker"] as int?;
    var (batTeam, bowlTeam) = bm.BattingBowlingIDS(ID);

    ViewData["NewBowler"] = true;
    Session["Striker"] = nonStriker;
    Session["Non-Striker"] = striker;

    ScoreBoard_VO score = new ScoreBoard_VO
    {
        Striker = Striker,
        NonStriker = NonStriker,
        MatchID = Convert.ToInt32(ID),
        BowlingTeamID = bowlTeam
    };

    if (bm.CurrentBall(score) == bm.PreviousBall(score) && (bool)ViewData["NewBowler"])
    {
        ViewData["NewBowler"] = false;
        Session["Striker"] = striker;
        Session["Non-Striker"] = nonStriker;
    }
    else if (bm.CurrentBall(score) != bm.PreviousBall(score) && Session["CHANGED"] != null && (bool)Session["CHANGED"] == true)
    {

    }
    else
    {
        bm.ChangingStrikerNonStriker(score);
        Session["CHANGED"] = true;
    }
    score = null;
}
else
{
    ViewData["NewBowler"] = false;
}*/

