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
    
    public partial class Fires
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> CauseID { get; set; }
        public string CauseAddInfo { get; set; }
        public string FireOrderName { get; set; }
        public Nullable<System.DateTime> FireDate { get; set; }
        public Nullable<bool> DeletionFlag { get; set; }
        public Nullable<int> GeneralFireOrderId { get; set; }
    
        public virtual Employes Employes { get; set; }
        public virtual FireCauses FireCauses { get; set; }
        public virtual GeneralOrders GeneralOrders { get; set; }
    }
}
