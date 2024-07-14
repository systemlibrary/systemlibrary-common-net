using System;

namespace SystemLibrary.Common.Net.Attributes;

/// <summary>
/// Mark a property as the Decrypted value of an Encrypted Configuration Property
/// 
/// <para>The property marked must be Public, Instance, Get and Set.</para>
/// 
/// <para>This attribute goes hand in hand with Config class, when a Config class is created it looks for which properties has this attribute and decrypts accordingly</para>
/// 
/// To decrypt properties must be encrypted through Encrypt() extension in this library that takes no Key/IV but the "default" youve specified
/// </summary>
/// <remarks>
/// The PropertyName must be a property within this same class, and class must inherit Config to work automatically
/// 
/// <para>Feel free to use this attribute in other scenarios as it most likely wont change name nor namespace in future versions</para>
/// 
/// But do note it is created to Decrypt an Encrypt Config Property  whenever the Config class is created and the decrypted value is a "singleton", it only decrypts once
/// </remarks>
/// <example>
/// Assume a config file:
/// <code>
/// api.json
/// {
///    token: "An encrypted value of the token, store it safely in git as it is encrypted by a key/IV only you know!"
/// }
/// </code>
/// Assume a cs file:
/// <code>
/// ApiConfig.cs
/// 
/// class ApiConfig : Config
/// {
///    public string Token {get;set;} //Encrypted value...
///    
///     public string TokenDecrypted {get;set;} // Auto decrypting based on name convention
///     public string TokenDecrypt {get;set;} // Auto decrypting based on name convention
///     
///     [Decrypt(propertyName="Token")]
///     public string TokenDec {get;set;} // Decrypt attribute specifies which property to decrypt
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DecryptAttribute : Attribute
{
    public string PropertyName;

    /// <summary>
    /// Set the property name that will be decrypted
    /// </summary>
    /// <param name="propertyName"></param>
    public DecryptAttribute(string propertyName = null)
    {
        PropertyName = propertyName;
    }
}
