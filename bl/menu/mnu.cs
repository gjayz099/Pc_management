namespace bl.menu
{
    public class mnu
    {

        public class Menu
        {
            public string UI { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        /// UI 
        public static string Menu_Dashboard = "MN00";
        public static string Menu_Manufature = "MN01";
        public static string Menu_Sale = "MN02";
        public static string Menu_Category = "MN03";
        public static string Menu_Re_01 = "RMN01";
        public static string Menu_Re_02 = "RMN02";
        public static string Menu_Re_03 = "RMN03";

        //// Name
        public static string Menu_Name_Dashboard = "DASHBOARD";
        public static string Menu_Name_Manufature = "MANUFATURE";
        public static string Menu_Name_Sale = "SALES";
        public static string Menu_Name_Category = "CATEGORIES";
        public static string Menu_Name_Re_01 = "RMN01";
        public static string Menu_Name_Re_02 = "RMN02";
        public static string Menu_Name_Re_03 = "RMN03";


      
        public static List<bl.menu.mnu.Menu> mnuMenuList = new List<bl.menu.mnu.Menu>
        {
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Manufature, Name = bl.menu.mnu.Menu_Name_Manufature, Url = "/PCPMS/Data/Manufature/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Category, Name = bl.menu.mnu.Menu_Name_Category, Url = "/PCPMS/Data/Categories/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Sale, Name = bl.menu.mnu.Menu_Name_Sale, Url = "/PCPMS/Data/Sale/Index"},
        };



        public string GetName(string ID)
        {
            var getName = mnuMenuList.Where(x => x.UI == ID)
                .Select(x => x.Name).FirstOrDefault();
            return getName;
        }

        public string GetUrl(string ID)
        {
            var getUrl = mnuMenuList.Where(x => x.UI == ID)
                .Select(x => x.Url).FirstOrDefault();
            return getUrl;
        }





    }
}
