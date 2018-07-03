namespace IGATimer
{
    class Session
    {
        public static bool IsSetTimer { get; set; }

        public static bool IsSetColor { get; set; } = true;

        public static string SelectedBgPath { get; set; } = "images/bg/bg1.jpg";

        public static string BgColorCode { get; set; } = "#26a6f2";

        public static bool IsBgColor { get; set; } = false;

        public static bool IsButtonColorBlack { get; set; } = false;

        public static string RemaniningTimeColorCode { get; set; } = "#ffffff";
    }
}
