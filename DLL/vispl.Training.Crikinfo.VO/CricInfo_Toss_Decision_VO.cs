using System;
using System.ComponentModel.DataAnnotations;
namespace Vispl.Training.Crickinfo.VO
{
    public class Toss_Decision_VO : IDisposable, IToss_Decision_VO
    {
        public int? TossID { get; set; }

        [Required]
        public string MatchID { get; set; }

        [Required]
        public string TeamASide { get; set; }

        [Required]
        public string TeamBSide { get; set; }

        [Required]
        public int? TossWonBy { get; set; }

        [Required]
        public int? TossLostBy { get; set; }

        [Required]
        public string Decision {  get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TossID = null;
                MatchID = null;
                TeamASide = null;
                TeamBSide = null;
                TossWonBy = null;
                TossLostBy = null;
                Decision = null;
            }
        }
        ~Toss_Decision_VO()
        {
            Dispose(false);
        }
    }
}
