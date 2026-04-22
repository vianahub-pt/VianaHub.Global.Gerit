using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Helpers;

public static class ClientTypeHelper
{
    public static ClientType? FromDescription(string value)
    {
        foreach (var type in Enum.GetValues(typeof(ClientType)).Cast<ClientType>())
        {
            var description = type.GetDescription();

            if (description.Equals(value, StringComparison.OrdinalIgnoreCase))
                return type;
        }

        return null;
    }
}
