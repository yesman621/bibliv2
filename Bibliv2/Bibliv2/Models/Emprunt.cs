//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bibliv2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emprunt
    {
        public string idEmprunt { get; set; }
        public string idEx { get; set; }
        public string numCarte { get; set; }
        public System.DateTime dateEmprnt { get; set; }
        public System.DateTime dateMax { get; set; }
    
        public virtual Emprunteur Emprunteur { get; set; }
        public virtual Exemplaire Exemplaire { get; set; }
    }
}
