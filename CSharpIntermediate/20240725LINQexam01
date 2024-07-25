namespace _20240725_LinqExam01
{
    //객체, 정우성:186, 김태희:158, ...
    class Profile
    {
        private string name;
        private int height;
        public string Name { get; set; }
        public int Height { get; set; }

        //생성자
        public Profile() { }
        public Profile(string name, int height) { Name = name; Height = height; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Profile[] arrProfiles =
            {
                new Profile() {Name="정우성", Height=186},
                new Profile() {Name="김태희", Height=158},
                new Profile() {Name="고현정", Height=172},
                new Profile("이문세", 178), new Profile("하동훈",171)

            };
            List<Profile> profiles = new List<Profile>();

            foreach (Profile profile in arrProfiles)
            {
                // 키가 175 이하인 사람은?
                if (profile.Height <= 175) { profiles.Add(profile); }
            }

            // LINQ 사용
            var myProfiles = from profile in arrProfiles
                             where profile.Height <= 175
                             orderby profile.Height descending
                             select profile;

            foreach (Profile profile in myProfiles)
            {
                Console.WriteLine($"{profile.Name} : {profile.Height}");
            }
        }
    }
}
