using System.Collections.Generic;
using Vispl.Training.Crickinfo.VO;

namespace Vispl.Training.Crickinfo.BM
{
    public interface IInnings_BM
    {
        List<BATTING_VO> BATTING_SCOREBOARD_1(string ID);
        List<BATTING_VO> BATTING_SCOREBOARD_2(string ID);
        List<BOWLING_VO> BOWLING_SCOREBOARD_1(string ID);
        List<BOWLING_VO> BOWLING_SCOREBOARD_2(string ID);
    }
}