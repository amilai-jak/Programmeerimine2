using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
