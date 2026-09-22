using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.WindowsForms
{
    // 20.03.2026 - opetaja naitest voetud OperationResult<T> klass
    [ExcludeFromCodeCoverage]
    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }

        public OperationResult() { }

        public OperationResult(T value)
        {
            Value = value;
        }

        public new OperationResult<T> AddError(string error)
        {
            base.AddError(error);

            return this;
        }

        public new OperationResult<T> AddPropertyError(string propertyName, string error)
        {
            base.AddPropertyError(propertyName, error);

            return this;
        }
    }
}
