[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
sealed class ForeignKey : System.Attribute
{
    // See the attribute guidelines at
    //  http://go.microsoft.com/fwlink/?LinkId=85236
    // This is a positional argument
    public ForeignKey()
    {
    }
    
}