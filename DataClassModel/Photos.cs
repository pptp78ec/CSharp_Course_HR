//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataClassModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photos
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public byte[] EmployeePhoto { get; set; }
        public string ImageFormat { get; set; }
        public Nullable<bool> DeletionFlag { get; set; }
    
        public virtual Employes Employes { get; set; }
    }
}
