namespace bl.menu
{
    public class mnu
    {


        /// UI 
        public static string Menu_Dashboard = "MN00";
        public static string Menu_Parts = "MN01";
        public static string Menu_Sale = "MN02";
        public static string Menu_Category = "MN03";
        public static string Menu_Re_01 = "RMN01";
        public static string Menu_Re_02 = "RMN02";
        public static string Menu_Re_03 = "RMN03";

        //// Name
        public static string Menu_Name_Dashboard = "DASHBOARD";
        public static string Menu_Name_Parts = "PARTS";
        public static string Menu_Name_Sale = "SALES";
        public static string Menu_Name_Category = "CATEGORIES";
        public static string Menu_Name_Re_01 = "RMN01";
        public static string Menu_Name_Re_02 = "RMN02";
        public static string Menu_Name_Re_03 = "RMN03";




        List<bl.menu.mnu.Menu> mnuMenu = new List<bl.menu.mnu.Menu>
        {
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Dashboard, Name = bl.menu.mnu.Menu_Name_Dashboard, Url = "/PCPMS/Dashboard/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Parts, Name = bl.menu.mnu.Menu_Name_Parts, Url = "/PCPMS/Data/Part/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Sale, Name = bl.menu.mnu.Menu_Name_Sale, Url = "/PCPMS/Data/Sale/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Category, Name = bl.menu.mnu.Menu_Name_Category, Url = "/PCPMS/Data/Categories/Index"},
        };




        public class Menu
        {
            public string UI { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }


    }
}
