using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SpydusWebAPI.Models;

namespace SpydusWebAPI.Models
{
    public class PatronModel
    {
        private string postXMLStr = string.Empty;
        private string result = string.Empty;
        private string response = string.Empty;
        private string error = string.Empty;
        private XDocument xmldoc;
        

        public string isValid (string bcode, string natid, string password) {

            if (SpyCoreApiMGR.VxnServer == null) {
                result = "Server Failed";
            }
            else {
                if (string.IsNullOrEmpty(postXMLStr))
                    postXMLStr = string.Empty;

                if (!string.IsNullOrEmpty(bcode)){
                    postXMLStr = SpyCoreApiMGR.AWREQGETSETBEGIN + string.Format(SpyCoreApiMGR.SQBRWBDN, bcode) + SpyCoreApiMGR.AWREQGETSETEND;
                   
                } else {
                    postXMLStr = SpyCoreApiMGR.AWREQGETSETBEGIN + string.Format(SpyCoreApiMGR.SQBRWNID, natid) + SpyCoreApiMGR.AWREQGETSETEND;
                }
                
                SpyCoreApiMGR.VxnServer.PostSvr(SpyCoreApiMGR.AWZREQ, postXMLStr , out response, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    result = error;

                } else {
                   
                    xmldoc = XDocument.Parse(response);

                    var recsList = (from elm in xmldoc.Root.Descendants("RECORD").Elements("GRPS")
                                 select
                                     new
                                     {
                                         pin = (elm.Element("BDI") == null || elm.Element("BDI").Element("FLD").Element("ID") == null) ? "" : (string)elm.Element("BDI").Element("FLD").Element("ID").Value,
                                         name = (elm.Element("HEADING") == null || elm.Element("HEADING").Element("FLD").Element("HEAD") == null) ? "" : (string)elm.Element("HEADING").Element("FLD").Element("HEAD").Value,
                                         bcode = (elm.Element("BDN") == null || elm.Element("BDN").Element("FLD").Element("ID") == null) ? "" : (string)elm.Element("BDN").Element("FLD").Element("ID").Value,
                                         //mobile = (elm.Element("BDT") == null || elm.Element("BDT").Element("FLD").Element("NUMBER") == null) ? "" : (string)elm.Element("BDT").Element("FLD").Element("NUMBER").Value,
                                         //PhType = (elm.Element("BDT") == null || elm.Element("BDT").Element("FLD").Element("TYPE") == null) ? "" : (string)elm.Element("BDT").Element("FLD").Element("TYPE").Value,
                                         email = (elm.Element("BDE") == null || elm.Element("BDE").Element("FLD").Element("TEXT") == null) ? "" : (string)elm.Element("BDE").Element("FLD").Element("TEXT").Value,
                                         add1 = (elm.Element("BDA") == null || elm.Element("BDA").Element("FLD").Element("LINE1") == null) ? "" : (string)elm.Element("BDA").Element("FLD").Element("LINE1").Value,
                                         add2 = (elm.Element("BDA") == null || elm.Element("BDA").Element("FLD").Element("LINE2") == null) ? "" : (string)elm.Element("BDA").Element("FLD").Element("LINE2").Value,
                                         add3 = (elm.Element("BDA") == null || elm.Element("BDA").Element("FLD").Element("LINE3") == null) ? "" : (string)elm.Element("BDA").Element("FLD").Element("LINE3").Value,
                                         postcode = (elm.Element("BDA") == null || elm.Element("BDA").Element("FLD").Element("PCODE") == null) ? "" : (string)elm.Element("BDA").Element("FLD").Element("PCODE").Value,
                                         country = (elm.Element("BDA") == null || elm.Element("BDA").Element("FLD").Element("COUNTRY") == null) ? "" : (string)elm.Element("BDA").Element("FLD").Element("COUNTRY").Value,
                                         dob = (elm.Element("BDD") == null || elm.Element("BDD").Element("FLD").Element("DATE") == null || elm.Element("BDD").Element("FLD").Element("DATE").Attribute("DISP") == null) ? "" : (string)elm.Element("BDD").Element("FLD").Element("DATE").Attribute("DISP").Value,
                                         sid = (elm.Element("SBI") == null || elm.Element("SBI").Element("FLD").Element("ID") == null) ? "" : (string)elm.Element("SBI").Element("FLD").Element("ID").Value,
                                         nid = (elm.Element("NBI") == null || elm.Element("NBI").Element("FLD").Element("ID") == null) ? "" : (string)elm.Element("NBI").Element("FLD").Element("ID").Value
                                     }).ToList();



                    if (recsList != null && recsList.Count > 0)
                    {

                        var brw = new Borrower
                        {
                            name = recsList[0].name.ToString(),
                            bcode = recsList[0].bcode.ToString(),
                            pin = recsList[0].pin.ToString(),
                            //mobile = mob,
                            email = recsList[0].email.ToString(),
                            dob = recsList[0].dob.ToString(),
                            address1 = recsList[0].add1.ToString(),
                            address2 = recsList[0].add2.ToString(),
                            address3 = recsList[0].add3.ToString(),
                            postcode = recsList[0].postcode.ToString(),
                            country = recsList[0].country.ToString(),
                            nid = recsList[0].nid.ToString(),
                            sid = recsList[0].sid.ToString()
                        };


                        if (brw.pin == password)
                        {

                            result = "Valid";
                        }
                        else
                        {
                            result = "Wrong Password";
                        }

                    }
                    else {
                        result = "No This Patron";
                    }
                }
            }
                
            return result;
        }
    }
}