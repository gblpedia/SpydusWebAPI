using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace SpydusWebAPI.Controllers
{
    public class Validator
    {
        public bool isNidValid(string nid)
        {
            bool valid = false;

            // TODO - Implementation of National/Residential ID checking rules.
            if (nid.Length == 10)
            {
                valid = (Regex.IsMatch(nid, @"[a-zA-Z][0-9]+") || Regex.IsMatch(nid, @"[a-zA-Z][a-zA-Z][0-9]+"));
            }
            
            
            return valid;
        }

        public bool isBcodeValid(string bcode) {
            bool valid = false;
            // TODO - Implementation of BRW BarCode checking rules.

            return valid;
        }
    }
}