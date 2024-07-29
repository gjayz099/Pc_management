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

     
        /// UI Data
        public static string Menu_Dashboard = "MN00";
        public static string Menu_Buy = "MN00";
        public static string Menu_Manufature = "MN02";
        public static string Menu_Sale = "MN03";
        public static string Menu_Category = "MN04";

        /// UI Report
        public static string Menu_P_S_C = "RMN01";
        public static string Menu_S_A_S_P = "RMN02";
        public static string Menu_P_S_C_S_B = "RMN03";
        public static string Menu_P_S_S_D = "RMN04";

        //// Data
        public static string Menu_Name_Dashboard = "DASHBOARD";
        public static string Menu_Name_Buy = "Buy";
        public static string Menu_Name_Manufature = "MANUFATURE";
        public static string Menu_Name_Sale = "SALES";
        public static string Menu_Name_Category = "CATEGORIES";
        //// Report
        public static string Menu_Name_P_S_C = "Products Specific Category"; ////Report for products in a specific category with additional details
        public static string Menu_Name_S_A_S_P = "Sales Specific Product"; //// Report for sales of a specific product in a date range
        public static string Menu_Name_P_S_C_S_B = "Products Specific Category With Stock Below"; ///// Report for products in a specific category with stock below a certain threshold
        public static string Menu_Name_P_S_S_D = "Products Sold Specific"; ////  Report for products sold in a specific date range

        //// list
        public static string Menu_Data = "LS00";
        public static string Menu_Report = "LS01";



        public static List<bl.menu.mnu.Menu> mnuMenuMain = new List<bl.menu.mnu.Menu>
        {
            /// Data
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI = bl.menu.mnu.Menu_Manufature, Name = bl.menu.mnu.Menu_Name_Manufature, Url = "/PCPMS/Data/Manufature/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI = bl.menu.mnu.Menu_Category, Name = bl.menu.mnu.Menu_Name_Category, Url = "/PCPMS/Data/Categories/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Data, UI= bl.menu.mnu.Menu_Sale, Name = bl.menu.mnu.Menu_Name_Sale, Url = "/PCPMS/Data/Sale/Index"},
            /// Report
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Report, UI = bl.menu.mnu.Menu_P_S_C, Name = bl.menu.mnu.Menu_Name_P_S_C, Url = "/PCPMS/Report/PSC/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Report, UI = bl.menu.mnu.Menu_P_S_S_D, Name = bl.menu.mnu.Menu_Name_P_S_S_D, Url = "/PCPMS/Report/PSSD/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Report, UI = bl.menu.mnu.Menu_P_S_C_S_B, Name = bl.menu.mnu.Menu_Name_P_S_C_S_B, Url = "/PCPMS/Report/PSCSB/Index"},
            new bl.menu.mnu.Menu{ MenuListUI = bl.menu.mnu.Menu_Report, UI = bl.menu.mnu.Menu_S_A_S_P, Name = bl.menu.mnu.Menu_Name_S_A_S_P, Url = "/PCPMS/Report/SASP/Index"},

     
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
