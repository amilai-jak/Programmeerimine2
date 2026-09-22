using Xunit;

namespace KooliProjekt.IntegrationTests.Helpers
{
    // Integratsioonitestid kasutavad uhist andmebaasi, seega ei tohi need
    // paralleelselt joosta
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class SequentialCollection
    {
    }
}
