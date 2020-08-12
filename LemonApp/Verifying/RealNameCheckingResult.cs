namespace Clansty.tianlang
{
    class RealNameCheckingResult
    {
        public RealNameCheckingResult(RealNameStatus rns, long? occupiedQQ = null)
        {
            Status = rns;
            OccupiedQQ = occupiedQQ;
        }
        public RealNameStatus Status { get; }
        public long? OccupiedQQ { get; }
    }
}
