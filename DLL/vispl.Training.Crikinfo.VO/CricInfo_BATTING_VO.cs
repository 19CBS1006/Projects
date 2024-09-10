using System;

namespace Vispl.Training.Crickinfo.VO
{
    public class BATTING_VO : IDisposable
    {
        public byte[] BytePicture {  get; set; }
        public string BatsmanPicture { get; set; }
        public string BatsmanName { get; set; }
        public string TotalRuns { get; set; }
        public string TotalBalls { get; set; }
        public string TotalOvers { get; set; }
        public string FOUR { get; set; }
        public string SIX { get; set; }
        public decimal? StrikeRate { get; set; }
        public string CurrentRunRate { get; set; }
        public string BowlerName { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BytePicture = null;
                BatsmanPicture = null;
                BatsmanName = null;
                TotalRuns = null;
                TotalBalls = null;
                TotalOvers = null;
                FOUR = null;
                SIX = null;
                StrikeRate = null;
                CurrentRunRate = null;
                BowlerName = null;
            }
        }
        ~BATTING_VO()
        {
            Dispose(false);
        }
    }
}