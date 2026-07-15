using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IPartyTypeTranslationDataRepository
{
    Task<PartyTypeTranslationEntity> GetByIdAsync(byte partyTypeId, string languageCode, CancellationToken ct);
    Task<IEnumerable<PartyTypeTranslationEntity>> GetByPartyTypeIdAsync(byte partyTypeId, CancellationToken ct);
    Task<bool> ExistsByPartyTypeAndLanguageAsync(byte partyTypeId, string languageCode, CancellationToken ct);
    Task<bool> ExistsByLanguageAndNameAsync(string languageCode, string name, CancellationToken ct);

    Task<bool> AddAsync(PartyTypeTranslationEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(PartyTypeTranslationEntity entity, CancellationToken ct);
}
