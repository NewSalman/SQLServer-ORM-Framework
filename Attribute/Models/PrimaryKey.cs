using System;

[System.AttributeUsage(System.AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public class PrimaryKey : System.Attribute
{
    // See the attribute guidelines at
    //  http://go.microsoft.com/fwlink/?LinkId=85236
    
    public enum IDTYPE
    {
        String,
        Int
    }
    // This is a positional argument
    public PrimaryKey()
    {
        
    }
    
}