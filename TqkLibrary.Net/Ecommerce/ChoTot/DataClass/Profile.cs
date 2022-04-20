#if NET462_OR_GREATER
namespace TqkLibrary.Net.Ecommerce.ChoTot
{
    public class Profile
    {
        public int account_id { get; set; }
        public string account_oid { get; set; }
        public string address { get; set; }
        public string avatar { get; set; }
        public long create_time { get; set; }
        public string deviation { get; set; }
        public string email { get; set; }
        public string email_verified { get; set; }
        public string facebook_id { get; set; }
        public string facebook_token { get; set; }
        //public List<string> favorites {get; set;}
        public string full_name { get; set; }
        public bool is_active { get; set; }
        public string long_term_facebook_token { get; set; }
        public string old_phone { get; set; }
        public string phone { get; set; }
        public string phone_verified { get; set; }
        public long start_time { get; set; }
        public long update_time { get; set; }
    }
}
#endif