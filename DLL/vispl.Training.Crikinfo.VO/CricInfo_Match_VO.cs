using System;
using System.ComponentModel.DataAnnotations;

namespace Vispl.Training.Crickinfo.VO
{
    public class Match_VO : IMatch_VO, IDisposable
    {
        /// <summary>
        /// Match ID
        /// </summary>
        public int? MatchId { get; set; }


        /// <summary>
        /// Venue of the Match
        /// </summary>
        [Required(ErrorMessage = "Match Venue cannot be Empty")]
        public string MatchVenue { get; set; }


        /// <summary>
        /// Type of the Match
        /// </summary>
        [Required(ErrorMessage = "Match Type cannot be Empty")]
        public string MatchType { get; set; }


        /// <summary>
        /// Team A
        /// </summary>
        [Required(ErrorMessage = "Select Team A")]
        public string Team_A { get; set; }


        /// <summary>
        /// Team B
        /// </summary>
        [Required(ErrorMessage = "Select Team B")]
        public string Team_B { get; set; }


        /// <summary>
        /// Time of Match
        /// </summary>
        [Required(ErrorMessage = "Please Select the Date of the Match")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime? MatchSchedule { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "Please Select a Time Zone")]
        public string MatchScheduleOffset { get; set; }


        /// <summary>
        /// For Displaying
        /// </summary>
        public DateTimeOffset? MatchTimings { get; set; }


        /// <summary>
        /// Disposing Method
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
                MatchId = null;
                MatchVenue = null;
                MatchType = null;
                Team_A = null;
                Team_B = null;
                MatchSchedule = null;
                MatchScheduleOffset = null;
                MatchTimings = null;
            }
        }
        ~Match_VO()
        {
            Dispose(false);
        }
    }
}
