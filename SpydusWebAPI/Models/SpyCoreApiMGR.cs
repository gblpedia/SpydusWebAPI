using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Vixen.CoreDB;
using Vixen.CoreUtility;

namespace SpydusWebAPI.Models
{
    public class SpyCoreApiMGR
    {

        public static VxnDB VxnServer;

        public const string AWZREQ = @"/cgi-bin/spydus.exe/PGM/CAT/AWZREQ";
        public const string XMLHEAD = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        public const string AWRCRECUPDPRSBEGIN = "<AWZREQ ROUTINE=\"AW.RCRECUPDPRS\"><RECORDS>";
        public const string AWRCRECUPDPRSEND = @"</RECORDS></AWZREQ>";
        public const string AWREQGETSETBEGIN = "<AWZREQ ROUTINE=\"AW.REQGETSET\"><SQ>";
        public const string AWREQGETSETEND = @"</SQ></AWZREQ>";
        public static string SQBRWBDN = @"BDN01\{0}";   //"BRL01\> BRWBDN: {0}";
        public static string SQBRWNID = @"NBI01\{0}";   //"BRL01\< (BRWNID: {0})";


        public static string GetAppSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch { }

            return string.Empty;
        }


        //public static VxnDB GetVxnDB()
        static SpyCoreApiMGR()
        {
            

            //if ( VxnServer != null)
                //return VxnServer;



            string instURL = GetAppSetting("ServerURL");

            if (!string.IsNullOrEmpty(instURL))
            {
                //string error = "";
                string userName = !string.IsNullOrEmpty(GetAppSetting("Username")) ? GetAppSetting("Username").Trim() : string.Empty;
                string staffPwd = !string.IsNullOrEmpty(GetAppSetting("Password")) ? GetAppSetting("Password").Trim() : string.Empty;
                string contextName = "WCFTEST";

                string strippedUsername = Vixen.CoreUtility.SpyStripLib.spec_strip(userName, Vixen.CoreUtility.SpyStripLib.striptype.INDEX_STRIP);
                strippedUsername = strippedUsername.Replace(" ", "");
                strippedUsername = strippedUsername.ToLower();

                // Connect to Spydus Server
                VxnServer = VxnDBMgr.Instance.GetVxnDB(instURL, userName, contextName);

                if (VxnServer != null && VxnServer.Info.PasswordSpydus != staffPwd)
                {
                    //TODO return error message
                    Console.WriteLine("Failed to Connect Spydus Server " + instURL + "with Wrong Password " + staffPwd);
                    VxnServer = null;
                }
                else if (VxnServer == null || !VxnServer.IsValidConnection)
                {
                    Vixen.CoreUtility.ProductInfo.Name = "WCF";
                    VxnDB.ASP = true;

                    string message = string.Empty;
                    VxnServer = VxnDBMgr.Instance.InitSilent(contextName, instURL, "", "WCF", strippedUsername, staffPwd, "", "", false, false, "", out message);

                }

                if (VxnServer == null)
                {
                    //TODO return error message
                    Console.WriteLine("Failed to Connect Spydus Server " + instURL);
                }
            }
            else
            {
                //TODO return error message
                Console.WriteLine("Can not find ServerURL in Web.config!");
            }

            //return VxnServer;
        }
    }



    // Borrower Data
    public class Borrower {

        public string name { get; set; }
        public string bcode { get; set; }
        public string pin { get; set; }
        public string nid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string dob { get; set; }
        public string sid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }


    }
}