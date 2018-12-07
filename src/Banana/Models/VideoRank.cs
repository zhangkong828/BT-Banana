namespace Banana.Models
{
    public class VideoRank
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Classify { get; set; }

        public double Score { get; set; }

        public string Image { get; set; }
        public string Remark { get; set; }
        public string Starring { get; set; }
    }
}
