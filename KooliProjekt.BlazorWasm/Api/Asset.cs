namespace KooliProjekt.BlazorWasm
{
    // 30.04.2026 - WinFormsi ja WPF-i projektis kasutatav klass
    public class Asset
    {
        public int Id { get; set; }
        public int AssetClassID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public bool IsRealEstate { get; set; }
    }
}
