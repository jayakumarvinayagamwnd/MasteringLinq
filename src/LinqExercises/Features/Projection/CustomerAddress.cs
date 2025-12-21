namespace LinqExercises.Features.Projection
{
    public record CustomerAddress(
	    string Name, 
	    string Address, 
	    string City, 
	    string State, 
	    string Country, 
	    string PostalCode);
}