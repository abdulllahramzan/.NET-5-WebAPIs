namespace myapp.Dtos.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; }
        public string Opponent { get; set; }
        public int AttackHp { get; set; }
        public int OpponentHp { get; set; }
        public int Damage { get; set; }
    }
}