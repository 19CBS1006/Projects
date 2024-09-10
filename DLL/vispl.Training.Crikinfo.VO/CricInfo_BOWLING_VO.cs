using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Training.Crickinfo.VO
{
    public class BOWLING_VO:IDisposable
    {
        public byte[] BytesPicture {  get; set; }
        public string BowlerPicture {  get; set; }
        public string BowlerId { get; set; }
        public string BowlerTeamId { get; set; }
        public string BowlerName { get; set; }
        public int? BallCount { get; set; }
        public int? RunsConceded { get; set; }
        public int? WicketsTaken { get; set; }
        public int? MaidenOvers { get; set; }
        public int? WicketType { get; set; }
        public float? EconomyRate { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BytesPicture = null;
                BowlerPicture = null;
                BowlerId = null;
                BowlerTeamId = null;
                BowlerName = null;
                BallCount = null;
                RunsConceded = null;
                WicketsTaken = null;
                MaidenOvers = null;
                EconomyRate = null;
            }
        }
        ~BOWLING_VO()
        {
            Dispose(false);
        }
    }
}
