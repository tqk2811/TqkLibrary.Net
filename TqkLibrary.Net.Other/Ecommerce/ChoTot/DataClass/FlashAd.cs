#if NET462_OR_GREATER
namespace TqkLibrary.Net.Ecommerce.ChoTot
{
    public class FlashAd
    {
#pragma warning disable IDE1006 // Naming Styles
        public string category { get; set; }
        public string subCategory { get; set; }
        public string type { get; set; }
        public string ward { get; set; }
        //public string street_number { get; set; }

        /// <summary>
        /// 1/0 => true/false
        /// </summary>
        //public string streetnumber_display { get; set; }

        /// <summary>
        /// 0: cá nhân<br/>
        /// 1: Môi giới
        /// </summary>
        public string company_ad { get; set; }
        //public string deposit { get; set; }
        public string size { get; set; }
        //public string width { get; set; }
        //public string length { get; set; }
        public string price { get; set; }
        public string land_type { get; set; }
        //public string direction { get; set; }
        public string property_legal_document { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string land_feature { get; set; }
        public string property_road_condition { get; set; }
        //public string size_unit { get; set; }
        public string address { get; set; }
        public string street_id { get; set; }
        public string app_id { get; set; } = "desktop_site_flashad";
        public string image_id0 { get; set; }
        public string image_id1 { get; set; }
        public string image_id2 { get; set; }
        public string image_id3 { get; set; }
        public string image_id4 { get; set; }
        public string image_id5 { get; set; }
        public string image_id6 { get; set; }
        public string image_id7 { get; set; }
        public string image_id8 { get; set; }
        public string image_id9 { get; set; }
        public string image_id10 { get; set; }
        public string image_id11 { get; set; }
        public string skip_verify_phone { get; set; } = "1";
        public string region_v2 { get; set; }
        public string area_v2 { get; set; }
        public string lang { get; set; } = "vi";
        public string rooms { get; set; }
        public string apartment_type { get; set; }
        public string area { get; set; }
        //public string balconydirection { get; set; }
        public string commercial_type { get; set; }
        //public string furnishing_rent { get; set; }
        //public string furnishing_sell { get; set; }
        public string house_type { get; set; }
        public string living_size { get; set; }
        public string property_status { get; set; }
        //public string ptyProject { get; set; }
        //public string region { get; set; }
        public string street { get; set; }
        //public string toilets { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        //public string block { get; set; }
        //public string floornumber { get; set; }
        //public string unitnumber { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
#endif