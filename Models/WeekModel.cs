namespace swf.Models
{
    public class WeekModel
    {
        public Guid IdWeek { get; set; }
        public short WeekNo { get; set; }
        public int YearNo { get; set; }
        public bool IsSetted { get; set; }
        public WeekModel() { }
        public WeekModel(Guid idWeek, short weekNo, int yearNo, bool isSetted)
        {
            IdWeek = idWeek;
            WeekNo = weekNo;
            YearNo = yearNo;
            IsSetted = isSetted;
        }
    }
}
