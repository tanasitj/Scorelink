//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scorelink.MO
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentDetail()
        {
            this.DocumentAreas = new HashSet<DocumentArea>();
        }
    
        public int DocDetId { get; set; }
        public int DocId { get; set; }
        public string DocPageNo { get; set; }
        public string FootnoteNo { get; set; }
        public string PageType { get; set; }
        public string ScanStatus { get; set; }
        public string PageFileName { get; set; }
        public string PagePath { get; set; }
        public string PageUrl { get; set; }
        public string Selected { get; set; }
        public string PatternNo { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentArea> DocumentAreas { get; set; }
        public virtual DocumentInfo DocumentInfo { get; set; }
    }
}
