using System;

[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public class PrimaryKey : System.Attribute
{
    // See the attribute guidelines at
    //  http://go.microsoft.com/fwlink/?LinkId=85236
    
    // This is a positional argument
    public PrimaryKey()
    {
        
    }
    
}