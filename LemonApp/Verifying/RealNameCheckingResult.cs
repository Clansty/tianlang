namespace Clansty.tianlang
{
    class RealNameCheckingResult
    {
        public RealNameCheckingResult(RealNameStatus rns, string occupiedQQ = null)
        {
            Status = rns;
            OccupiedQQ = occupiedQQ;
        }
        public RealNameStatus Status { get; }
        public string OccupiedQQ { get; }
    }
}
