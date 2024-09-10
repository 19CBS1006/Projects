using System;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class MatchFilter_VO : IDisposable, IMatchFilter_VO
    {
        /// <summary>
        /// Macth From DATE
        /// </summary>
        [Required(ErrorMessage = "Match Start Date cannot be Empty")]
        [DataType(DataType.Date)]
        [Display(Name = "Match Start Date")]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime? MatchFrom { get; set; }


        [Required(ErrorMessage = "Match Start Offset cannot be Empty")]
        [Display(Name = "Match Start Time Zone")]
        public string MatchFromOffset { get; set; }


        /// <summary>
        /// Match To DATE
        /// </summary>
        [Required(ErrorMessage = "Match Ends Date cannot be Empty")]
        [DataType(DataType.Date)]
        [Display(Name = "Match End Date")]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime? MatchTo { get; set; }


        [Required(ErrorMessage = "Match End Offset cannot be Empty")]
        [Display(Name = "Match End Time Zone")]
        public string MatchToOffset { get; set; }


        /// <summary>
        /// For Displaying
        /// </summary>
        public DateTimeOffset? MatchTimings { get; set; }


        /// <summary>
        /// Team A
        /// </summary>
        public string Team_A { get; set; }


        /// <summary>
        /// Team B
        /// </summary>
        public string Team_B { get; set; }


        /// <summary>
        /// Venue of the Match
        /// </summary>
        public string MatchVenue { get; set; }


        /// <summary>
        /// Match Result
        /// </summary>
        public string MatchResult { get; set; }


        /// <summary>
        /// Type of Match
        /// </summary>
        public string MatchType { get; set; }


        /// <summary>
        /// Status of the Match
        /// </summary>
        public string MatchStatus { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MatchFrom = null;
                MatchFromOffset = null;
                MatchTo = null;
                MatchToOffset = null;
                MatchTimings = null;
                Team_A = null;
                Team_B = null;
                MatchVenue = null;
                MatchStatus = null;
            }
        }

        ~MatchFilter_VO()
        {
            Dispose(false);
        }
    }
}
