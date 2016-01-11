using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Mvc;
using System.Web.Security;

namespace SWX.Models
{
    public class AreaID_VModels
    {
        public int Id { get; set; }

        public int AreaID { get; set; }

        public string NameEN { get; set; }

        public string NameCN { get; set; }

        public string DistrictEN { get; set; }

        public string DistrictCN { get; set; }

        public string ProvEN { get; set; }

        public string ProvCN { get; set; }

        public string NationEN { get; set; }

        public string NationCN { get; set; }
    }
}