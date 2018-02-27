namespace BoxOffice.Model
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool IsSelected { get; set; }

        public string Permissions { get; set; }
    }
}
