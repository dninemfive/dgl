namespace d9.dgl;
internal class EvolutionRules
{
    /*
     * blue - "water"
	flows from high R to low R regions. naturally spreads out otherwise.
	change = negative difference
green - "life"
	spawns randomly with a probability based on water and heat. low nonzero populations grow by using water; high populations die off, releasing water. produces heat.
red - "energy"
	low energy and high energy energy regions both tend to grow.
	change = |diff|
     */

}
