using AsOrm3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRestAs400.Models
{

    [DBLibrary(ZLibrary.SNS)]
    [DBTable("LMPICXVF")]
    public class LuaMpIncremento
    {
        [Display(Name = "UdB ultimo agg.", Description = "CHAR 3 ")]
        public string PPCUDB { get; set; }
        [Display(Name = "Utente ultimo agg.", Description = "CHAR 10 ")]
        public string PPCUTE { get; set; }
        [Display(Name = "Data ultimo agg.", Description = "NUMERIC 8 0")]
        public decimal PPFECH { get; set; }
        [Display(Name = "Ora ultimo agg.", Description = "NUMERIC 6 0")]
        public decimal PPHORA { get; set; }
        [Display(Name = "Numero ultima az. agg.", Description = "DECIMAL 9 0")]
        public decimal PPNJOB { get; set; }
        [Display(Name = "Pgm ultimo agg.", Description = "CHAR 10 ")]
        public string PPNPGM { get; set; }
        [Display(Name = "Attivo/annullato", Description = "CHAR 1 ")]
        public string A001XV { get; set; }
        [Display(Name = "Flag sospensione", Description = "CHAR 1 ")]
        public string A002XV { get; set; }
        [Display(Name = "UdB propietaria record", Description = "CHAR 3 ")]
        public string A003XV { get; set; }
        [Display(Name = "UdB Sistema 1°resident", Description = "CHAR 3 ")]
        public string A004XV { get; set; }
        [Display(Name = "UdB Sistema residente", Description = "CHAR 3 ")]
        public string A005XV { get; set; }
        [Display(Name = "UdB Sistema destino", Description = "CHAR 3 ")]
        public string A006XV { get; set; }
        [Display(Name = "Udb listino", Description = "CHAR 3 ")]
        [Key]
        public string XVCUDB { get; set; }
        [Display(Name = "Udbfornitore", Description = "CHAR 3 ")]
        [Key]
        public string XVUDBF { get; set; }
        [Display(Name = "Codice fornitore", Description = "CHAR 6 ")]
        [Key]
        public string XVCFOR { get; set; }
        [Display(Name = "Stagione", Description = "CHAR 1 ")]
        [Key]
        public string XVCSTG { get; set; }
        [Display(Name = "Codice Listino UNI ACQ", Description = "CHAR 6 ")]
        [Key]
        public string XVCLUA { get; set; }
        [Display(Name = "Codice concetto", Description = "CHAR 1 ")]
        [Key]
        public string XVCCON { get; set; }
        [Display(Name = "Valore concetto", Description = "CHAR 6 ")]
        [Key]
        public string XVVCON { get; set; }

        [Display(Name = "Valore incremento", Description = "NUMERIC 13 2")]
        public decimal XVVINC { get; set; }
        [Display(Name = "Tipo incremento", Description = "CHAR 1 ")]
        public string XVTINC { get; set; }
    }
}