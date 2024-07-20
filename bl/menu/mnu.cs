namespace bl.menu
{
    public class mnu
    {

        public class Menu
        {
            public string UI { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public string MenuListUI { get; set; }
            public class MenuList
            {
                public string UI { get; set; }
                public string Name { get; set; }
            }
        }

     
        /// UI 
        public static string Menu_Dashboard = "MN00";
        public static string Menu_Buy = "MN00";
        public static string Menu_Manufature = "MN02";
        public static string Menu_Sale = "MN03";
        public static string Menu_Category = "MN04";


        public static string Menu_Re_01 = "RMN01";
        public static string Menu_Re_02 = "RMN02";
        public static string Menu_Re_03 = "RMN03";

        //// Name
        public static string Menu_Name_Dashboard = "DASHBOARD";
        public static string Menu_Name_Buy = "Buy";
        public static string Menu_Name_Manufature = "MANUFATURE";
        public static string Menu_Name_Sale = "SALES";
        public static string Menu_Name_Category = "CATEGORIES";


        public static string Menu_Name_Re_01 = "RMN01";
        public static string Menu_Name_Re_02 = "RMN02";
        public static string Menu_Name_Re_03 = "RMN03";

        //// list
        public static string Menu_Data = "LS00";
        public static string Menu_Report = "LS01";



        //public static List<bl.menu.mnu.Menu> mnuMenuMain = new List<bl.menu.mnu.Menu>
        //{

        //};

        public static List<bl.menu.mnu.Menu> mnuMenuMain = new List<bl.menu.mnu.Menu>
        {
            /// Data
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI = bl.menu.mnu.Menu_Manufature, Name = bl.menu.mnu.Menu_Name_Manufature, Url = "/PCPMS/Data/Manufature/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI = bl.menu.mnu.Menu_Category, Name = bl.menu.mnu.Menu_Name_Category, Url = "/PCPMS/Data/Categories/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI= bl.menu.mnu.Menu_Sale, Name = bl.menu.mnu.Menu_Name_Sale, Url = "/PCPMS/Data/Sale/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Report, UI = bl.menu.mnu.Menu_Re_01, Name = bl.menu.mnu.Menu_Name_Re_01, Url = "/PCPMS/Data/Sale/Index"},
            /// one Menu
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Dashboard, Name = bl.menu.mnu.Menu_Name_Dashboard, Url = "/PCPMS/Dashboard/Index"},
            new bl.menu.mnu.Menu{ UI = bl.menu.mnu.Menu_Buy, Name = bl.menu.mnu.Menu_Name_Buy, Url = "/PCPMS/Buy/Index"},
        };

        public static string GetNamedta(string UI)
        {
            var getName = mnuMenuMain.Where(x => x.UI == UI)
                .Select(x => x.Name).FirstOrDefault();
            return getName;
        }

        public static string GetUrldta(string UI)
        {
            var getUrl = mnuMenuMain.Where(x => x.UI == UI)
                .Select(x => x.Url).FirstOrDefault();
            return getUrl;
        }






    }
}
