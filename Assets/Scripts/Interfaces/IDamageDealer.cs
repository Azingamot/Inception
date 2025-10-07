internal interface IDamageDealer
{
    public float DamageAmount { get; }
    public DamageType[] TypesOfDamage { get; }
}
