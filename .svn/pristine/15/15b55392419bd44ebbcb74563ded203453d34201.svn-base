using AsOrm3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{
    [DBLibrary(ZLibrary.SNS)]
    [DBTable("WAUSR02K")]
    public class WebAccessoriFornitori
    {
        [Key]
        [StringLength(3)]
        public string GRCUDF { get; set; }

        [Key]
        [StringLength(6)]
        public string GRCFOR { get; set; }

        [StringLength(35)]
        public string GRRAGS { get; set; }
        
    }
}