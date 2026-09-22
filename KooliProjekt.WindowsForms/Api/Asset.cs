namespace KooliProjekt.WindowsForms
{
    // 19.03.2026 - API mudel, mida Windows Forms programm kasutab
    public class Asset
    {
        public int Id { get; set; }
        public int AssetClassID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public bool IsRealEstate { get; set; }
        public string AssetClassName { get; set; }
    }
}
