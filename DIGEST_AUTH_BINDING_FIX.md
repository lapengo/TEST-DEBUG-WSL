# Digest Authentication Binding Fix - Technical Guide

## 🎯 Problem

### Error Message:
```
The HTTP request is unauthorized with client authentication scheme 'Anonymous'. 
The authentication header received from the server was 'Digest realm="DataExchangeService", 
nonce="...", algorithm=MD5, qop="auth", Digest realm="DataExchangeService", 
nonce="...", algorithm=SHA-256, qop="auth"'
```

### What This Means:
- **Client sending:** Anonymous authentication (no credentials)
- **Server expecting:** Digest authentication (MD5 or SHA-256)
- **Result:** 401 Unauthorized error

---

## 🔍 Root Cause Analysis

### The Problem Sequence:

1. **Auto-generated WSDL binding** (Reference.cs):
   ```csharp
   var httpBindingElement = new HttpTransportBindingElement();
   httpBindingElement.AllowCookies = true;
   httpBindingElement.MaxBufferSize = int.MaxValue;
   httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
   // NO AuthenticationScheme set - defaults to Anonymous!
   ```

2. **Credentials were set** (in DataExchangeService):
   ```csharp
   _client.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
   _client.ClientCredentials.HttpDigest.ClientCredential.Password = password;
   ```

3. **But the binding didn't know to use them!**
   - The `HttpTransportBindingElement.AuthenticationScheme` defaults to `Anonymous`
   - Even with credentials set, the HTTP transport layer uses Anonymous auth
   - Credentials are ignored

### Why This Happens:
WCF separates **transport configuration** (binding) from **credential configuration** (ClientCredentials). You need BOTH:
- ✅ Set credentials on `ClientCredentials.HttpDigest`
- ✅ Tell transport to use Digest: `AuthenticationScheme = Digest`

---

## ✅ The Fix

### Complete Solution:

```csharp
public DataExchangeService(string serviceUrl, string? username = null, string? password = null)
{
    // Step 1: Create custom binding with proper AuthenticationScheme
    var binding = CreateCustomBinding();
    var endpoint = new System.ServiceModel.EndpointAddress(serviceUrl);
    
    _client = new DataExchangeClient(binding, endpoint);

    // Step 2: Set credentials
    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
    {
        _client.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
        _client.ClientCredentials.HttpDigest.ClientCredential.Password = password;
    }
}

private static System.ServiceModel.Channels.CustomBinding CreateCustomBinding()
{
    var binding = new System.ServiceModel.Channels.CustomBinding();
    
    // Text message encoding (same as auto-generated)
    var textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
    textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(
        System.ServiceModel.EnvelopeVersion.Soap12, 
        System.ServiceModel.Channels.AddressingVersion.None);
    binding.Elements.Add(textBindingElement);
    
    // HTTP transport with Digest authentication
    var httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
    httpBindingElement.AllowCookies = true;
    httpBindingElement.MaxBufferSize = int.MaxValue;
    httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
    
    // THE KEY FIX - Set AuthenticationScheme to Digest!
    httpBindingElement.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
    
    binding.Elements.Add(httpBindingElement);
    
    return binding;
}
```

### What Changed:

**Before:**
```csharp
_client = new DataExchangeClient(
    DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,  // Uses auto-generated binding
    serviceUrl
);
// Auto-generated binding has AuthenticationScheme = Anonymous (default)
```

**After:**
```csharp
var binding = CreateCustomBinding();  // Custom binding with Digest auth
var endpoint = new System.ServiceModel.EndpointAddress(serviceUrl);
_client = new DataExchangeClient(binding, endpoint);
// Custom binding has AuthenticationScheme = Digest
```

---

## 🔧 Technical Details

### AuthenticationScheme Options:

```csharp
public enum AuthenticationSchemes
{
    None = 0,
    Digest = 1,           // ← We use this
    Negotiate = 2,
    Ntlm = 4,
    Basic = 8,
    Anonymous = 32768     // ← Auto-generated default
}
```

### HTTP Digest Authentication Flow:

1. **Client** → Server: Initial request (no auth)
2. **Server** → Client: 401 + WWW-Authenticate: Digest realm="...", nonce="..."
3. **Client** → Server: Request with Authorization: Digest username="...", response="..." (hash)
4. **Server** → Client: 200 OK (if credentials valid)

**Key Point:** Step 3 only happens if `AuthenticationScheme = Digest` is set!

---

## 📊 Comparison

### Authentication Scheme Matrix:

| Scheme | Credentials Sent | Encryption | Use Case |
|--------|------------------|------------|----------|
| Anonymous | None | N/A | Public endpoints |
| Basic | Base64(user:pass) | ❌ None | Simple auth (HTTPS only) |
| Digest | Hash(user:pass:nonce) | ✅ Hashed | Secure without SSL |
| NTLM | Windows token | ✅ Encrypted | Windows integrated |
| Negotiate | Kerberos/NTLM | ✅ Encrypted | Enterprise Windows |

**Our Case:** Digest - secure authentication without requiring HTTPS

---

## 🧪 Testing

### Verify the Fix:

1. **Check binding configuration:**
   ```csharp
   var binding = CreateCustomBinding();
   var httpElement = binding.Elements.Find<HttpTransportBindingElement>();
   Console.WriteLine($"AuthenticationScheme: {httpElement.AuthenticationScheme}");
   // Should output: Digest (not Anonymous)
   ```

2. **Monitor HTTP traffic:**
   - First request: No Authorization header
   - Server response: 401 + WWW-Authenticate: Digest
   - Second request: Authorization: Digest (with hash)
   - Server response: 200 OK

3. **Check for success:**
   - No 401 errors
   - SOAP response received
   - Data populated correctly

---

## 📝 Best Practices

### 1. Always Match Transport and Credentials:

```csharp
// If using Digest credentials...
_client.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
_client.ClientCredentials.HttpDigest.ClientCredential.Password = password;

// ...ensure binding uses Digest scheme
httpBindingElement.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;
```

### 2. For Different Auth Schemes:

**Basic Authentication:**
```csharp
// Credentials
_client.ClientCredentials.UserName.UserName = username;
_client.ClientCredentials.UserName.Password = password;

// Binding
httpBindingElement.AuthenticationScheme = System.Net.AuthenticationSchemes.Basic;
```

**NTLM/Windows:**
```csharp
// Credentials (use default credentials)
_client.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

// Binding
httpBindingElement.AuthenticationScheme = System.Net.AuthenticationSchemes.Ntlm;
```

### 3. Security Considerations:

- ✅ **Digest over HTTP:** OK - credentials are hashed
- ⚠️ **Basic over HTTP:** INSECURE - use HTTPS only
- ✅ **NTLM:** Secure - uses Windows encryption
- ✅ **Always validate server certificates for HTTPS**

---

## 🚀 Migration Guide

### If You Have Existing Code:

**Old (Broken):**
```csharp
var client = new DataExchangeClient(
    DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,
    serviceUrl
);
client.ClientCredentials.HttpDigest.ClientCredential.UserName = username;
client.ClientCredentials.HttpDigest.ClientCredential.Password = password;
// Still uses Anonymous authentication!
```

**New (Working):**
```csharp
var service = new DataExchangeService(serviceUrl, username, password);
// Properly configured with Digest authentication
```

---

## 📚 References

- [WCF Transport Security](https://learn.microsoft.com/en-us/dotnet/framework/wcf/feature-details/transport-security)
- [HTTP Authentication Schemes](https://tools.ietf.org/html/rfc2617)
- [AuthenticationSchemes Enum](https://learn.microsoft.com/en-us/dotnet/api/system.net.authenticationschemes)

---

## ✅ Summary

**The Problem:**
- Auto-generated binding used `Anonymous` authentication
- Credentials were set but not used

**The Solution:**
- Create custom binding with `AuthenticationScheme = Digest`
- Maintains all other binding settings
- Properly utilizes configured credentials

**The Result:**
- ✅ Digest authentication working
- ✅ Credentials properly sent
- ✅ Server accepts requests
- ✅ No more 401 errors

**Key Takeaway:** In WCF, you must configure BOTH the credentials AND the transport binding's authentication scheme to match!
