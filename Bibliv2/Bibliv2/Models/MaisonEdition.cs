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
    
    public partial class MaisonEdition
    {
        public MaisonEdition()
        {
            this.Exemplaires = new HashSet<Exemplaire>();
        }
    
        public string idME { get; set; }
        public string nom { get; set; }
        public string ville { get; set; }
        public string pays { get; set; }
    
        public virtual ICollection<Exemplaire> Exemplaires { get; set; }
    }
}