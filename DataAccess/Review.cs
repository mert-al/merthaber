//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using DataAccess.Models;
    using System;
    using System.Collections.Generic;
    
    public partial class Review : BaseEntity
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public Nullable<int> News_Id { get; set; }
        public Nullable<int> Album_Id { get; set; }
        public Nullable<int> Video_Id { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual News News { get; set; }
        public virtual Video Video { get; set; }
    }
}
