using CsvHelper.Configuration;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EquipmentType;

namespace VianaHub.Global.Gerit.Application.Mappings.Csv;

public class BulkUploadEquipmentTypeItemMap : ClassMap<BulkUploadEquipmentTypeItem>
{
    public BulkUploadEquipmentTypeItemMap()
    {
        Map(m => m.Name).Name("Nome", "Name", "Nombre");
        Map(m => m.Description).Name("Descrição", "Description", "Descripción").Optional();
    }
}
