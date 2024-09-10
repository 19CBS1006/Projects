using System;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class Player_VO : IDisposable, IPlayer_VO
    {
        /// <summary>
        /// Getting Player ID from Team ID
        /// </summary>
        public int? Player_ID { get; set; }

        /// <summary>
        /// Jersey Number
        /// </summary>
        [Required]
        [Display(Name = "Jersey Number")]
        public string Jersey { get; set; }


        /// <summary>
        /// First Name
        /// </summary>
        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }


        /// <summary>
        /// Middle Name
        /// </summary>
        [Display(Name = "Middle Name: ")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [Required]
        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }

        /// <summary>
        /// Date of Birth
        /// </summary>
        [Required]
        [Display(Name = "Date of Birth (D.O.B.): ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; } = DateTime.Now;

        /// <summary>
        /// Age
        /// </summary>
        [Required]
        [Display(Name = "Age: ")]
        public int? Age { get; set; }

        /// <summary>
        /// Birthplace of the player
        /// </summary>
        [Required]
        [Display(Name = "Birth Place: ")]
        public string BirthPlace { get; set; }


        /// <summary>
        /// Indicates if the player is the captain
        /// </summary>
        [Required]
        [Display(Name = "Captain: ")]
        public string IsCaptain { get; set; }


        /// <summary>
        /// Indicates if the player is a substitute
        /// </summary>
        [Required]
        [Display(Name = "Substitute: ")]
        public string IsSubstitute { get; set; }


        /// <summary>
        /// Franchise the player is associated with
        /// </summary>
        [Display(Name = "Franchise: ")]
        public string Franchise { get; set; }


        /// <summary>
        /// Number of matches played by the player
        /// </summary>
        [Required]
        [Display(Name = "Matches Played: ")]
        public int? MatchesPlayed { get; set; }


        /// <summary>
        /// Total runs scored by the player
        /// </summary>
        [Required]
        [Display(Name = "Runs Scored: ")]
        public int? RunsScored { get; set; }

        /// <summary>
        /// Total wickets taken by the player (nullable)
        /// </summary>
        [Required]
        [Display(Name = "Wickets Taken: ")]
        public int? WicketsTaken { get; set; }

        /// <summary>
        /// Number of centuries scored by the player (nullable)
        /// </summary>
        [Required]
        [Display(Name = "Centuries: ")]
        public int? Centuries { get; set; }

        /// <summary>
        /// Number of half-centuries scored by the player (nullable)
        /// </summary>
        [Required]
        [Display(Name = "Half-Centuries: ")]
        public int? HalfCenturies { get; set; }

        /// <summary>
        /// Debut date of the player
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        public DateTime? DebutDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// ICC ranking of the player (nullable)
        /// </summary>
        [Required]
        [Display(Name = "ICC Ranking: ")]
        public int? ICCRanking { get; set; }

        /// <summary>
        /// FORM-POSITION FIELD
        /// </summary>
        [Display(Name = "Position: ")]
        public string Position { get; set; }

        /// <summary>
        /// Player Type
        /// </summary>
        public string Batsman { get; set; }
        public string Baller { get; set; }
        public string Fielder { get; set; }
        public string AllRounder { get; set; }


        /// <summary>
        /// FORM-COUNTRY FIELD
        /// </summary>
        [Required]
        [Display(Name = "Country: ")]
        public string Country { get; set; }


        /// <summary>
        /// Profile Picture
        /// </summary>
        [Display(Name = "Profile Picture: ")]
        public byte[] ProfilePicture { get; set; }


        /// <summary>
        /// 64bit String for picture display
        /// </summary>
        public string picture { get; set; }


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
                Player_ID = null;
                Jersey = null;
                FirstName = null;
                MiddleName = null;
                LastName = null;
                DOB = null;
                Age = null;
                BirthPlace = null;
                IsCaptain = null;
                IsSubstitute = null;
                Franchise = null;
                MatchesPlayed = null;
                RunsScored = null;
                WicketsTaken = null;
                Centuries = null;
                HalfCenturies = null;
                DebutDate = null;
                ICCRanking = null;
                Position = null;
                Batsman = null;
                Baller = null;
                Fielder = null;
                AllRounder = null;
                Country = null;
                ProfilePicture = null;
                picture = null;
            }
        }
        ~Player_VO()
        {
            Dispose(false);
        }
    }
}
