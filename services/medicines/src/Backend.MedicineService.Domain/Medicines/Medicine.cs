using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Backend.MedicineService.Medicines;

public class Medicine : AuditedEntity<Guid>
{
    public string Name { get; set; }
    public DateTime ExpiredDate { get; set; }
}