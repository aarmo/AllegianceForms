namespace AllegianceForms.Engine
{
    public class GameAlert
    {
        public int SectorId { get; set; }
        public string Message { get; set; }

        public GameAlert(int sectorId, string message)
        {
            SectorId = sectorId;
            Message = message;
        }
    }
}
